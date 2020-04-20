using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect which heals the unit.
/// </summary>
public class HealEffect : SingleUseEffect
{
	[SerializeField]
	private float healAmount;

	protected override void execEffect()
	{
		target.heal(healAmount);
	}
}
