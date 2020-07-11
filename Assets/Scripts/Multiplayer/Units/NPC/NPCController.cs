using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// This class handles spawning npcgroups and updating UI.
/// It can also be used to apply effects to all npcs.
/// It is implemented as a singleton pattern.
/// </summary>
public class NPCController : NetworkBehaviour
{
	[SerializeField] private Wave[] waves;

	public static NPCController Instance { get; private set; }

	[SerializeField] private Text waveCountText;
	private int currWaveIdx;
	private List<NPCGroup> currAliveGroups;

	/// <summary>
	/// Sets singleton instance.
	/// </summary>
	void Awake()
	{
		Instance = this;
	}

	/// <summary>
	/// Start is called before the first frame update.
	/// </summary>
	private void Start()
	{
		if (!isServer) return;

		currWaveIdx = 0;
		currAliveGroups = new List<NPCGroup>();
		SpawnNextWave();
	}

	/// <summary>
	/// Spawns the next wave.
	/// </summary>
	public void SpawnNextWave()
	{
		if (!isServer) return;
		waves[currWaveIdx].StartWave();
		CmdUpdateWaveUI((currWaveIdx + 1) + ". Semester");
		currWaveIdx++;
	}

	/// <summary>
	/// Updates the wave ui on all clients.
	/// </summary>
	/// <param name="waveCountText">Wave text</param>
	[Command]
	private void CmdUpdateWaveUI(string waveCountText)
	{
		RpcUpdateWaveUI(waveCountText);
	}

	/// <summary>
	/// Updates the wave ui on all clients.
	/// </summary>
	/// <param name="waveCountText">Wave text</param>
	[ClientRpc]
	private void RpcUpdateWaveUI(string waveCountText)
	{
		this.waveCountText.text = waveCountText;
	}


	/// <summary>
	/// Removes a npc group from the alive groups.
	/// </summary>
	/// <param name="npcGroup">group to remove</param>
	public void removeAliveGroup(NPCGroup npcGroup)
	{
		currAliveGroups.Remove(npcGroup);
	}

	/// <summary>
	/// Adds a new group as alive.
	/// </summary>
	/// <param name="npcGroup">Group to add</param>
	public void addAliveGroup(NPCGroup npcGroup)
	{
		currAliveGroups.Add(npcGroup);
	}


	/// <summary>
	/// Stuns or unstuns all npcs.
	/// </summary>
	/// <param name="turnOn">Should it stun</param>
	public void changeAllStunned(bool turnOn)
	{
		foreach (NPCGroup npcGroup in currAliveGroups)
		{
			npcGroup.stunGroup(turnOn);
		}
	}
}
