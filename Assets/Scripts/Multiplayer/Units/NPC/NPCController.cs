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
	[SerializeField] private SpawnPositionContainer spawnPosContainer;

	public static NPCController Instance { get; private set; }

	[SerializeField] private float countdown;
	[SerializeField] private Text countdownText;
	[SerializeField] private Text waveCountText;
	private float countdownTimer;
	private int npcCount;
	private int currWaveIdx;
	private bool allNpcsAreDead;
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
		currAliveGroups = new List<NPCGroup>();
		countdownTimer = countdown;
		allNpcsAreDead = true;
		currWaveIdx = 0;
	}

	/// <summary>
	/// Update is called once per frame.
	/// Updates countdown and spawns wave,
	/// when countdown is expired.
	/// </summary>
	private void Update()
	{
		if (isServer && allNpcsAreDead)
		{
			if (countdownTimer > 0f)
			{
				countdownTimer -= Time.deltaTime;

				CmdUpdateWaveUI("Wave Countdown: " + (int) countdownTimer,(currWaveIdx + 1) + ". Semester");
			}
			else
			{
				allNpcsAreDead = false;
				CmdUpdateWaveUI("",(currWaveIdx + 1) + ". Semester");
				countdownTimer = countdown;
				spawnWave();
			}
		}
	}

	[Command]
	private void CmdUpdateWaveUI(string countdownText, string waveCountText)
	{
		RpcUpdateWaveUI(countdownText,waveCountText);
	}

	[ClientRpc]
	private void RpcUpdateWaveUI(string countdownText, string waveCountText)
	{
		this.countdownText.text = countdownText;
		this.waveCountText.text = waveCountText;
	}

	/// <summary>
	/// Spawns a wave by spawning all its npcgroups.
	/// The spawning of the npcs is handled by the npcgroups.
	/// Only runs on the server.
	/// </summary>
	[Server]
	private void spawnWave()
	{
		for (int i = 0; i < waves[currWaveIdx].NpcGroups.Length; i++)
		{
			NPCGroup npcGroup = Instantiate(waves[currWaveIdx].NpcGroups[i],
				spawnPosContainer.SpawnPositions[i].position, Quaternion.identity);
			NetworkServer.Spawn(npcGroup.gameObject);
			npcGroup.spawnGroup();
			currAliveGroups.Add(npcGroup);
		}

		currWaveIdx++;
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
	/// Reduces npcCount by 1.
	/// Only runs on the server.
	/// </summary>
	[Server]
	public void reduceNpcCount()
	{
		npcCount--;
		if (npcCount <= 0)
		{
			allNpcsAreDead = true;
		}
	}

	/// <summary>
	/// Increases the npcCount by 1.
	/// Only runs on the server.
	/// </summary>
	[Server]
	public void increaseNpcCount()
	{
		npcCount++;
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

	/// <summary>
	/// Workaround for storing the npcgroups of a wave,
	/// so it can be used in the editor.
	/// (Using a 2d-array of npcgroup won't show correctly)
	/// </summary>
	[System.Serializable]
	private struct Wave
	{
		[SerializeField] private NPCGroup[] npcGroups;
		public NPCGroup[] NpcGroups => npcGroups;
	}

}
