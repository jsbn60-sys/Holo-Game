	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect which gives the unit a certain amount of shield over a duration in ticks.
/// </summary>
public class ChangeShieldTickEffect : TickingEffect
{
	[SerializeField]
	private float shieldAmount;

	protected override void execEffect()
	{
		target.changeShield(shieldAmount / tickAmount);
	}

}
