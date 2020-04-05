/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class implements the buffed base attack skill of the tank/LSE.
/// It uses the Health class to increase the amount of health the player has.
/// It also sets the buffedAttack boolean of the PlayerController to true so the base attack of the player changes.
///	healthIncrease is the amount of health the player gets when activating this skill.
/// </summary>
public class LSE_BuffedBaseAttack : Skill
{
	public int healthIncrease = 30;

	public override void Activate(PlayerController player)
	{
		player.CmdBuffTank(healthIncrease);
	}
}
