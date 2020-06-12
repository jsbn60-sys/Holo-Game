using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class represents an interface for controlling the deathScreen of the player
/// and updating UI elements.
/// </summary>
public class DeathScreen : MonoBehaviour
{
	[SerializeField] private Text playerToWatchText;

	/// <summary>
	/// Updates the playerToWatch UI text.
	/// </summary>
	/// <param name="playerName">Player that is watched</param>
	public void updatePlayerToWatchText(string playerName)
	{
		playerToWatchText.text = playerName;
	}
}
