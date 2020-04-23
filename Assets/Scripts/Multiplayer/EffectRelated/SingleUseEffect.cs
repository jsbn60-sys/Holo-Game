using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect which is instant, meaning it has no duration.
/// A good example for this is the HealEffect, which heals once and is done after that.
///
/// The abstract method execEffect is in this case to be understood as simply using the effect.
/// </summary>
public abstract class SingleUseEffect : Effect
{
	public void Start()
	{
		execEffect();
		Destroy(gameObject);
	}
}
