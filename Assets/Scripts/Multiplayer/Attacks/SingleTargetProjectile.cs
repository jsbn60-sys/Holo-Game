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
	/// Hits enemy when colliding.
	/// </summary>
	/// <param name="other">Any trigger</param>
	protected override void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals("Enemy"))
		{
			onHit(other.GetComponent<Unit>());
		}
		else if (other.tag.Equals("AOEGround"))
		{
			return;
		}
		Destroy(gameObject);
	}
}
