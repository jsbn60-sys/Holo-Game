using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect that changes the speed at which players throw their attacks.
/// IMPORTANT: THIS EFFECT CAN ONLY BE APPLIED TO PLAYERS.
/// </summary>
public class ChangeThrowSpeedTempEffect : DurationEffect
{
	[SerializeField] private float throwSpeedFactor;
	protected override void execEffect()
	{
		target.GetComponent<Player>().changeThrowSpeed(throwSpeedFactor);
	}

	protected override void turnOffEffect()
	{
		target.GetComponent<Player>().changeThrowSpeed(1/throwSpeedFactor);
	}
}
