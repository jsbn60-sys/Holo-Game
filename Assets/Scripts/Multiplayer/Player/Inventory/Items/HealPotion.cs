/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Multiplayer;

///<summary>
/// This class implements the HealPotion item which the player can use to fully heal himself instantly
/// This class only calls a function in the PlayerController script which executes the effects of the item
///</summary>
public class HealPotion : Item
{
	public override void Ability(PlayerController player)
	{
		player.GetComponent<Health>().CmdhealFull();
		player.GetComponent<AudioManager>().PlaySound(player.transform.position,4);
	}
}
