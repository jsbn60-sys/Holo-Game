using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	/// <param name="other">Any trigger that was hit</param>
	protected override void OnTriggerEnter(Collider other)
	{
		Collider[] inRangeEnemies = Physics.OverlapSphere(transform.position, radius, enemyLayer);

		foreach (Collider enemy in inRangeEnemies)
		{
			onHit(enemy.GetComponent<Unit>());
		}
		Destroy(gameObject);
	}
}
