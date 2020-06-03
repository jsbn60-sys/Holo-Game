/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using Multiplayer.Lobby;
using UnityEngine;



/// <summary>
/// This script is attached to the resume button of the menu to continue playing.
/// </summary>
public class ResumeScript : MonoBehaviour {

	//public GameObject menu;

	public void Resume()
	{
		LobbyManager.Instance.LocalPlayerObject.GetComponent<Player>().setForGameplay();
	}
}
