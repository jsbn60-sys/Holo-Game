using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class represents any Projectile that has an area of effect.
/// </summary>
public class AOEProjectile : Projectile
{
	[SerializeField]
	private LayerMask enemyLayer;

	[SerializeField]
	private float radius;

	/// <summary>
	/// Get all enemies in range and hit them.
	/// </summary>
	/// <param name="hit">Any trigger that was hit</param>
	protected override void onTriggerHit(Collider hit)
	{
		Collider[] inRangeEnemies = Physics.OverlapSphere(transform.position, radius, enemyLayer);

		foreach (Collider enemy in inRangeEnemies)
		{
			onHit(enemy.GetComponent<Unit>());
		}
	}
}
