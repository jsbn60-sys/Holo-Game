/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;
using UnityEngine.Networking;

///<summary>
/// This class implements the Teleporter item which the player can use to teleport to a random out of 4 predefined positions on the map
/// An effect will be spawned to display the item is active 
/// This class calls a function in the PlayerController script which executes the effects of the item
///</summary>
public class Teleport : Item
{
	public override void Ability(PlayerController player)
	{
		player.CmdTeleportEffectStart();
		Random rand = new Random();
		// random Number from 0 to 3. This numerb used for select random position
		int randomPosition = rand.Next(0, 3);
		//player looks to the center of the card
		var rot = transform.rotation;


		switch (randomPosition)
		{
			case 0:
				player.transform.position = new Vector3(72.82869f, 0f, 4.383396f);
				rot = transform.rotation;
				rot.y = 152.783f;
				transform.rotation = rot;
				break;
			case 1:
				player.transform.position = new Vector3(-14.53801f, 0f, 70.94662f);
				rot = transform.rotation;
				rot.y = -82.953f;
				transform.rotation = rot;
				break;
			case 2:
				player.transform.position = new Vector3(-105.7161f, 0f, 43.11845f);
				rot = transform.rotation;
				rot.y = -121.785f;
				transform.rotation = rot;
				break;
			case 3:
				player.transform.position = new Vector3(-101.779f, 0f, -30.32481f);
				rot = transform.rotation;
				rot.y = 132.974f;
				transform.rotation = rot;
				break;
				
		}
		player.CmdTeleportEffectEnd(player.transform.position);
	}

	

	

}


