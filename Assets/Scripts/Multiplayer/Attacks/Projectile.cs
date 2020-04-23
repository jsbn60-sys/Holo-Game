using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This class represents any projectile in the game.
/// </summary>
public abstract class Projectile : Attack
{
	[SerializeField] protected GameObject hitFX;
	[SerializeField] private float speed;

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
	protected abstract void OnTriggerEnter(Collider other);
}
