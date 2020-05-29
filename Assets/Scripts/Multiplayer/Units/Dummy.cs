using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple dummy unit, that will be targeted by npcs in range.
/// </summary>
public class Dummy : Unit
{
	/// <summary>
	/// Destroys itself on death.
	/// </summary>
	protected override void onDeath()
    {
	    Destroy(gameObject);
    }

	/// <summary>
	/// Dummies can't push anyone.
	/// </summary>
	/// <param name="target">Target that collided</param>
	/// <returns>Always false</returns>
	protected override bool canPushTarget(Unit target)
	{
		return false;
	}
}
