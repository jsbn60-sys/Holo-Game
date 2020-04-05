/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Multiplayer;

///<summary>
/// This class implements all items which instantiate explosions.
/// Throwable detonators explode in distance, non throwables at the player's position.
/// This class only calls a function in the PlayerController script which executes the effects of the item.
///</summary>
public class Detonator : Item{

    public GameObject explosion;
	public bool throwable; // if the explosion should be local or distant

	public override void Ability(PlayerController player)
    {
		player.CmdCreateDetonator(throwable);	
	}
}

