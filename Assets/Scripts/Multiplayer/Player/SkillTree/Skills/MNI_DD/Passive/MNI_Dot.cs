/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// This class implements the Dot MainSkillBuff of a MNI Player.
/// With this skill activated, the Main Skill will also apply Damage Over Time (DoT) Effects
/// and increase the players fire Rate
/// on hit Enemys.
/// Simply sets the dotEffectActive var to true and increases the players fire Rate var
///</summary>
public class MNI_Dot : Skill
{

	public override void Activate(PlayerController player)
	{
		player.fireRate -= 0.15f;
		player.dotEffectActive = true;
	}
}
