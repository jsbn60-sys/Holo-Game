using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents any projectile in the game.
/// </summary>
[System.Serializable]
public class Projectile : Attack
{
	[SerializeField]
	private float speed;

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
	/// Checks for collision with enemies
	/// </summary>
	/// <param name="other">GameObject that was colliding</param>
	private void OnTriggerEnter(Collider other)
	{
		GameObject hit = other.gameObject;

		if (hit.tag.Equals("Enemy"))
		{
			onHit(hit.GetComponent<Unit>());
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
