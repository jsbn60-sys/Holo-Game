using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect which is instant, meaning it has no duration.
/// A good example for this is the HealEffect, which heals once and is done after that.
/// </summary>
public abstract class SingleUseEffect : Effect
{
	/// <summary>
	/// Sets target and executes effect once.
	/// </summary>
	/// <param name="target">Unit to apply effect to</param>
	public override void turnOnEffect(Unit target)
	{
		this.target = target;
		execEffect();
	}
}
