using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect that changes the targets onTouchDmg for a certain duration.
/// </summary>
public class ChangeOnTouchDmgTempEffect : DurationEffect
{
	[SerializeField] private float onTouchDmgChangeAmount;
	protected override void execEffect()
	{
		target.changeOnTouchDmg(onTouchDmgChangeAmount);
	}

	protected override void turnOffEffect()
    {
	    target.changeOnTouchDmg(-onTouchDmgChangeAmount);
    }
}
