using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect which increases the units damage for a certain duration.
/// </summary>
public class DamageBoostEffect : DurationEffect
{
	[SerializeField]
	private float dmgBoostAmount;

	protected override void execEffect()
	{
		target.getAttack().changeDmg(true, dmgBoostAmount);
	}

	protected override void turnOffEffect()
	{
		target.getAttack().changeDmg(false, dmgBoostAmount);
	}
}
