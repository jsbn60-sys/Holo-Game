using System.Collections;
using System.Collections.Generic;
using Multiplayer.Lobby;
using UnityEngine;

public class ThrowRandomItemEffect : PermanentEffect
{
	protected override void execEffect()
	{
		target.GetComponent<Drone>().CmdDrop(
			LobbyManager.Instance.getIdxOfPrefab(
				ItemController.Instance.getRandomItemPrefab().gameObject));
	}
}
