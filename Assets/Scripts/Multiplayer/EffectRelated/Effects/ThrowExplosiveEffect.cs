using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect which throws an explosiveProjectile from the players position.
/// </summary>
public class ThrowExplosiveEffect : SingleUseEffect
{
	[SerializeField]
	private AOEProjectile explosionProjectile;

	protected override void execEffect()
	{
		target.GetComponent<Player>().shootProjectile(explosionProjectile.GetComponent<Projectile>());
	}
}
