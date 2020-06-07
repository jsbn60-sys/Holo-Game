using System;
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
	[Header("Unit : Attributes")]
	[SerializeField] protected float maxHealth;
	[SerializeField] protected float maxShield;
	[SerializeField] protected Attack attack;
	[SerializeField] protected float attackRate;
	[SerializeField] protected float speed;
	[SerializeField] protected float jumpForce;
	[SerializeField] protected float pushForce;
	[SerializeField] protected float onTouchDmg;

	protected float health;
	protected float shield;
	protected bool isInvulnerable;
	protected bool isInvisible;
	protected bool isStunned;

	private const float POS_THRESHOLD = 1f;
	private const float ROT_THRESHOLD = 5f;
	protected float attackTimer;

	[Header("Unit : UI")]
	[SerializeField] private RectTransform healthBar;
	[SerializeField] private RectTransform shieldBar;
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
	/// Updates the units position/rotation on the network if it is inaccurate by a certain threshold.
	/// For players this means that all player positions/rotations are corrected, except the one of the local player,
	/// because he calls this function on all other clients, because his version of his playerObject is the
	/// authoritative one.
	/// For NPCs this means that all NPCs positions/rotations are corrected, except the one of the server,
	/// because he call this function on all other clients, because his version of the NPCs is the authoritative one.
	/// </summary>
	/// <param name="actualPos">The correct position of the unit</param>
	/// <param name="actualRot">The correct rotation of the unit</param>
	protected void checkNetworkPosition(Vector3 actualPos, Quaternion actualRot)
	{
		if (Vector3.Distance(actualPos,this.transform.position) >= POS_THRESHOLD)
		{
			this.transform.position = Vector3.Lerp(this.transform.position, actualPos, Time.deltaTime * speed);
		}

		if (Quaternion.Angle(actualRot, this.transform.rotation) >= ROT_THRESHOLD)
		{
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation,actualRot,Time.deltaTime * speed);
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
	/// Determines what happens on death.
	/// Has to be implemented by subclasses.
	/// </summary>
	protected abstract void onDeath();

	/// <summary>
	/// Determines if the unit has hit a target it can push.
	/// </summary>
	/// <param name="target">Target that was hit</param>
	/// <returns>Can this unit push this target</returns>
	protected abstract bool canPushTarget(Unit target);


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
	/// Checks if collison is a unit and if it is a pushable target.
	/// If pushForce > 0f -> push
	/// If onTouchDmg > 0f -> dealDamage
	/// </summary>
	/// <param name="other">Target that collided</param>
	protected void OnCollisionEnter(Collision other)
	{

		if (other.gameObject.GetComponent<Unit>() != null
		    && canPushTarget(other.gameObject.GetComponent<Unit>()))
		{
			if (pushForce > 0f)
			{
				Vector3 pushDirection = (other.gameObject.transform.position - this.transform.position).normalized;
				other.gameObject.GetComponent<Rigidbody>().AddForce(pushDirection * pushForce);
			}
			if (Math.Abs(onTouchDmg) > 0f)
			{
				other.gameObject.GetComponent<Unit>().CmdChangeHealth(onTouchDmg);
			}
		}
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
		healthBar.sizeDelta = new Vector2((health/maxHealth)*100, healthBar.sizeDelta.y);
	}

	/// <summary>
	/// This function updates the shieldBar size.
	/// </summary>
	private void UpdateShieldbarSize()
	{
		shieldBar.sizeDelta = new Vector2((shield/maxShield)*100, healthBar.sizeDelta.y);
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
		GameObject objectPrefab = LobbyManager.Instance.getPrefabAtIdx(prefabIdx);
		GameObject objectCopy = Instantiate(objectPrefab, this.transform.position, Quaternion.identity);
		NetworkServer.Spawn(objectCopy);

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
	/// Causes an explosion force to all nearby
	/// targets on the explosion layer.
	/// </summary>
	/// <param name="explosionForce">Strength of the explosion</param>
	/// <param name="explosionLayer">Layer of the explosion</param>
	public void explode(float explosionForce, LayerMask explosionLayer)
	{
		Collider[] npcsInRange = Physics.OverlapSphere(transform.position, 10, explosionLayer);
		foreach (Collider npc in npcsInRange)
		{
			npc.GetComponent<Rigidbody>().AddExplosionForce(explosionForce,this.transform.position,10,0,ForceMode.Impulse);
		}
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
    /// <param name="healthChangeAmount">Amount of health to change</param>
	/// </summary>
	[ClientRpc]
	public void RpcChangeHealth(float healthChangeAmount)
	{
		changeHealth(healthChangeAmount);
	}

	/// <summary>
	/// Tells all clients that this players shield changed.
	/// </summary>
	/// <param name="shieldChangeAmount">Amnount of shield to change</param>
	[Command]
	public void CmdGiveShield(float shieldChangeAmount)
	{
		RpcGiveShield(shieldChangeAmount);
	}

	/// <summary>
	/// Tells all clients that this players shield changed.
	/// </summary>
	/// <param name="shieldChangeAmount">Amnount of shield to change</param>
	[ClientRpc]
	public void RpcGiveShield(float shieldChangeAmount)
	{
		changeShield(shieldChangeAmount);
	}

	/// <summary>
	/// Spawns a given amount of rotating projectiles around the player.
	/// </summary>
	/// <param name="prefabIdx">Rotating projectile prefab</param>
	/// <param name="amount">Amount of projectiles to spawn</param>
	public void spawnRotatingProjectiles(GameObject projectilePrefab, int amount)
	{
		float degreeBetweenProjectiles = 360f / amount;

		for (int i = 0; i < amount; i++)
		{
			Vector3 spawnPos = this.transform.position + Quaternion.Euler(0, degreeBetweenProjectiles * i, 0)
				* this.transform.forward * 2;

			GameObject projectileCopy = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
			projectileCopy.GetComponent<RotatingProjectile>().Target = this.transform;
		}
	}

	/// <summary>
	/// Changes the pushforce of this unit by a given amount.
	/// </summary>
	/// <param name="amount">Amount to change pushforce by</param>
	public void changePushForce(float amount)
	{
		pushForce += amount;
	}

	/// <summary>
	/// Changes the onTouchDmg of this unit by a given amount.
	/// </summary>
	/// <param name="amount">Amount to change onTouchDmg by</param>
	public void changeOnTouchDmg(float amount)
	{
		onTouchDmg += amount;
	}
}
