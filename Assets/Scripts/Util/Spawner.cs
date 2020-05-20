using System;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
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
    /// Spawns the objects on the network.
    /// </summary>
    private void SpawnObjects()
    {
	    foreach (SpawnInfo spawnInfo in spawnInfos)
	    {
		    Vector3 spawnPos = transform.position + spawnInfo.SpawnPos.RotatedBy(transform.rotation);
		    GameObject objectCopy = Instantiate(spawnInfo.ObjectToSpawn, spawnPos, transform.rotation);
		    NetworkServer.Spawn(objectCopy.gameObject);
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
