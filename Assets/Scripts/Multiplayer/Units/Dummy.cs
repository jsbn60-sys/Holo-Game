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
}
