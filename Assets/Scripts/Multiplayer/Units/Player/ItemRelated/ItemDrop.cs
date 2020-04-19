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
	public static ItemDrop instance;

	//List with items for the spawn
	public List<GameObject> itemPrefabs = new List<GameObject>();
	public GameObject supportSlowField;
	private int itemCount;
	private float itemDropChance = 0.1f;
	private float normalItemChance = 0.8f;
	int randomItemNum = 0;

	public void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}
	    
      
public void Start()
	{
		itemCount =itemPrefabs.Count;
	}

	/// <summary>
	/// Spawns an item at the specified location. The chances of items are defined above.
	/// </summary>
	public IEnumerator spawnItem(float x,float y,float z, bool safe)
	{
		Vector3 spawnPosition;
		float posX = x;
		float posY = y;
		float posZ = z;

		if (!safe)
		{
			float randomDropChance = Random.Range(0f, 1f);
			if (randomDropChance > itemDropChance) // don't spawn item
			{
				yield return null;
			}

			float randomItemChance = Random.Range(0f, 1f);
			
			if (randomItemChance < normalItemChance) //spawn normal item
			{
				randomItemNum = Random.Range(0, 7);
			} else //spawn good item
			{
				randomItemNum = Random.Range(7, 10);
			}
		} else
		{
			randomItemNum = Random.Range(0, 10);
		}

		GameObject itemPrefab = itemPrefabs[randomItemNum];
		spawnPosition = new Vector3(posX,posY,posZ);

		if (itemPrefab != null)
		{
			var spawnRotation = Quaternion.Euler(0.0f,UnityEngine.Random.Range(0, 180),0.0f);
			var item = (GameObject)Instantiate(itemPrefab, spawnPosition, spawnRotation);
			item.SetActive(true);
			NetworkServer.Spawn(item);
		}
		else
		{
			Debug.Log("ItemPrefab or Spawnposition is null");
		}
		yield return null;
	}

	public IEnumerator spawnDamageBoost(float x, float y, float z)
	{ 
		Vector3 spawnPos = new Vector3(x, y, z);
		GameObject damageBoostItem=null;
		foreach (GameObject item in itemPrefabs)
		{
			if(item.name == "DamageBoost")
			{
				 damageBoostItem = item;
			}
		}
		var damageBoost = Instantiate(damageBoostItem, spawnPos, Quaternion.identity);
		damageBoost.name = "damageBoost";
		NetworkServer.Spawn(damageBoost);
		yield return null;
	}
}
	

