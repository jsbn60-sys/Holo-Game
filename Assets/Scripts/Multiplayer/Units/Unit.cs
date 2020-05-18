using System.Collections;
using System.Collections.Generic;
using Leap;
using Multiplayer.Lobby;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Networking;

/// <summary>
/// This class is the parent class for all Players and NPCs.
/// It handels health & shield, attacks and effects.
/// </summary>
public abstract class Unit : NetworkBehaviour
{
	[SerializeField] protected float maxHealth;
	[SerializeField] protected float maxShield;

	protected float health;
	protected float shield;
	protected bool isInvulnerable;
	protected bool isInvisible;
	protected bool isStunned;

	[SerializeField] protected Attack attack;
	[SerializeField] protected float attackRate;
	[SerializeField] protected float speed;
	[SerializeField] protected float jumpForce;
	protected float attackTimer;

	[SerializeField] private RectTransform healthBar;
	[SerializeField] private RectTransform shieldBar;

	[SyncVar] public Vector3 forwardDirection;
	private float speedBeforeStun; //workaround: stores old speed while stunned

	public float Shield => shield;
	public bool IsInvisible => isInvisible;

	protected void Start()
	{
		isInvulnerable = false;
		isInvisible = false;
		attackTimer = attackRate;

		// initializes health & shield
		health = maxHealth;
		shield = 0f;
		UpdateHealthbarSize();
		UpdateShieldbarSize();
	}

	/// <summary>
	/// Updates timer for attacks.
	/// </summary>
	protected void Update()
	{
		if (attackTimer > 0f)
		{
			attackTimer -= Time.deltaTime;
		}
	}

	/// <summary>
	/// Resets attackTimer.
	/// </summary>
	protected void useAttack()
	{
		attackTimer = attackRate;
	}

	/// <summary>
	/// Returns if player can attack.
	/// </summary>
	/// <returns>Can the player attack</returns>
	protected bool readyToAttack()
	{
		return attackTimer <= 0f && !isStunned;
	}

	/// <summary>
	/// Returns if the unit has died.
	/// </summary>
	/// <returns>Is the unit dead.</returns>
	public bool isDead()
	{
		return health <= 0;
	}

	/// <summary>
	/// Determines what happens on death
	/// has to be implemented by subclasses.
	/// </summary>
	protected abstract void onDeath();


	/// <summary>
	/// Changes the players health.
	/// This can be damage or healing.
	/// Is used by the Attack class
	/// and various health related effects.
	/// </summary>
	/// <param name="amount">Amount of health to change</param>
	public void changeHealth(float amount)
	{
		if (amount >= 0)
		{
			health = Mathf.Min(health + amount, maxHealth);
		}
		else if (!isInvulnerable)
		{
			amount = -amount;

			float shieldOverflowDmg = -Mathf.Min(shield-amount,0);

			shield = Mathf.Max(shield - amount, 0);

			health -= shieldOverflowDmg;
			if (isDead())
			{
				onDeath();
			}
		}
		UpdateHealthbarSize();
		UpdateShieldbarSize();
	}

	/// <summary>
	/// Restores the unit to max health.
	/// </summary>
	public void revive()
	{
		changeHealth(maxHealth);
	}

	/// <summary>
	/// Adds shield to the player.
	/// </summary>
	/// <param name="shieldAmount">Amount of shield</param>
	public void changeShield(float shieldAmount)
	{
		shield = Mathf.Clamp(shield + shieldAmount, 0, maxShield);
		UpdateShieldbarSize();
	}

	/// <summary>
	/// This function updates the healthBar size.
	/// </summary>
	public void UpdateHealthbarSize()
	{
		healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
		RectTransform redHealthTransform = healthBar.GetComponentInParent<RectTransform>().parent.GetComponent<RectTransform>();
		redHealthTransform.sizeDelta = new Vector2(maxHealth, redHealthTransform.sizeDelta.y);
	}

	/// <summary>
	/// This function updates the shieldBar size.
	/// </summary>
	private void UpdateShieldbarSize()
	{
		shieldBar.sizeDelta = new Vector2(shield, healthBar.sizeDelta.y);
	}

	public Attack getAttack()
	{
		return attack;
	}

