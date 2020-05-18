using System.Collections;
using System.Collections.Generic;
using Multiplayer.Lobby;
using UnityEngine;

public class PlaceObjectEffect : PermanentEffect
{
	[SerializeField] private GameObject objectToPlace;
	[SerializeField] private bool placeInfrontOfPlayer;
	protected override void execEffect()
    {
	    if (placeInfrontOfPlayer)
	    {
		    target.GetComponent<Player>().CmdPlaceObjectInfront(LobbyManager.Instance.getIdxOfPrefab(objectToPlace));
	    }
	    else
	    {
		    target.GetComponent<Player>().CmdPlaceObjectOnTop(LobbyManager.Instance.getIdxOfPrefab(objectToPlace));
	    }
    }
}
