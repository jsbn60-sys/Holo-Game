using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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

	[SerializeField] private float explosionForce;

	/// <summary>
	/// AOEProjectiles can trigger on anything but players.
	/// </summary>
	/// <param name="hit">Collider that was hit</param>
	/// <returns>Was the collider a valid target</returns>
	protected override bool hitValidTarget(Collider hit)
	{
		return !hit.tag.Equals("Player");
	}

	/// <summary>
	/// Get all enemies in range and hit them.
	/// The actual collider that was hit is irrelevant.
	/// </summary>
	/// <param name="hit">Any trigger that was hit</param>
	protected override void onTriggerHit(Collider hit)
	{
		Collider[] npcsInRange = Physics.OverlapSphere(transform.position, radius, npcLayer);
		foreach (Collider npc in npcsInRange)
		{
			npc.GetComponent<Rigidbody>().AddExplosionForce(explosionForce,this.transform.position,radius,0,ForceMode.Impulse);
			onHit(npc.GetComponent<Unit>());
		}
		Destroy(this.gameObject);
	}
}
