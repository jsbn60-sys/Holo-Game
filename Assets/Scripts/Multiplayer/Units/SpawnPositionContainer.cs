using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple container for spawn transforms.
/// </summary>
public class SpawnPositionContainer : MonoBehaviour
{
	[SerializeField] private Transform[] spawnPositions;

	public Transform[] SpawnPositions => spawnPositions;
}
