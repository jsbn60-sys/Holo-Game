using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
using UnityEngine;
using UnityEngine.Networking;

public class ItemSpawner : MonoBehaviour
{
	[System.Serializable]
	private struct SpawnInfo
	{
		[SerializeField] private Item itemToSpawn;
		[SerializeField] private Vector3 spawnPos;

		public Item ItemToSpawn => itemToSpawn;
		public Vector3 SpawnPos => spawnPos;
	}

	[SerializeField] private SpawnInfo[] spawnInfos;
    // Start is called before the first frame update
    void Start()
    {
	    foreach (SpawnInfo spawnInfo in spawnInfos)
	    {
		    Vector3 spawnPos = transform.position + spawnInfo.SpawnPos.RotatedBy(transform.rotation);
		    Item item = Instantiate(spawnInfo.ItemToSpawn, spawnPos, Quaternion.identity, transform);
		    NetworkServer.Spawn(item.gameObject);
	    }
    }
}
