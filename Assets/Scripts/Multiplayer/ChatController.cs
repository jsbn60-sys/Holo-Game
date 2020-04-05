/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
namespace Multiplayer.Lobby
{
	/// <summary>
	/// This class holds and manages a live chat system.
	/// It is attached to the "Chat" GameObject in the Lobby Scene.
	/// </summary>
	[RequireComponent(typeof(LobbyManager))]
	public class ChatController : NetworkBehaviour
	{
		const short chatMsg = 1000;
		NetworkClient _client;

		public int maxMessages = 40;
		public string playerName;
		public GameObject chatPanel, textObject;
		public InputField chatBox;

		[SerializeField]
		private Text chatline;
		[SerializeField]
		private SyncListString chatLog = new SyncListString();

		[SerializeField]
		private LobbyManager lobbyManager;
		public override void OnStartClient()
		{
			chatLog.Callback = OnChatUpdated;
		}

		public void Start()
		{
			UpdateChatControllerObject();
		}
		/// <summary>
		/// Client method for posting messages.
		/// </summary>
		/// <param name="message">The message that is sent</param>
		[Client]
		public void PostChatMessage(string message)
		{
			if(_client == null)
			{
				UpdateChatControllerObject();
			}
			if (message.Length == 0)return;
			var msg = new StringMessage(message);
			_client.Send(chatMsg, msg);

			chatBox.text = "";
			chatBox.DeactivateInputField();
		}
		/// <summary>
		/// Server method for posting messages.
		/// </summary>
		/// <param name="netMsg">The message that is posted</param>
		[Server]
		void OnServerPostChatMessage(NetworkMessage netMsg)
		{
			string message = netMsg.ReadMessage<StringMessage>().value;
			chatLog.Add(message);
		}
		private void OnChatUpdated(SyncListString.Operation op, int index)
		{
			chatline.text += "\n" + chatLog[chatLog.Count - 1];
		}

		/// <summary>
		/// Called in the update-loop of LobbyPlayer (For Chat selection in Lobby) and in the update-loop of PlayerController (For Chat selection in Game)
		/// </summary>
		public void HandleChatSelection()
		{
			if (!chatBox.text.Equals(""))
			{
				if (Input.GetKeyDown(KeyCode.Return))
				{
					PostChatMessage(playerName + ": " + chatBox.text);
					chatBox.DeactivateInputField();
				}
			}
			else
			{
				if (!chatBox.isFocused && Input.GetKeyDown(KeyCode.Return))
				{
					chatBox.ActivateInputField();
				}
			}
		}

		/// <summary>
		/// Initializes the chatController Object and registers a handler for posting messages
		/// </summary>
		private void UpdateChatControllerObject()
		{
			playerName = LobbyManager.LocalPlayerName;
			_client = NetworkManager.singleton.client;
			NetworkServer.RegisterHandler(chatMsg, OnServerPostChatMessage);
		}
	}
}
