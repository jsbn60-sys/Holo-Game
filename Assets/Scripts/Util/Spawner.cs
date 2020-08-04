using System;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
using Multiplayer.Lobby;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class represents a spawner that spawns game objects
/// either at start or on destruction.
/// </summary>
public class Spawner : MonoBehaviour
{
	[SerializeField] private SpawnInfo[] spawnInfos;

	[SerializeField] private bool spawnOnlyOnDestruction;
	[SerializeField] private bool rotateWithOrigin;

	/// <summary>
	/// Start is called before the first frame update.
	/// Spawns the objects if spawn is on start.
	/// </summary>
	private void Start()
	{
		if (!spawnOnlyOnDestruction)
		{
			SpawnObjects();
		}
	}

	/// <summary>
	/// Is called before the game object is destroyed.
	/// Spawns the objects if spawn is on destruction.
	/// </summary>
	private void OnDestroy()
	{
		if (spawnOnlyOnDestruction)
		{
			SpawnObjects();
		}
	}


	/// <summary>
	/// Spawns the object locally,
	/// because this class is only used by objects that are spawned by effects, which are executed locally.
	/// </summary>
	private void SpawnObjects()
	{
		foreach (SpawnInfo spawnInfo in spawnInfos)
		{
			Quaternion spawnRotation;
			if (rotateWithOrigin)
			{
				spawnRotation = transform.rotation;
			}
			else
			{
				spawnRotation = Quaternion.identity;
			}

			Vector3 spawnPos = transform.position + spawnInfo.SpawnPos.RotatedBy(transform.rotation);

			Unit localPlayer = LobbyManager.Instance.LocalPlayerObject.GetComponent<Unit>();

			if (localPlayer.isServer)
			{
				localPlayer.CmdSpawn(
					LobbyManager.Instance.getIdxOfPrefab(spawnInfo.ObjectToSpawn),
					spawnPos,
					spawnRotation);
			}
		}
	}

	/// <summary>
	/// Stores information on which gameobject to spawn where.
	/// </summary>
	[System.Serializable]
	private struct SpawnInfo
	{
		[SerializeField] private GameObject objectToSpawn;
		[SerializeField] private Vector3 spawnPos;

		public GameObject ObjectToSpawn => objectToSpawn;
		public Vector3 SpawnPos => spawnPos;
	}
}
