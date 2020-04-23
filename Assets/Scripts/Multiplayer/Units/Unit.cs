using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class is the parent class for all Players and NPCs.
/// It handels health & shield, attacks and effects.
/// </summary>
public abstract class Unit : NetworkBehaviour
{
	[SerializeField]
	protected float maxHealth;
	[SerializeField]
	protected float maxShield;

	protected float health;
	protected float shield;
	protected bool isInvulnerable;

	[SerializeField]
	protected Attack attack;
	protected float attackRate;
	protected float speed;
	protected float jumpForce;
	protected float attackTimer;

	[SerializeField]
	private RectTransform healthBar;
	[SerializeField]
	private RectTransform shieldBar;

	protected void Start()
	{
		isInvulnerable = false;
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
	protected bool canAttack()
	{
		return attackTimer <= 0f;
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
	/// Function for an attack to hit the unit.
	/// Should only be called by the Attack class.
	/// </summary>
	/// <param name="dmg">Damage that is dealt.</param>
	/// <param name="onHitEffects">Hit effects applied to the player.</param>
	public void getHit(float dmg)
	{
		if (isInvulnerable)
		{
			return;
		}

		float shieldOverflowDmg = -Mathf.Min(shield-dmg,0);

		shield = Mathf.Max(shield - dmg, 0);

		health -= shieldOverflowDmg;
		if (isDead())
		{
			onDeath();
		}
		UpdateHealthbarSize();
		UpdateShieldbarSize();
	}

	/// <summary>
	/// Restores the unit to max health.
	/// </summary>
	public void revive()
	{
		heal(maxHealth);
	}

	/// <summary>
	/// Heals the player.
	/// </summary>
	/// <param name="healAmount">Amount of healing</param>
	public void heal(float healAmount)
	{
		health = Mathf.Min(health + healAmount, maxHealth);
		UpdateHealthbarSize();
	}

	/// <summary>
	/// Adds shield to the player.
	/// </summary>
	/// <param name="shieldAmount">Amount of shield</param>
	public void giveShield(float shieldAmount)
	{
		shield = Mathf.Min(shield + shieldAmount, maxShield);
		Debug.Log("Shield: " + shield);
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

	public float getSpeed()
	{
		return speed;
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

}
