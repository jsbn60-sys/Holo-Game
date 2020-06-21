using System;
using System.Collections;
using System.Collections.Generic;
using Multiplayer.Lobby;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class represents an NPC that explodes once it reaches its target.
/// Self explosion is handled by killing the NPC and then causing a explosion when it is destroyed on the network.
/// </summary>
public class Exploding_NPC : NPC
{
	[Header("Exploding_NPC : Attributes")]
	[SerializeField] private float explosionForce;
	[SerializeField] private LayerMask explosionLayer;
	[SerializeField] private float explosionRadius;

	private float explosionTimer;

	/// <summary>
	/// Explode once in range.
	/// </summary>
	protected override void execCanAttackActions()
	{
		explosionTimer = 0.5f;
		StartCoroutine(setupExplosion());
	}

	/// <summary>
	/// Not needed.
	/// </summary>
	protected override void execTargetNotInRangeActions() { }

	/// <summary>
	/// Not needed.
	/// </summary>
	protected override void execInRangeActions() { }

	/// <summary>
	/// Coroutine that lets the NPC grow before destroying it.
	/// </summary>
	/// <returns>Coroutine</returns>
	private IEnumerator setupExplosion()
	{
		while(explosionTimer > 0f)
		{
			transform.localScale *= 1.003f;
			explosionTimer -= 0.1f;
			yield return  new WaitForSeconds(0.1f);
		}


		if (isServer)
		{
			CmdChangeHealth(-100000);
		}
	}

	/// <summary>
	/// Causes the NPCs to explode and deal damage to all nearby objects in the explosionLayer.
	/// </summary>
	private void selfExplode()
	{
		explode(explosionRadius,explosionForce, explosionLayer);
		Collider[] npcsInRange = Physics.OverlapSphere(transform.position, explosionRadius, explosionLayer);
		foreach (Collider npc in npcsInRange)
		{
			attack.onHit(npc.GetComponent<Unit>());
		}
	}

	/// <summary>
	/// Before destroying the NPC it explodes.
	/// </summary>
	private void OnDestroy()
	{
		selfExplode();
	}
}
