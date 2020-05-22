using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect that changes the amount of enemies a projectile can pierce for a certain duration.
/// IMPORTANT: THIS REQUIRES THAT THE TARGET HAS A SINGLETARGETPROJECTILE AS AN ATTACK.
/// </summary>
public class ChangePierceAmountTempEffect : DurationEffect
{
	[SerializeField] private int pierceChangeAmount;
	protected override void execEffect()
	{
		target.getAttack().GetComponent<SingleTargetProjectile>().changePierceAmount(pierceChangeAmount);
	}

	protected override void turnOffEffect()
	{
		target.getAttack().GetComponent<SingleTargetProjectile>().changePierceAmount(-pierceChangeAmount);
	}
}
