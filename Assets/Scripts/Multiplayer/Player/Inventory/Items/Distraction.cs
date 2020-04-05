/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Multiplayer;
using UnityEngine.Networking;
using NPC;
using UnityEngine.UI;

///<summary>
/// This class implements the Distraction item which the player can use to spawn a tutor which will help distracting the enemy students.
/// An icon will be displayed at the left side of the screen while the item is active 
/// This class calls a function in the PlayerController script which executes the effects of the item
///</summary>
public class Distraction : Item {
	public int duration; //Amount of time this item will last
	public GameObject icon;
	
	public override void Ability(PlayerController player) 
	{
		player.CmdDummy(true);
		icon = new GameObject();
		DisplayIcon(icon, new Vector3(30.5f, 381.5f, 0f));
		player.StartCoroutine(DestroyDummy(player));
	}
	
	private IEnumerator DestroyDummy(PlayerController player)
    {
        yield return new WaitForSecondsRealtime(duration);
		player.CmdDummy(false);
		Destroy(icon);
	}
}
