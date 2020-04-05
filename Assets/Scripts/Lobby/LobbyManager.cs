/* edited by: SWT-P_SS_2019_Holo */
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Multiplayer.Lobby
{
	public class LobbyManager : NetworkLobbyManager
	{
		public delegate void DisconnectButtonDelegate();
		public DisconnectButtonDelegate disconncetDelegate;
		public GameObject[] players;

		public static LobbyManager Instance;

		public static string LocalPlayerName = "";

		public Text statusText;

		[SerializeField]
		private RectTransform connectMenu;
		[SerializeField]
		private RectTransform lobbyMenu;
		[SerializeField]
		private RectTransform offlineMenu;
		//[SerializeField]
		//private RectTransform chat;

		private void Start()
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		/// <summary>
		/// This is called on the client when the client is finished loading a new networked scene.
		/// </summary>
		/// <param name="conn">The connection that finished loading a new networked scene.</param>
		public override void OnLobbyClientSceneChanged(NetworkConnection conn)
		{
			if (SceneManager.GetSceneAt(0).name == lobbyScene)
			{
				lobbyMenu.gameObject.SetActive(false);
				connectMenu.gameObject.SetActive(true);
			}
			else
			{
				lobbyMenu.gameObject.SetActive(false);
				connectMenu.gameObject.SetActive(false);
			}
		}


		/// <summary>
		/// This hook is invoked when a host is started.\n
		/// StartHost has multiple signatures, but they all cause this hook to be called.
		/// </summary>
		public override void OnStartHost()
		{
			 base.OnStartHost();
			disconncetDelegate = StopHostClbk;
			SetStatusInfo("Hosting");

			connectMenu.gameObject.SetActive(false);
			//chat.gameObject.SetActive(true);
		}

		/// <summary>
		/// This is called on the server when a client disconnects.
		/// </summary>
		/// <param name="conn">	The connection that disconnected.</param>
		public override void OnLobbyServerDisconnect(NetworkConnection conn)
		{
			connectMenu.gameObject.SetActive(false);
			//chat.gameObject.SetActive(false);
		}

		public override void OnClientConnect(NetworkConnection conn)
		{
			base.OnClientConnect(conn);
			//hide main menu
			connectMenu.gameObject.SetActive(false);
			//chat.gameObject.SetActive(true);

			if (NetworkServer.active) return;

			//only to do on pure client (not self hosting client)
			disconncetDelegate = StopClientClbk;
			SetStatusInfo("Client");
		}

		/// <summary>
		/// Create Players
		/// </summary>
		/// <param name="conn">The connection the player object is for.</param>
		/// <param name="playerControllerId">The controllerId of the player.</param>
		/// <returns></returns>
		public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
		{
			GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject);

			return obj;
		}

		public void Spawnplayer(int x, GameObject lp, GameObject gp)
		{
			LobbyPlayer lobbyPlayer = lp.GetComponent<LobbyPlayer>();
			PlayerController playerController = gp.GetComponent<PlayerController>();

			NetworkServer.Destroy(gp);
			GameObject p4 = Instantiate(players[x], new Vector3(-51, 0, 37), Quaternion.identity);
			p4.name = lobbyPlayer.playerName;
			p4.GetComponent<PlayerController>().playerName = lobbyPlayer.playerName;
			NetworkServer.Spawn(p4);
			NetworkServer.ReplacePlayerForConnection(lobbyPlayer.connectionToClient, p4, lobbyPlayer.playerControllerId);
		}

		/// <summary>
		/// This is called on the server when it is told that a client has finished switching from the lobby scene to a game player scene.
		///	Apply config from the lobby-player to the game-player object.
		/// </summary>
		/// <param name="lp">The lobby player object.</param>
		/// <param name="gp">The game player object.</param>
		/// <returns>False to not allow this player to replace the lobby player.</returns>
		public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lp, GameObject gp)
		{
			Cursor.lockState = CursorLockMode.Locked;
			LobbyPlayer lobbyPlayer = lp.GetComponent<LobbyPlayer>();
			PlayerController playerController = gp.GetComponent<PlayerController>();
			switch (lobbyPlayer.playerRole)
			{
				case PlayerRole.MNI:
					playerController.role = 0;
					gp.GetComponent<PlayerController>().playerName = lobbyPlayer.playerName;
					Spawnplayer(0, lp, gp);
					return false;
				case PlayerRole.LSE:
					playerController.role = 1;
					Spawnplayer(1, lp, gp);
					return false;
				case PlayerRole.Wirtschaft:
					playerController.role = 2;
					Spawnplayer(2, lp, gp);
					return false;
				case PlayerRole.Gesundheit:
					playerController.role = 3;
					Spawnplayer(3, lp, gp);
					return false;
				case PlayerRole.Drone:
					NetworkServer.Destroy(gp);
					
					//set perspective of drone
					players[(int)PlayerRole.Drone].GetComponent<Drone>().usesHoloboard = lobbyPlayer.droneUsesHoloboardPerspective;
					
					//spawn drone prefab and replace with prof
					GameObject drone = Instantiate(players[(int)PlayerRole.Drone], Vector3.up * 30, Quaternion.identity);
					
					NetworkServer.Spawn(drone);
					NetworkServer.ReplacePlayerForConnection(lobbyPlayer.connectionToClient, drone, lobbyPlayer.playerControllerId);

					//rename spawned drone object to players name
					drone.name = lobbyPlayer.playerName;

					//must return false, otherwise unity still tries to spawn prof, I don't know?
					return false;
			}
			//playerController.name = lobbyPlayer.playerName;
			return true;
		}

		/// <summary>
		/// method called on Server when a client Disconects
		/// Destroys the players on the Scenes and changes the Player count 
		/// </summary>
		/// <param name="conn"></param>
		public override void OnServerDisconnect(NetworkConnection conn)
		{
			Debug.Log("Client disconnected: " + conn.lastError);

			// Removes the player game object from the world.
			NetworkServer.DestroyPlayersForConnection(conn);
			WaveCreator.Instance.ReducePlayerCount();


			// Changes the remaining Players Status to not ready and frees all classes to be Picked
			foreach (var nwPlayer in lobbySlots)
			{
				var lobbyPlayer = nwPlayer as LobbyPlayer;
				if (lobbyPlayer != null)
				{
					lobbyPlayer.ChangeButtonStatusAndCheckDroneStatus();
				}
			}

			// Forces the server to shutdown. 
			/*
			Shutdown();

			// Reset internal state of the server and start the server again.
			Start();
			*/

		}
		/// <summary>
		/// return to MainMenu After GameOver and reset Network/Server
		/// </summary>
		public void ReturnToLobby(bool isServer)
		{
			
			if (isServer) // Player is Server
			{
				Debug.Log("is Server");
				ServerReturnToLobby();
				NetworkManager.singleton.StopHost();
				NetworkServer.Reset();
				Instance.StopServer();
				Instance.Start();



				StopHostClbk();
				WaveCreator.Instance.SetPlayerCountToZero();

			}
			else // Player is Client
			{
				WaveCreator.Instance.SetPlayerCountToZero();
				StopClientClbk();
			}

			connectMenu.gameObject.SetActive(true);

			offlineMenu.gameObject.SetActive(false);
			lobbyMenu.gameObject.SetActive(true);

			Time.timeScale = 1.0f;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		public void StopHostClbk()
		{
			StopHost();
			connectMenu.gameObject.SetActive(true);
		}

		public void StopClientClbk()
		{
			StopClient();
			connectMenu.gameObject.SetActive(true);
		}

		public void SetStatusInfo(string status)
		{
			statusText.text = status;
		}

		/// <summary>
		/// Called on clients when the Server disconects / Shutdowns
		/// Rebuilds the main Scene and Stops the Client
		/// </summary>
		/// <param name="conn"></param>
		public override void OnClientDisconnect(NetworkConnection conn)
		{
			WaveCreator.Instance.SetPlayerCountToZero();

			offlineMenu.gameObject.SetActive(true);
			lobbyMenu.gameObject.SetActive(false);
			connectMenu.gameObject.SetActive(false);
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			
			StopClient();

		}

		public void OnClose()
		{		
			Application.Quit();
		}
}
}