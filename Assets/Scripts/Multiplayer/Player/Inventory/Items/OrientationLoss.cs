/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;
using UnityEngine.UI;

///<summary>
/// This class implements the OrientationLoss item which the player can use to make all enemies lose orientation and start dancing around each other.
/// An icon will be displayed at the left side of the screen while the item is active.
/// This class calls a function in the PlayerController script which executes the effects of the item.
///</summary>
public class OrientationLoss : Item {

	public int duration; // Amount of time this item will last
	public GameObject icon;
	
	public override void Ability(PlayerController player) {
		player.CmdParty(true);
		player.StartCoroutine(resetVisibility(player));
		player.GetComponent<Party>().CmdParty();
		icon = new GameObject();
		DisplayIcon(icon, new Vector3(30.5f, 181.5f, 0f));
	}

	private IEnumerator resetVisibility(PlayerController player)
    {
        yield return new WaitForSecondsRealtime(duration);
		player.CmdParty(false);
		Destroy(icon);
	}


}
