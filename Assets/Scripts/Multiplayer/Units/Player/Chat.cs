using System;
using System.Collections;
using System.Collections.Generic;
using Multiplayer.Lobby;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This class handles interaction with the chat.
/// </summary>
public class Chat : MonoBehaviour
{
	[SerializeField] private InputField chatInput;
	[SerializeField] private Text chatContent;

	private bool chatIsSelected;

	public bool ChatIsSelected => chatIsSelected;

	/// <summary>
	/// Posts a message in the chat.
	/// </summary>
	/// <param name="sender">Player who send the message</param>
	/// <param name="message">Message the player send</param>
	public void postMessage(string sender, string message)
	{
		chatContent.text = chatContent.text + "\n" + sender + ":" + message;
	}

	/// <summary>
	/// Toggles the chat.
	/// </summary>
	public void toggleInput()
	{
		if (chatIsSelected)
		{
			chatIsSelected = false;
			EventSystem.current.SetSelectedGameObject(null); // deselect workaround
		}
		else
		{
			chatIsSelected = true;
			chatInput.Select();
		}
	}

	/// <summary>
	/// Sends a text to all other players.
	/// Is called by the InputField on Enter or Escape,
	/// but only will send on Enter.
	/// </summary>
	/// <param name="message">Current content of the input field</param>
	public void sendText(string message)
	{
		if (!Input.GetKeyDown(KeyCode.Escape))
		{
			Player localPlayer = LobbyManager.Instance.LocalPlayerObject.GetComponent<Player>();
			localPlayer.CmdSendTextMessage(localPlayer.name, message);
			chatInput.text = "";
		}
	}
}
