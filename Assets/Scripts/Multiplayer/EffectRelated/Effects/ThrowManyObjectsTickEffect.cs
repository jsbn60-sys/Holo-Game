using System.Collections;
using System.Collections.Generic;
using Multiplayer.Lobby;
using UnityEngine;

/// <summary>
/// This class represents an effect which throws a given amount of instances of a projectile
/// in a cone infront of the player with a certain tickRate.
/// </summary>
public class ThrowManyObjectsTickEffect : TickingEffect
{
	[SerializeField] private GameObject projectToThrow;
	[SerializeField] private int throwAmount;
	private float currDegree;
	protected override void execEffect()
	{
		int prefabIdx = LobbyManager.Instance.getIdxOfPrefab(projectToThrow);
		currDegree = -20;
		for (int i = 0; i < throwAmount; i++)
		{
			target.GetComponent<Player>().shoot(prefabIdx,currDegree);
			currDegree += 40f/(throwAmount-1);
		}
		currDegree = -20;
	}
}
