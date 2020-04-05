/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// This class implements the Dot And Bullet Speed MainSkillBuff of a MNI Player.
/// With this Skill active, the main Skill's Damage Over Time (DoT) damage, implemented by the MNI_Dot Skill, will be increased
/// and during the activation of the Main Skill, Bullet will also Travel faster
/// Sets the  dot Multiplier of the player to "newDotMultiplier" and increases the BulletSpeed by "speedMultiplier"
///</summary>
public class MNI_DotAndBulletSpeed : Skill
{
	public int newDotMultiplier = 2;
	public int speedMultiplier = 2;
	public override void Activate(PlayerController player)
	{
		player.dotMultiplier = newDotMultiplier;
		player.bulletSpeed = player.bulletSpeed * speedMultiplier;
	}
}
