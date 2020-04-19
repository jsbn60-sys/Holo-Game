/* author: SWT-P_SS_2019_Holo */
using Multiplayer.Lobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is attached to the quitToMenu button of the menu to return to the lobby.
/// </summary>
public class QuitToMenuScript : MonoBehaviour
{
	public Player localPlayer;
	// retuns a Player to the MainMenu
	public void ReturnToLobby()
	{
		LobbyManager.Instance.ReturnToLobby(true);
		LobbyManager.Instance.StopClientClbk();
	}

	public void ReturnToLobbyIngame()
	{
		Debug.Log("Player is Server?: " + localPlayer.isServer + " PlayerCount: " + WaveCreator.Instance.ReturnPlayerCount());

		if (localPlayer.isServer) // if Player is Server check if Player is the only Player
		{
			Debug.Log(WaveCreator.Instance.ReturnPlayerCount());
			if (WaveCreator.Instance.ReturnPlayerCount() < 2)
			{
				// player is Server but the only Player, so return to Lobby
				LobbyManager.Instance.ReturnToLobby(localPlayer.isServer);
			}

		}
		else
		{
			// player is client, return to Lobby
			LobbyManager.Instance.ReturnToLobby(localPlayer.isServer);
		}
	}

}
