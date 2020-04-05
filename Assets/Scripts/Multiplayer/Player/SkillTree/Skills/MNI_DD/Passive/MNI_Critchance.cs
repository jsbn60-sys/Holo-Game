/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// This class implements the Critchance Passive Skill of a MNI Player.
/// With this Skill activated, the player's attacks have a chance to do
/// increased Damage.
/// Triggers the canCrit var of player and sets the critchance
///</summary>
public class MNI_Critchance : Skill
{
	public int critChance = 10; 
	public override void Activate(PlayerController player)
	{
		player.critchance = critChance;
		player.canCrit = true;
	}
}
