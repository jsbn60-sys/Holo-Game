using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// This class represents an effect which increases the units damage for a certain duration.
/// </summary>
public class ChangeDamageTempEffect : DurationEffect
{

	[SerializeField] private float dmgChangeAmount;

	protected override void execEffect()
	{
		target.getAttack().changeDmg(dmgChangeAmount);
	}

	protected override void turnOffEffect()
	{
		target.getAttack().changeDmg(-dmgChangeAmount);
	}
}
