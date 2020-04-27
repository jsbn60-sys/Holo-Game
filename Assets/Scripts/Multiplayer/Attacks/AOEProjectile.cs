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
	/// <param name="other">Any trigger that was hit</param>
	protected override void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals("AOEGround"))
		{
			return;
		}

		if ((triggersOnEnemy && other.tag.Equals("Enemy"))
			|| triggersOnWalls && !other.tag.Equals("Plane")
		    || triggersOnGround && other.tag.Equals("Plane"))
		{
			if (hitFX != null)
			{
				Instantiate(hitFX, transform.position, Quaternion.identity);
			}

			Collider[] inRangeEnemies = Physics.OverlapSphere(transform.position, radius, enemyLayer);

			foreach (Collider enemy in inRangeEnemies)
			{
				onHit(enemy.GetComponent<Unit>());
			}
		}
		Destroy(gameObject);
	}
}
