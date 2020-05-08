using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Effect which gives the target a speed boost for a duration.
/// </summary>
public class ChangeSpeedTempEffect : DurationEffect
{
	[SerializeField] private float speedBoostFactor;

	protected override void execEffect()
	{
		target.changeSpeed(true, speedBoostFactor);
	}

	protected override void turnOffEffect()
	{
		target.changeSpeed(false, speedBoostFactor);
	}
}
