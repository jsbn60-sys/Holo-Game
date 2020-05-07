using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect that changes the attackRate of a unit.
/// </summary>
public class ChangeAttackRateTempEffect : DurationEffect
{
	[SerializeField] private float attackRateChangeFactor;

	protected override void execEffect()
    {
		target.changeAttackRate(attackRateChangeFactor);
    }

	protected override void turnOffEffect()
	{
		target.changeAttackRate(1/attackRateChangeFactor);
	}
}
