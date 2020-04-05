/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class implements the shield cooldown skill of the tank/LSE.
/// It reduces the cooldown of the main skill by <cooldownReduction>
/// </summary>
public class LSE_ShieldCD : Skill
{
	public int cooldownReduction = 10;

	public override void Activate(PlayerController player)
	{
		Skill mainSkill = player.skills[2];
		mainSkill.cooldown -= cooldownReduction;
	}
}
