/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

///<summary>
/// This class implements the GlobalStun item which the player can use to freeze all enemies in their current position.
/// An icon will be displayed at the left side of the screen while the item is active.
/// This class calls a function in the PlayerController script which executes the effects of the item.
///</summary>
public class GlobalStun : Item {

	public int duration; //Amount of time this item will last
	public GameObject icon;
	
	public override void Ability(PlayerController player) {

		player.CmdCreateGlobalStun(true);

		player.GetComponent<AudioManager>().PlaySound(player.transform.position,2);
		
		
		player.StartCoroutine(UnStun(player));

		icon = new GameObject();
		DisplayIcon(icon, new Vector3(30.5f, 331.5f, 0f));
		player.StartCoroutine(UnStun(player));
	}

	private IEnumerator UnStun(PlayerController player)
    {
        yield return new WaitForSecondsRealtime(duration);
		player.CmdCreateGlobalStun(false);
		Destroy(icon);
	}
}
