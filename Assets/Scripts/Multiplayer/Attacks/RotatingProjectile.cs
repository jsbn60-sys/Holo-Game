using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class represents a projectile that rotates around a given target
/// and hits NPCs that it collides with.
/// </summary>
public class RotatingProjectile : Projectile
{
	private Transform target;

	public Transform Target
	{
		set => target = value;
	}

	private Vector3 relativeDistance;


	/// <summary>
	/// Start is called before the first frame update.
	/// </summary>
	void Start ()
	{
		if(target != null)
		{
			relativeDistance = transform.position - target.position;
		}
	}

	/// <summary>
	/// Update is called once per frame.
	/// Keeps the projectile rotating around the target.
	/// LateUpdate is used to keep projectile accurate.
	/// </summary>
	void LateUpdate ()
	{
		if(target != null)
		{
			transform.position = target.position + relativeDistance;
			transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);
			relativeDistance = transform.position - target.position;
		}
	}

	/// <summary>
	/// RotatingProjectiles only hit NPCs.
	/// </summary>
	/// <param name="hit">Collider that was hit</param>
	/// <returns>Was the collider a valid target</returns>
    protected override bool hitValidTarget(Collider hit)
    {
	    return hit.tag.Equals("NPC");
    }

	/// <summary>
	/// Hits NPC and destroy itself.
	/// </summary>
	/// <param name="hit">NPC that was hit</param>
    protected override void onTriggerHit(Collider hit)
    {
	    onHit(hit.GetComponent<Unit>());
	    Destroy(this.gameObject);
    }
}
