/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Multiplayer;
using UnityEngine.Networking;
using NPC;

///<summary>
/// This class implements the OneHit item which the player can use to remove an enemy from the game by simply clicking on it.
/// This class only calls a function in the PlayerController script which executes the effects of the item
///</summary>
public class OneHit : Item {
	public override void Ability(PlayerController player) {
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
}


