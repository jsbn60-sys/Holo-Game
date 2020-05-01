using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect which is instant, meaning it has no duration.
/// A good example for this is the HealthChangeEffect, which for example heals once and is done after that.
/// Passive effects are also implemented through this class, since they are permanent.
///
/// The abstract method execEffect is in this case to be understood as simply using the effect.
/// </summary>
public abstract class PermanentEffect : Effect
{
	public void Start()
	{
		execEffect();
		Destroy(gameObject);
	}
}
