using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect that changes the targets pushForce for a given duration.
/// </summary>
public class ChangePushForceTempEffect : DurationEffect
{
	[SerializeField] private float pushForceChangeAmount;

	protected override void execEffect()
	{
		target.changePushForce(pushForceChangeAmount);
	}

    protected override void turnOffEffect()
    {
	    target.changePushForce(-pushForceChangeAmount);
    }
}
