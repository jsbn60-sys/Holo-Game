/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Multiplayer;
using UnityEngine.UI;

///<summary>
/// This class implements the Invisibility item which the player can use to be fully invisible to all enemies.
/// An icon will be displayed at the left side of the screen while the item is active.
/// This class calls a function in the PlayerController script which executes the effects of the item.
///</summary>
public class Invisibility : Item {
    public int duration; // Amount of time this item will last
	public GameObject icon;


	public override void Ability(PlayerController player)
	{
		//Remove Player from NPCs targets, so they dont chase him anymore
		player.CmdInvisibility(true);
		icon = new GameObject();
		player.StartCoroutine(ResetVisibility(player));
		DisplayIcon(icon, new Vector3(30.5f, 281.5f, 0f));
		player.GetComponent<AudioManager>().PlaySound(player.transform.position, 3);
	}


	private IEnumerator ResetVisibility(PlayerController player)
	{
		//count to duration and add player to NPCs targets again to make them chase him again
        yield return new WaitForSecondsRealtime(duration);
		player.CmdInvisibility(false);

		Destroy(icon);
	}
}

