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
		foreach(Effect effect in onHitEffects)
		{
			effect.startEffect(target);
		}
		target.getHit(dmg, onHitEffects);
	}
}
