using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class is the parent class to all attacks in the game
/// and gives basic functionality with damage and a list of on-hit-effects
/// which are applied on hit.
/// </summary>
public abstract class Attack : NetworkBehaviour
{
	[SerializeField]
	protected float dmg;

	[SerializeField]
	protected List<Effect> onHitEffects;

	private AudioClip sound;

	/// <summary>
	/// interface function for hitting the target with an attack
	/// </summary>
	/// <param name="target"></param>
	public void onHit(Unit target)
	{
		target.getHit(dmg);
		CmdAttackEffect(this.gameObject,target.gameObject);
	}

	public void changeDmg(bool increase, float amount)
	{
		if (increase)
		{
			dmg += amount;
		}
		else
		{
			dmg -= amount;
		}
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="attack"></param>
	/// <param name="target"></param>
	[Command]
	public void CmdAttackEffect(GameObject attack, GameObject target)
	{
		RpcAttackEffect(attack,target);
	}

	[ClientRpc]
	public void RpcAttackEffect(GameObject attack, GameObject target)
	{
		foreach (Effect effect in attack.GetComponent<Attack>().onHitEffects)
		{
			Effect.attachEffect(effect.gameObject,target.GetComponent<Unit>());
		}
	}
}
