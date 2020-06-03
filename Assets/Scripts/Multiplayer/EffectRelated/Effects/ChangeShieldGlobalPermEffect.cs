using System.Collections;
using System.Collections.Generic;
using Multiplayer.Lobby;
using UnityEngine;

/// <summary>
/// This class represents an effect that changes the shield of all players.
/// Since effects are executed locally, changing the shield of the localPlayer on all clients will cause a global effect.
/// </summary>
public class ChangeShieldGlobalPermEffect : PermanentEffect
{
	[SerializeField] private float changeShieldAmount;

	protected override void execEffect()
	{
		LobbyManager.Instance.LocalPlayerObject.GetComponent<Player>().CmdGiveShield(changeShieldAmount);
	}
}
