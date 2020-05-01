using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect that changes the attackRate of a unit.
/// </summary>
public class ChangeAttackRateTempEffect : DurationEffect
{
	[SerializeField] private float attackRateChangeAmount;

	protected override void execEffect()
    {
		target.changeAttackRate(attackRateChangeAmount);
    }

	protected override void turnOffEffect()
	{
		target.changeAttackRate(-attackRateChangeAmount);
	}
}
