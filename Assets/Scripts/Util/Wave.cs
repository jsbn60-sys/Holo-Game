using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class represents a wave of enemies.
/// A wave spawns groups of npcs in an interval
/// and activates doors on finish.
/// </summary>
public abstract class Wave : NetworkBehaviour
{
	/// <summary>
	/// Doors to activate on finish.
	/// </summary>
	[SerializeField] private Door[] doors;

	/// <summary>
	/// npc groups to spawn
	/// </summary>
	[SerializeField] private NPCGroup[] npcGroups;

	/// <summary>
	/// Interval to spawn npcs in
	/// </summary>
	[SerializeField] private float spawnInterval;

	/// <summary>
	/// Spawn positions of npcs
	/// </summary>
	[SerializeField] private Transform[] spawnPositions;

	/// <summary>
	/// Rest time until next spawn
	/// </summary>
	private float intervalTimer;

	/// <summary>
	/// Is this wave active
	/// </summary>
	protected bool isActive;

	/// <summary>
	/// Starts this wave.
	/// </summary>
	public void StartWave()
	{
		intervalTimer = spawnInterval;
		isActive = true;
		initWave();
	}

	/// <summary>
	/// Initializes this wave.
	/// </summary>
	protected abstract void initWave();

	/// <summary>
	/// Checks if the active condition is still true.
	/// </summary>
	protected abstract void UpdateActiveCondition();

	/// <summary>
	/// Gets the text displayed on the doors.
	/// </summary>
	/// <returns>Text to display</returns>
	protected abstract string getDoorText();

	/// <summary>
	/// Update is called once per frame.
	/// </summary>
	protected void Update()
	{
		if (!isActive) return;
		UpdateActiveCondition();
		updateDoors();

		if (!isActive)
		{
			NPCController.Instance.SpawnNextWave();
		}

		if (intervalTimer > 0f)
		{
			intervalTimer -= Time.deltaTime;

			if (intervalTimer <= 0f)
			{
				SpawnGroups();
				intervalTimer = spawnInterval;
			}
		}
	}

	/// <summary>
	/// Spawns the npc groups.
	/// </summary>
	private void SpawnGroups()
	{
		for (int i = 0; i < npcGroups.Length; i++)
		{
			NPCGroup npcGroup = Instantiate(npcGroups[i],
				spawnPositions[i % spawnPositions.Length].position, Quaternion.identity);
			NetworkServer.Spawn(npcGroup.gameObject);
			npcGroup.spawnGroup();
			NPCController.Instance.addAliveGroup(npcGroup);
		}
	}

	/// <summary>
	/// Updates text of doors or closes them if wave is finished.
	/// </summary>
	private void updateDoors()
	{
		if (!isActive)
		{
			CmdActivateDoors();
		}
		else
		{
			CmdUpdateDoors(getDoorText());
		}
	}

	/// <summary>
	/// Updates all clients that the doors are closing.
	/// </summary>
	[Command]
	private void CmdActivateDoors()
	{
		RpcActivateDoors();
	}

	/// <summary>
	/// Updates all clients that the doors are closing.
	/// </summary>
	[ClientRpc]
	private void RpcActivateDoors()
	{
		foreach (Door door in doors)
		{
			door.activateDoor();
		}
	}

	/// <summary>
	/// Updates all clients about the door text.
	/// </summary>
	/// <param name="doorText">Door text to display</param>
	[Command]
	private void CmdUpdateDoors(string doorText)
	{
		RpcUpdateDoors(doorText);
	}

	/// <summary>
	/// Updates all clients about the door text.
	/// </summary>
	/// <param name="doorText">Door text to display</param>
	[ClientRpc]
	private void RpcUpdateDoors(string doorText)
	{
		foreach (Door door in doors)
		{
			door.updateDoor(doorText);
		}
	}
}
