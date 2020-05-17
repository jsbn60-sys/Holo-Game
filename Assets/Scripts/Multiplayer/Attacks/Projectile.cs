using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class represents any projectile in the game.
/// </summary>
public abstract class Projectile : Attack
{
	[SerializeField] protected GameObject hitFX;
	[SerializeField] protected float speed;
	[SerializeField] protected bool triggersOnGround;
	[SerializeField] protected bool triggersOnNpc;
	[SerializeField] protected bool triggersOnWalls;
	[SerializeField] protected int pierceAmount;

	protected int amountOfEnemiesHit;

	private void Start()
	{
		amountOfEnemiesHit = 0;
	}

	/// <summary>
	/// Sets up the projectile to fly in the wanted direction
	/// </summary>
	/// <param name="forward">Directon in which the projectile should fly</param>
	public void setupProjectile(Vector3 forward,float throwSpeed)
	{
		GetComponent<Rigidbody>().velocity = (forward + new Vector3(0, 0.25f, 0)) * 3 * speed * throwSpeed;
		transform.Rotate(0, 0, -60f);
		Destroy(gameObject, 10f);
	}

	/// <summary>
	/// Decides what to do, when the projectile hits a trigger.
	/// </summary>
	/// <param name="other">Trigger that was hit</param>
	protected void OnTriggerEnter(Collider other)
	{
		// workaround to prevent collision
		if (other.tag.Equals("AOEGround") || other.tag.Equals("Item"))
		{
			return;
		}

		if (!triggersOnNpc && other.tag.Equals("NPC")
		    || !triggersOnWalls && !(other.tag.Equals("Plane") || other.tag.Equals("NPC"))
		    || !triggersOnGround && other.tag.Equals("Plane"))
		{
			spawnHitFx();
			Destroy(gameObject);
		}
		else
		{
			onTriggerHit(other);
			if ( other.tag.Equals("NPC") && (amountOfEnemiesHit < pierceAmount))
			{
				amountOfEnemiesHit++;

				if (amountOfEnemiesHit >= pierceAmount)
				{
					spawnHitFx();
					Destroy(gameObject);
				}
			}
		}
	}


	protected abstract void onTriggerHit(Collider hit);

	/// <summary>
	/// Changes the amount of enemies a projectile can pierce.
	/// </summary>
	/// <param name="amount">Amount to change pierceAmount</param>
	public void changePierceAmount(int amount)
	{
		pierceAmount += amount;
	}

	/// <summary>
	/// Spawns HitFx if the projectile has one.
	/// </summary>
	private void spawnHitFx()
	{
		if (hitFX != null)
		{
			GameObject fx = Instantiate(hitFX, new Vector3(transform.position.x,0,transform.position.z), Quaternion.identity);
			NetworkServer.Spawn(fx);
		}
	}
}
