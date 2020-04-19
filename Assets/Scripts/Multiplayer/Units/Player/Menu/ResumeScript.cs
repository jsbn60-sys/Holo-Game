/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// This script is attached to the resume button of the menu to continue playing.
/// </summary>
public class ResumeScript : MonoBehaviour {

	//public GameObject menu;

	public void Resume()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject go in players)
		{
			go.GetComponent<Player>().CmdCloseMenu();
		//	menu.SetActive(false);
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}
}
