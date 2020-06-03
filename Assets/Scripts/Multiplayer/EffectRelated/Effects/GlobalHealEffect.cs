using System.Collections;
using System.Collections.Generic;
using Multiplayer.Lobby;
using UnityEngine;

/// <summary>
/// This class represents an effect that heals all players in the game.
/// Since effects are executed locally, changing the health of the localPlayer on all clients will cause a global effect.
///  </summary>
public class GlobalHealEffect : PermanentEffect
{
	[SerializeField] private float changeHealthAmount;
	protected override void execEffect()
    {
	    LobbyManager.Instance.LocalPlayerObject.GetComponent<Player>().CmdChangeHealth(changeHealthAmount);

    }
}
