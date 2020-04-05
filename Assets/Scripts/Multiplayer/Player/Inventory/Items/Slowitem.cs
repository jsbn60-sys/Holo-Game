/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Multiplayer;

///<summary>
/// This class implements the Slow item which the player can use to throw a slowbullet.
/// The slowbullet will spawn a slowfield on the ground which reduces the speed of the enemies walking on it
/// An icon will be displayed at the left side of the screen while the item is active.
/// This class calls a function in the PlayerController script which executes the effects of the item.
///</summary>
public class Slowitem : Item{

public GameObject slowbullet;
public override void Ability(PlayerController player)
    {
		    player.CmdCreateSlowBullet();
    }
}
