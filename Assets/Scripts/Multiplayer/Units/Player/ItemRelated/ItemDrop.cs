/* edited by: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


/// <summary>
/// This class is called at the start of the multiplayer scene. The ItemSpawner spawns items at random positions on the map.
/// The items are spawned with different frequencies to provide a balanced game experience.
/// </summary>
public class ItemDrop : NetworkBehaviour
{
	public static void SpawnItem(Item itemPrefab, Vector3 spawnPos, Transform parent )
	{

		Item itemCopy = Instantiate(itemPrefab,parent);
		NetworkServer.Spawn(itemCopy.gameObject);
	}
}


