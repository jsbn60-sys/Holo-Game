/* edited by: SWT-P_SS_2019_Holo */
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine.UI;

namespace Multiplayer.Lobby
{
	[RequireComponent(typeof(LobbyManager))]
	public class LobbyMenu : MonoBehaviour {

		[SerializeField]
		private LobbyManager lobbyManager;

		[SerializeField]
		private InputField ipInput;
		[SerializeField]
		private InputField playerNameInput;

		[SerializeField]
		private Dropdown ipList;

		public string ipLogName;
		private String[] ips;

		private void Awake()
		{
			String path = Path.Combine(Application.persistentDataPath, "saves", ipLogName + ".txt");

			ipList.ClearOptions();
			if (File.Exists(path))
			{
				ips = File.ReadAllLines(path);
				List<string> logList = new List<string>(ips);
				ipList.AddOptions(logList);
			}

			if(!File.Exists(path) || ips == null || ips.Length == 0)
			{
				ipList.enabled = false;
				ipList.gameObject.SetActive(false);
			}

		}


		public void OnSelectLastIPList()
		{
			ipInput.SetTextWithoutNotify(ipList.options[ipList.value].text); //selected item

		}

		public void OnClickLastIP()
		{
			ipList.ClearOptions();
			string[] logFile = File.ReadAllLines(ipLogName);
			List<string> logList = new List<string>(logFile);
			ipList.AddOptions(logList);
		}

		public void OnClickHost()
		{
			if (!playerNameInput.text.Equals("")){
				//Start new Server
				SafeIP();

				NetworkClient nc = lobbyManager.StartHost();
				if (nc == null)
				{
					lobbyManager.StopHostClbk();
					lobbyManager.SetStatusInfo("Failed to start server!");
				}

				//Set player name
				LobbyManager.LocalPlayerName = playerNameInput.text;
			} else
			{
				StartCoroutine(playerNameInput.GetComponent<BlinkingImage>().blinkOnce());
			}
		}
		
		public void OnClickJoin()
		{
			SafeIP();

			//Set player name
			LobbyManager.LocalPlayerName = playerNameInput.text;
			//Set network address
			lobbyManager.networkAddress = ipInput.text;
			//Start client
			lobbyManager.StartClient();
			//Set status info
			lobbyManager.SetStatusInfo("Connecting...");

		}

		public void SafeIP()
		{
			String path = Path.Combine(Application.persistentDataPath, "saves", ipLogName + ".txt");
			String ip = ipInput.text;

			if (ips != null && ips.Contains(ip)){
				// ip already in List, break
				return;
			}

			if (!Directory.Exists(Path.GetDirectoryName(path)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(path));
			}

			if (File.Exists(path))
			{
				var lines = File.ReadAllLines(path);
				if (lines.Length >= 10)
				{
					File.WriteAllText(path, "");
					for (int i = 1; i < 10; i++)
						{
						File.AppendAllText(path, lines[i] + Environment.NewLine);
					}				
				}
				File.AppendAllText(path, ip + Environment.NewLine);
			}
			else
			{
				//create file
				File.WriteAllText(path, ip + Environment.NewLine);
			}
		}

		public void OnDisconnect()
		{
			lobbyManager.disconncetDelegate();
		}
	}
}
