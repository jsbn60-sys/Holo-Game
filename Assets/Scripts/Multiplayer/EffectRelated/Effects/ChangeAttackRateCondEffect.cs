using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect that changes the attackRate of a unit
/// as long as it has a shield.
/// </summary>
public class ChangeAttackRateCondEffect : ConditionEffect
{
	[SerializeField] private float attackRateChangeFactor;
	protected override bool isActive()
	{
		return target.Shield > 0;
	}
	protected override void execEffect()
	{
		target.changeAttackRate(attackRateChangeFactor);
	}

    protected override void turnOffEffect()
    {
	    target.changeAttackRate(1/attackRateChangeFactor);
    }
}
