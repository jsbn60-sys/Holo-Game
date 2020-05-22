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
	[SerializeField] protected float speed;

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

		if (hitValidTarget(other))
		{
			onTriggerHit(other);
		}
		else
		{
			Destroy(this.gameObject);
		}
	}


	protected abstract bool hitValidTarget(Collider hit);
	protected abstract void onTriggerHit(Collider hit);
}
