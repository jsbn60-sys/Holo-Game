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
	[SerializeField] private float speed;
	[SerializeField] protected bool triggersOnGround;
	[SerializeField] protected bool triggersOnEnemy;
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
	public void setupProjectile(Vector3 forward)
	{
		GetComponent<Rigidbody>().velocity = (forward + new Vector3(0, 0.25f, 0)) * 3 * speed;
		transform.Rotate(0, 0, -60f);
		Destroy(gameObject, 3f);
	}

	/// <summary>
	/// Decides what to do, when the projectile hits a trigger.
	/// </summary>
	/// <param name="other">Trigger that was hit</param>
	protected void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals("AOEGround"))
		{
			return;
		}

		if (triggersOnEnemy && other.tag.Equals("Enemy")
		    || triggersOnWalls && !other.tag.Equals("Plane")
		    || triggersOnGround && other.tag.Equals("Plane"))
		{
			onTriggerHit(other);

			if ( other.tag.Equals("Enemy") && (amountOfEnemiesHit < pierceAmount))
			{
				amountOfEnemiesHit++;
				return;
			}
		}
		if (hitFX != null)
		{
			Instantiate(hitFX, new Vector3(transform.position.x,0,transform.position.z), Quaternion.identity);
		}
		Destroy(gameObject);
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
}
