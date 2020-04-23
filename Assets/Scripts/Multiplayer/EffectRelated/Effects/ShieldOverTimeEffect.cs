using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect which gives the unit a certain amount of shield over a duration in ticks.
/// </summary>
public class ShieldOverTimeEffect : TickingEffect
{
	[SerializeField]
	private float shieldAmount;

	protected override void execEffect()
	{
		target.giveShield(shieldAmount / tickAmount);
	}

}
