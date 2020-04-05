/* author: SWT-P_WS_2018_Holo */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


/// <summary>
/// This class is called at the start of the multiplayer scene. The ItemSpawner spawns items at random positions on the map.
/// The items are spawned with different frequencies to provide a balanced game experience.
/// </summary>
public class ItemSpawner : NetworkBehaviour
{
	//List with items for the spawn
	public List<GameObject> itemPrefabs = new List<GameObject>();
	// number of items to spawn
	public int numberOfItems;
	// percent per group items
	public List<int> percent = new List<int>();
	// count items in the group
	public List<int> countItem = new List<int>();
	// spawn probability per every items
	private List<int> everyItemCount = new List<int>();

	public override void OnStartServer()
	{
		int everyItemNumber = 0;
		int allPercent = 0, allItem = 0;
		Vector3 spawnPosition;

		//calculation of elements per percent
		for (int i = 0; i < percent.Count; i++)
		{
			allPercent = allPercent + percent[i] * countItem[i];
			allItem = allItem + countItem[i];
		}

		// if count of items in the list "countItem" is less then count of items in the list "itemPrefabs"
		if (allItem != itemPrefabs.Count)
		{
			Debug.Log("quantities of items error");
		}
		else
		{
			//for every item in list "countItem"
			for (int i = 0; i < countItem.Count; i++)
			{
				double m;
				// every Item gets own percent count
				for (int k = 0; k < countItem[i]; k++)
				{

					m = percent[i] / (allPercent / numberOfItems);
					everyItemCount.Add(System.Convert.ToInt16(System.Math.Round(m)));
					everyItemNumber++;
				}
			}

			//quantities of items per prefabs
			for (int h = 0; h < everyItemCount.Count; h++)
			{
				//quantities of items same prefabs
				for (int c = 0; c < everyItemCount[h]; c++)
				{
					GameObject itemPrefab = itemPrefabs[h];
					spawnPosition = new Vector3(Random.Range(-100, 80), -0.16f, Random.Range(-50, 70));
					if (itemPrefab != null)
					{
						var spawnRotation = Quaternion.Euler(
							0.0f,
							UnityEngine.Random.Range(0, 180),
							0.0f);
						var item = (GameObject)Instantiate(itemPrefab, spawnPosition, spawnRotation);

						if (BuildingController(item))
						{
							Destroy(item);
							c--;
						}
						NetworkServer.Spawn(item);
					}
					else
					{
						Debug.Log("ItemPrefab or Spawnposition is null");
					}
				}
			}
		}
	}

	/// <summary>
	/// Makes sure that no items are in the buildings.
	/// </summary>
	private bool BuildingController(GameObject item)
	{
		string[] buildingName = { "A10", "A11", "A12", "A13", "A14", "A15", "A16", "A20", "A21", "A22", "C" };
		GameObject build;
		Collider box;
		GameObject obj;
		var boxCollider = item.transform.position;

		for (int i = 0; i < buildingName.Length; i++)
		{
			build = GameObject.FindGameObjectWithTag(buildingName[i]);
			box = build.GetComponent<Collider>();
			obj = item.gameObject;
			if (box.bounds.Contains(transform.TransformPoint(boxCollider)))
			{
				return true;
			}
		}
		return false;
	}
}
