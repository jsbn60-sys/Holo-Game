/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// This class implements the DmgBuff MainSkillBuff of a MNI Player.
/// When this Skill is activated, the Dmg Buff, given by the MainSkill, will be increased
/// Simply changes the boostDmg value of the player to the "newBoostValue"
///</summary>
public class MNI_MainSkill_DmgBuff : Skill
{
	public float newBoostValue = 1.4f;
	public override void Activate(PlayerController player)
	{
		player.boostDmg = newBoostValue;
	}
}
