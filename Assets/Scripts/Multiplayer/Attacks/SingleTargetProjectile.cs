using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This class represents any projectile in the game that only hits one unit.
/// </summary>
public class SingleTargetProjectile : Projectile
{
	[SerializeField] private int pierceAmount;
	private int amountOfEnemiesPierced;

	private void Start()
	{
		amountOfEnemiesPierced = 0;
	}

	/// <summary>
	/// SingleTargetProjectiles only trigger on NPCs.
	/// </summary>
	/// <param name="hit">Collider that was hit</param>
	/// <returns>Was the collider a valid target</returns>
	protected override bool hitValidTarget(Collider hit)
	{
		Debug.Log("HIT: " + hit.name);
		return hit.tag.Equals("NPC");
	}

	/// <summary>
	/// Hits an NPC on collision.
	/// </summary>
	/// <param name="hit">NPC that was hit</param>
	protected override void onTriggerHit(Collider hit)
	{
		onHit(hit.GetComponent<Unit>());

		if (amountOfEnemiesPierced >= pierceAmount)
		{
			GetComponent<Collider>().isTrigger = false;
			GetComponent<Rigidbody>().velocity = Random.insideUnitSphere * 5f;
			Destroy(this.gameObject,2f);
		}
		else
		{
			amountOfEnemiesPierced++;
		}
	}

	/// <summary>
	/// Changes the amount of enemies a projectile can pierce.
	/// </summary>
	/// <param name="amount">Amount to change pierceAmount</param>
	public void changePierceAmount(int amount)
	{
		pierceAmount += amount;
	}
}
