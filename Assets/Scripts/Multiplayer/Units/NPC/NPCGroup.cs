using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

/// <summary>
/// This class represents a group of npcs
/// which are spawned together.
/// </summary>
public class NPCGroup : NetworkBehaviour
{
	[SerializeField] private SpawnInfo[] spawnInfos;

	private List<NPC> npcs;

	/// <summary>
	/// Spawns all npcs in this group in a radius
	/// around the npcgroup position.
	/// Only runs on the server.
	/// </summary>
	[Server]
	public void spawnGroup()
	{
		npcs = new List<NPC>();
		foreach (SpawnInfo spawnInfo in spawnInfos)
		{
			for (int i = 0; i < spawnInfo.SpawnAmount; i++)
			{
				Vector3 spawnPos = this.transform.position + (Vector3)(Random.insideUnitCircle * spawnInfo.SpawnAmount);
				GameObject npc = Instantiate(spawnInfo.NpcPrefab,spawnPos,Quaternion.identity, this.transform);
				NetworkServer.Spawn(npc);
				npcs.Add(npc.GetComponent<NPC>());
				npc.GetComponent<NPC>().Group = this;
				NPCController.Instance.increaseNpcCount();
			}
		}
	}

	/// <summary>
	/// Removes the dead npc from this group.
	/// Destroys this group if there is no npc left.
	/// </summary>
	/// <param name="npc"></param>
	public void removeNpc(NPC npc)
	{
		npcs.Remove(npc);
		NPCController.Instance.reduceNpcCount();
		if (npcs.Count == 0)
		{
			Destroy(this.gameObject);
		}
	}

	/// <summary>
	/// Stuns or unstuns this group.
	/// </summary>
	/// <param name="turnOn">Should the group be stunned</param>
	public void stunGroup(bool turnOn)
	{
		foreach (NPC npc in npcs)
		{
			npc.changeStunned(turnOn);
		}
	}

	/// <summary>
	/// Workaround for storing a npcPrefab and how much of it
	/// should be spawned.
	/// </summary>
	[System.Serializable]
	private class SpawnInfo
	{
		[SerializeField] private GameObject npcPrefab;
		[SerializeField] private int spawnAmount;

		public GameObject NpcPrefab => npcPrefab;
		public int SpawnAmount => spawnAmount;
	}
}
