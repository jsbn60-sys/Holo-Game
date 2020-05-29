using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This class stores all spawnable itemprefabs and gives functionality for accessing them.
/// It is implemented as a singleton.
/// </summary>
public class ItemController : MonoBehaviour
{
	[SerializeField] private Item[] spawnableItemPrefabs;

	public static ItemController Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}

	public Item getRandomItemPrefab()
	{
		return spawnableItemPrefabs[Random.Range(0, spawnableItemPrefabs.Length - 1)];
	}
}
