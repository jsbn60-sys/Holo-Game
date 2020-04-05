/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class implements a modification of the Healer main ability(Healing Shots).
/// The cooldown gets removed, the ability is no longer a buff.
/// The player can toggle the ability on/off whenever he wants.
/// It also reduces the attack speed of the healer for balancing.
/// </summary>
public class Ges_PermaHealingMode : Skill
{
	public float atkSpdMultiplier = 1.2f; //1*1.2 = 1.2 -> 20% less atk spd
	public override void Activate(PlayerController player)
	{
		player.permaHealingMode = true;
		player.fireRate = player.fireRate * atkSpdMultiplier;
	}
}
