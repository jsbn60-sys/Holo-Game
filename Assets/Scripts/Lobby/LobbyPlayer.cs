/* edited by: SWT-P_WS_2019_Holo */
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Multiplayer.Lobby
{
	public enum PlayerRole
	{
		MNI,
		Wirtschaft,
		Gesundheit,
		LSE,
		Drone
	}

	public class LobbyPlayer : NetworkLobbyPlayer
	{
		public Color readColor;
		public Color notReadyColor;
		[SyncVar(hook = "OnPlayerNameChanged")]
		public string playerName = "";
		[SyncVar(hook = "OnPlayerRoleChanged")]
		public PlayerRole playerRole = PlayerRole.MNI;
		public Button readyButton;
		public Button roleButton;
		[SyncVar(hook = "OnHoloboardPerspectiveToggled")]
		public bool droneUsesHoloboardPerspective = false;

		//Class taken vars
		[SyncVar]
		private bool isDroneTaken = false;
		[SyncVar]
		private bool isMNITaken = false;
		[SyncVar]
		private bool isLSETaken = false;
		[SyncVar]
		private bool isGESTaken = false;
		[SyncVar]
		private bool isWITaken = false;


		public Toggle droneHoloboardToggle;
		public Text nameText;
		public bool ready = false;

		GameObject chatObject;

		private void Start()
		{
			chatObject = GameObject.Find("Chat");
			ChangeButtonStatusAndCheckDroneStatus();
		}

		private void Update()
		{
			chatObject.GetComponent<Multiplayer.Lobby.ChatController>().HandleChatSelection();
		}

		public override void OnClientEnterLobby()
		{
			base.OnClientEnterLobby();
			//Add player to the lobby list
			LobbyPlayerList.Instance.AddPlayer(this);
			//if player is local
			if (isLocalPlayer)
			{
				//setup local player
				SetupLocalPlayer();
			}
			else// if player is remote
			{
				//set up remote player
				SetupOtherPlayer();
			}
		}

		/// <summary>
		/// Called from Lobby Manager and on connect
		/// Removes ready status from other Players and checks if a Player has already Picked Drone
		/// </summary>
		public void ChangeButtonStatusAndCheckDroneStatus()
		{
			isDroneTaken = false;
			isMNITaken = false;
			isLSETaken = false;
			isGESTaken = false;
			isWITaken = false;

			CmdSyncFreePlayerRoles();
		}

		public override void OnStartAuthority()
		{
			base.OnStartAuthority();
			//Setup local Player
			SetupLocalPlayer();
		}

		public override void OnStartLocalPlayer()
		{
			base.OnStartLocalPlayer();
			//if player is local
			if (isLocalPlayer)
			{
				//setup local player
				SetupLocalPlayer();
			}
			else// if player is remote
			{
				//set up remote player
				SetupOtherPlayer();
			}
		}

		/// <summary>
		/// Sync player name
		/// </summary>
		/// <param name="newName"></param>
		private void OnPlayerNameChanged(string newName)
		{
			playerName = newName;
			nameText.text = newName;
		}

		/// <summary>
		/// Sync player role
		/// </summary>
		/// <param name="newRole"></param>
		private void OnPlayerRoleChanged(PlayerRole newRole)
		{
			switch (newRole)
			{
				case PlayerRole.MNI:
					roleButton.GetComponentInChildren<Text>().text = "MNI";
					//when drone player, hide camera perspective option
					ShowHoloboardPerspectiveToggleSwitch(false);
					break;
				case PlayerRole.Wirtschaft:
					roleButton.GetComponentInChildren<Text>().text = "Wirtschaft";
					//when drone player, hide camera perspective option
					ShowHoloboardPerspectiveToggleSwitch(false);
					break;
				case PlayerRole.Gesundheit:
					roleButton.GetComponentInChildren<Text>().text = "Gesundheit";
					//when drone player, hide camera perspective option
					ShowHoloboardPerspectiveToggleSwitch(false);
					break;
				case PlayerRole.LSE:
					roleButton.GetComponentInChildren<Text>().text = "LSE";
					//when drone player, hide camera perspective option
					ShowHoloboardPerspectiveToggleSwitch(false);
					break;
				case PlayerRole.Drone:
					roleButton.GetComponentInChildren<Text>().text = "Drone";
					//when drone player, display camera perspective option
					ShowHoloboardPerspectiveToggleSwitch(true);
					break;
				default:
					throw new InvalidEnumArgumentException("Invalid Role");
			}
			if(isLocalPlayer)
				SendNotReadyToBeginMessage();
			
			playerRole = newRole;
		}

		private void ChangeReadyButtonColor(Color c)
		{
			ColorBlock b = readyButton.colors;
			b.normalColor = c;
			b.pressedColor = c;
			b.highlightedColor = c;
			b.disabledColor = c;
			readyButton.colors = b;
		}

		private void SetupOtherPlayer()
		{
			//Change the color for the ready button
			ChangeReadyButtonColor(notReadyColor);
			readyButton.transform.GetChild(0).GetComponent<Text>().text = "...";
			readyButton.interactable = false;
			roleButton.interactable = false;

			//Sync player name
			OnPlayerNameChanged(playerName);
			//Sync palyer role
			OnPlayerRoleChanged(playerRole);
			//Sync drone perspective
			OnHoloboardPerspectiveToggled(droneUsesHoloboardPerspective);
			//Client isn't ready
			OnClientReady(false);
		}

		/// <summary>
		/// Called when Player clicks on Ready Button
		/// Locks the current Role so that others can't pick it
		/// </summary>
		/// <param name="readyState"> if player is already ready or not </param>
		public override void OnClientReady(bool readyState)
		{
			if (readyState)
			{
				ready = true;
				ChangeReadyButtonColor(readColor);

				Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
				textComponent.text = "READY";
				readyButton.interactable = false;

				switch(playerRole){
					case(PlayerRole.MNI):
						isMNITaken = true;
						break;
					case (PlayerRole.LSE):
						isLSETaken = true;
						break;
					case (PlayerRole.Gesundheit):
						isGESTaken = true;
						break;
					case (PlayerRole.Wirtschaft):
						isWITaken = true;
						break;
					case PlayerRole.Drone:
						isDroneTaken = true;
						break;
				}

				CmdSyncFreePlayerRoles();
				CmdCheckOtherPlayerRoles();
			}
			else
			{
				ready = false;
				ChangeReadyButtonColor(notReadyColor);

				Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
				textComponent.text = isLocalPlayer ? "JOIN" : "...";
				readyButton.interactable = isLocalPlayer;

				// re-add listeners to role Button
				if (isLocalPlayer)
				{
					roleButton.onClick.RemoveAllListeners();
					roleButton.onClick.AddListener(OnRoleButtonClick);
				}
			}
		}

		private void SetupLocalPlayer()
		{
			//Change the color for the ready button
			ChangeReadyButtonColor(notReadyColor);
			OnClientReady(false);

			roleButton.interactable = true;
			//add ready callback to button
			readyButton.onClick.RemoveAllListeners();
			readyButton.onClick.AddListener(OnReadyClick);
			//add change role callback to button
			roleButton.onClick.RemoveAllListeners();
			roleButton.onClick.AddListener(OnRoleButtonClick);
			//add change drone perspective callback to toggle button
			droneHoloboardToggle.onValueChanged.RemoveAllListeners();
			droneHoloboardToggle.onValueChanged.AddListener(OnHoloboardPerspectiveToggled);

			//Send player name to server
			CmdChangeName(LobbyManager.LocalPlayerName);
			//Send player role to server
			CmdChangeRole(playerRole);
			//Send player perspective to server
			CmdChangePerspective(droneUsesHoloboardPerspective);
		}

		[Command]
		private void CmdChangePerspective(bool isHoloboard)
		{
			droneUsesHoloboardPerspective = isHoloboard;
		}

		[Command]
		private void CmdChangeName(string newName)
		{
			playerName = newName;
		}

		/// <summary>
		/// Syncs the Status of Free Classes wit other players
		/// </summary>
		[Command]
		private void CmdSyncFreePlayerRoles()
		{
			foreach (var nwPlayer in LobbyManager.Instance.lobbySlots)
			{
				var lobbyPlayer = nwPlayer as LobbyPlayer;
				if (lobbyPlayer != null && lobbyPlayer != this)
				{
					lobbyPlayer.isDroneTaken = isDroneTaken;
					lobbyPlayer.isMNITaken = isMNITaken;
					lobbyPlayer.isLSETaken = isLSETaken;
					lobbyPlayer.isGESTaken = isGESTaken;
					lobbyPlayer.isWITaken = isWITaken;
				}
			}
		}

		/// <summary>
		/// Checks if another Player has the same Role as current Player selected and changes Role
		/// </summary>
		[Command]
		private void CmdCheckOtherPlayerRoles()
		{
			foreach (var nwPlayer in LobbyManager.Instance.lobbySlots)
			{
				var lobbyPlayer = nwPlayer as LobbyPlayer;
				if (lobbyPlayer != null && lobbyPlayer != this)
				{
					if (lobbyPlayer.playerRole == this.playerRole)
					{
						lobbyPlayer.CmdAutoChangeRole();
					}
					// re-add Listener
					if (!lobbyPlayer.ready)
					{
						lobbyPlayer.roleButton.onClick.RemoveAllListeners();
						lobbyPlayer.roleButton.onClick.AddListener(lobbyPlayer.OnRoleButtonClick);
					}
				}
			}
		}

		/// <summary>
		/// Automaticly Changes Role to next avaliable Role
		/// </summary>
		[Command]
		private void CmdAutoChangeRole()
		{
			switch (playerRole)
			{
				case PlayerRole.MNI:
					if (!isWITaken) playerRole = PlayerRole.Wirtschaft;
					else if (!isGESTaken) playerRole = PlayerRole.Gesundheit;
					else if (!isLSETaken) playerRole = PlayerRole.LSE;
					else if (!isDroneTaken) playerRole = PlayerRole.Drone;
					break;
				case PlayerRole.Wirtschaft:
					if (!isGESTaken) playerRole = PlayerRole.Gesundheit;
					else if (!isLSETaken) playerRole = PlayerRole.LSE;
					else if (!isDroneTaken) playerRole = PlayerRole.Drone;
					else if (!isMNITaken) playerRole = PlayerRole.MNI;
					break;
				case PlayerRole.Gesundheit:
					if (!isLSETaken) playerRole = PlayerRole.LSE;
					else if (!isDroneTaken) playerRole = PlayerRole.Drone;
					else if (!isMNITaken) playerRole = PlayerRole.MNI;
					else if (!isWITaken) playerRole = PlayerRole.Wirtschaft;
					break;
				case PlayerRole.LSE:
					if (!isDroneTaken) playerRole = PlayerRole.Drone;
					else if (!isMNITaken) playerRole = PlayerRole.MNI;
					else if (!isWITaken) playerRole = PlayerRole.Wirtschaft;
					else if (!isGESTaken) playerRole = PlayerRole.Gesundheit;
					break;
				case PlayerRole.Drone:
					if (!isMNITaken) playerRole = PlayerRole.MNI;
					else if (!isWITaken) playerRole = PlayerRole.Wirtschaft;
					else if (!isGESTaken) playerRole = PlayerRole.Gesundheit;
					else if (!isLSETaken) playerRole = PlayerRole.LSE;
					break;
				default:
					return;
			}
		}

		/// <summary>
		/// Makes the given Role available again
		/// </summary>
		/// <param name="role">role to be free again</param>
		private void FreeRole(PlayerRole role)
		{
			switch (role)
			{
				case PlayerRole.Drone:
					isDroneTaken = false;
					break;
				case PlayerRole.MNI:
					isMNITaken = false;
					break;
				case PlayerRole.LSE:
					isLSETaken = false;
					break;
				case PlayerRole.Gesundheit:
					isGESTaken = false;
					break;
				case PlayerRole.Wirtschaft:
					isWITaken = false;
					break;
			}
			CmdSyncFreePlayerRoles();
		}

		/// <summary>
		/// Changes the Role of a Player to the Requested Role.
		/// Checks if requested Role is still free, otherwise, automaticly picks next free Role
		/// </summary>
		/// <param name="requestedRole"></param>
		[Command]
		private void CmdChangeRole(PlayerRole requestedRole)
		{
			ready = false;
			bool classFree = true;
			FreeRole(playerRole);

			// Checks if Requested Role is free, switches to next free otherwise
			switch (requestedRole){
				case PlayerRole.Drone:
					if (isDroneTaken)
					{
						CmdAutoChangeRole();
						classFree = false;
					}
					break;
				case PlayerRole.MNI:
					if (isMNITaken)
					{
						CmdAutoChangeRole();
						classFree = false;
					}
					break;
				case PlayerRole.LSE:
					if (isLSETaken)
					{
						CmdAutoChangeRole();
						classFree = false;
					}
					break;
				case PlayerRole.Gesundheit:
					if (isGESTaken)
					{
						CmdAutoChangeRole();
						classFree = false;
					}
					break;
				case PlayerRole.Wirtschaft:
					if (isWITaken)
					{
						CmdAutoChangeRole();
						classFree = false;
					}
					break;
			}
			// if requested Class is already used, return
			if (!classFree) return;

			playerRole = requestedRole;
		}

		/// <summary>
		/// Changes Role to the next Role when Role Button is Clicked
		/// </summary>
		private void OnRoleButtonClick()
		{
			switch (playerRole)
			{
				case PlayerRole.MNI:
					CmdChangeRole(PlayerRole.Wirtschaft);
					break;
				case PlayerRole.Wirtschaft:
					CmdChangeRole(PlayerRole.Gesundheit);
					break;
				case PlayerRole.Gesundheit:
					CmdChangeRole(PlayerRole.LSE);
					break;
				case PlayerRole.LSE:
					CmdChangeRole(PlayerRole.Drone);
					break;
				case PlayerRole.Drone:
					CmdChangeRole(PlayerRole.MNI);
					break;
				default:
					return;
			}
		}

		private void OnReadyClick()
		{
			if(isLocalPlayer)
				SendReadyToBeginMessage();
		}
		
		private void OnHoloboardPerspectiveToggled(bool isEnabled)
		{
			droneUsesHoloboardPerspective = isEnabled;
			if (isLocalPlayer)
			{
				CmdChangePerspective(droneUsesHoloboardPerspective);
			}
		}

		private void ShowHoloboardPerspectiveToggleSwitch(bool show)
		{
			if (isLocalPlayer)
			{
				droneHoloboardToggle.gameObject.SetActive(show);
			}
		}
	}
}