	/// <summary>
	/// Increases/decreases speed by a factor.
	/// </summary>
	/// <param name="increase">should increase</param>
	/// <param name="factor">increase/decrease factor</param>
	public void changeSpeed(bool increase, float factor)
	{
		if (increase)
		{
			this.speed *= factor;
		} else
		{
			this.speed /= factor;
		}
	}

	/// <summary>
	/// Changes if the unit is invulnerable or not.
	/// </summary>
	/// <param name="turnOn">Should unit be invulnerable</param>
	public void changeInvulnerability(bool turnOn)
	{
		isInvulnerable = turnOn;
	}

	/// <summary>
	/// Changes if the unit is stunned or not.
	/// </summary>
	/// <param name="turnOn">Should unit be stunned</param>
	public void changeStunned(bool turnOn)
	{
		isStunned = turnOn;

		if (turnOn)
		{
			speedBeforeStun = speed;
			speed = 0f;
		}
		else
		{
			speed = speedBeforeStun;
		}
	}

	/// <summary>
	/// WIP: Used to drop something on top of the player.
	/// </summary>
	/// <param name="objectToPlace">Object that should be placed</param>
	[Command]
	public void CmdPlaceObjectOnTop(int prefabIdx)
	{
		Instantiate(LobbyManager.Instance.getPrefabAtIdx(prefabIdx), transform.position, Quaternion.identity);

	}

	/// <summary>
	/// Runs on the servers to attach an effect on a player.
	/// </summary>
	/// <param name="prefabIdx">Index of RegisteredPrefab of the effect</param>
	[Command]
	public void CmdAttachEffect(int prefabIdx)
	{
		RpcAttachEffect(prefabIdx);
	}

	/// <summary>
	/// Runs on all clients to attach an effect on a player.
	/// </summary>
	/// <param name="prefabIdx">Index of RegisteredPrefab of the effect</param>
	[ClientRpc]
	public void RpcAttachEffect(int prefabIdx)
	{
		GameObject effect = LobbyManager.Instance.getPrefabAtIdx(prefabIdx);

		Effect.attachEffect(effect,gameObject.GetComponent<Unit>());
	}

	/// <summary>
	/// WIP: Function for attaching an effect to a unit.
	/// An effect is only attached if the unit is an enemy
	/// or if it is the localPlayer.
	/// He sends an RPC to all clients to attach the Effect locally.
	/// This way there are no duplicates.
	/// (NetworkServer.Spawn() might be used for this)
	/// </summary>
	/// <param name="effect">Effect that should be attached</param>
	public void attachEffect(Effect effect)
	{
		// Effects are only attached on enemies on the server
		if (this.tag.Equals("NPC") && !isServer)
		{
			return;
		}
		if (!this.tag.Equals("Dummy") && !this.tag.Equals("NPC") && (!isClient || !isLocalPlayer))
		{
			Debug.Log("DUMMY FAIL");
			return;
		}

		/*
		 * Workaround:
		 * PlaceObjectsEffects spawn objects on the server.
		 * But because effects are executed locally on each user
		 * the object would be spawned multiple times.
		 * This is way they are only executed locally.
		 */
		if (effect.GetComponent<PlaceObjectEffect>() == null)
		{
			CmdAttachEffect(LobbyManager.Instance.getIdxOfPrefab(effect.gameObject));
		}
		else
		{
			Effect.attachEffect(effect.gameObject,this);
		}
	}

	/// <summary>
	/// Changes the attackRate of the unit.
	/// </summary>
	/// <param name="amount">Amount to change</param>
	public void changeAttackRate(float factor)
	{
		attackRate *= factor;
	}

	/// <summary>
	/// Workaround for dealing damage with attacks, because attack damage is not handled over effects.
	/// </summary>
	/// <param name="healthChangeAmount">Amount of health to change</param>
	[Command]
	public void CmdChangeHealth(float healthChangeAmount)
	{
		RpcChangeHealth(healthChangeAmount);
	}

	/// <summary>
	/// Workaround for dealing damage with attacks, because attack damage is not handled over effects.
	/// </summary>
	[ClientRpc]
	public void RpcChangeHealth(float healthChangeAmount)
	{
		changeHealth(healthChangeAmount);
	}
}
