using System.Collections;
using System.Collections.Generic;
using Multiplayer.Lobby;
using UnityEngine;

/// <summary>
/// This class represents an effect that throws multiple objects in a cone downwards over a given time.
///
/// IMPORTANT: THIS CLASS CAN ONLY BE USED BY THE DRONE.
/// </summary>
public class ThrowManyObjectsInConeTickEffect : TickingEffect
{
	[SerializeField] private GameObject projectToThrow;
	[SerializeField] private int throwAmount;
	[SerializeField] private float degree;

	protected override void execEffect()
	{
		int prefabIdx = LobbyManager.Instance.getIdxOfPrefab(projectToThrow);
		for (int i = 0; i < throwAmount; i++)
		{
			target.GetComponent<Drone>()
				.CmdShoot(prefabIdx, Random.Range(degree, -degree), Random.Range(degree, -degree));
		}
	}
}
