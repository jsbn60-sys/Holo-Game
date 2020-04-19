/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is attached to the exit button of the menu to exit the game.
/// </summary>
public class ExitScript : MonoBehaviour {
	
	// Use this for initialization
	public void ExitGame()
	{
		Application.Quit();
	}
}
