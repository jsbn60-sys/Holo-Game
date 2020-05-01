using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect which gives the player a certain amount of shield.
/// </summary>
public class ChangeShieldPermEffect : PermanentEffect
{
	[SerializeField]
	private float shieldAmount;

	protected override void execEffect()
	{
		target.giveShield(shieldAmount);
	}
}
