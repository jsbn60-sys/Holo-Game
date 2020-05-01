using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class is the parent class to all attacks in the game
/// and gives basic functionality with damage and a list of on-hit-effects
/// which are applied on hit.
/// Dealing damage with an attack is not implemented over the HealthChangeEffect
/// as an onHitEffect, because that would make changing the damageValue more difficult.
/// </summary>
public abstract class Attack : NetworkBehaviour
{
	[SerializeField]
	protected float dmg;

	[SerializeField]
	protected float critChance;

	[SerializeField]
	protected List<Effect> onHitEffects;

	private AudioClip sound;

	/// <summary>
	/// interface function for hitting the target with an attack.
	/// Applies effect and deals damage with crit chance.
	/// </summary>
	/// <param name="target">Target to hit</param>
	public void onHit(Unit target)
	{
		float dmgToDeal = -dmg;

		if (Random.value < critChance)
		{
			dmgToDeal *= 2;
		}

		target.changeHealth(dmgToDeal);
		foreach (Effect effect in onHitEffects)
		{
			target.attachEffect(effect);
		}
	}

	/// <summary>
	/// Changes the damage of the attack.
	/// Used by DamageChangeEffect.
	/// </summary>
	/// <param name="amount">Amount of dmg to change</param>
	public void changeDmg(float amount)
	{
		dmg += amount;
	}

	/// <summary>
	/// Changes the critChance of the attack.
	/// Used by CritChanceChangeEffect.
	/// </summary>
	/// <param name="amount">Amount of critChance to change</param>
	public void changeCritChance(float amount)
	{
		critChance += amount;
	}
}
