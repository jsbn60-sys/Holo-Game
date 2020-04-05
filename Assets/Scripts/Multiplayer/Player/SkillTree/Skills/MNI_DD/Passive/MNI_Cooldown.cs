/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// This class implements the Cooldown Passive Skill of a MNI Player.
/// With this Skill active, all invested points in the right side of the skill
/// tree will yield a cooldown reduction for all active Skills
/// Simply sets the coolDownSkill var of player to true, Rest is implemented in affected skills
///</summary>
public class MNI_Cooldown : Skill
{
	public override void Activate(PlayerController player)
	{
		player.coolDownSkill = true;
	}
}
