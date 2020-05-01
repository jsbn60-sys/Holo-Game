using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This class represents any projectile in the game that only hits one unit.
/// </summary>
public class SingleTargetProjectile : Projectile
{

	/// <summary>
	/// Hits an enemy on collision
	/// </summary>
	/// <param name="hit"></param>
	protected override void onTriggerHit(Collider hit)
	{
		onHit(hit.GetComponent<Unit>());
	}
}
