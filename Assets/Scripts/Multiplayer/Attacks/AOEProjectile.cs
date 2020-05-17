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
	private LayerMask npcLayer;

	[SerializeField]
	private float radius;

	/// <summary>
	/// Get all enemies in range and hit them.
	/// </summary>
	/// <param name="hit">Any trigger that was hit</param>
	protected override void onTriggerHit(Collider hit)
	{
		Collider[] npcsInRange = Physics.OverlapSphere(transform.position, radius, npcLayer);

		foreach (Collider npc in npcsInRange)
		{
			onHit(npc.GetComponent<Unit>());
		}
	}
}
