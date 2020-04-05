/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Multiplayer;

///<summary>
/// This class implements the HealAll item which the player can use to heal all players in the game
/// This class only calls a function in the PlayerController script which executes the effects of the item
///</summary>
public class HealAll : Item {
	public int heal; //Amount of healthpoints added to each players health

	public override void Ability(PlayerController player) {
		player.CmdHealAll(heal);
	}




}


