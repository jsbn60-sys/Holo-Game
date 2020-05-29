using System.Collections;
using System.Collections.Generic;
using Multiplayer.Lobby;
using UnityEngine;

/// <summary>
/// This class represents an effect that gives the local player an item.
/// Since effects are executed on all clients locally this will cause every player to receive an item.
/// </summary>
public class GiveRandomItemGlobalEffect : PermanentEffect
{
	protected override void execEffect()
	{
		GameObject itemPrefab = ItemController.Instance.getRandomItemPrefab().gameObject;
		int prefabIdx = LobbyManager.Instance.getIdxOfPrefab(itemPrefab);
		LobbyManager.Instance.LocalPlayerObject.GetComponent<Unit>().CmdPlaceObjectOnTop(prefabIdx);
    }
}
