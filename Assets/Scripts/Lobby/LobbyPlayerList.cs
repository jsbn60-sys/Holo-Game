using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer.Lobby
{
	public class LobbyPlayerList : MonoBehaviour
	{
		public static LobbyPlayerList	Instance = null;

		public RectTransform			playerListContentTransform;

		private VerticalLayoutGroup	layout;
		private List<LobbyPlayer>	players = new List<LobbyPlayer>();

		public void OnEnable()
		{
			Instance = this;
			layout = playerListContentTransform.GetComponent<VerticalLayoutGroup>();
		}

		private void Update()
		{
			//this dirty the layout to force it to recompute evryframe (a sync problem between client/server
			//sometime to child being assigned before layout was enabled/init, leading to broken layouting)

			if(layout)
				layout.childAlignment = Time.frameCount%2 == 0 ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
		}

		public void AddPlayer(LobbyPlayer player)
		{
			if (players.Contains(player))
				return;

			players.Add(player);
			player.transform.SetParent(playerListContentTransform, false);
		}

		public void RemovePlayer(LobbyPlayer player)
		{
			players.Remove(player);
		}

		public void ResetList()
		{
			players = new List<LobbyPlayer>();
		}
	}
}

