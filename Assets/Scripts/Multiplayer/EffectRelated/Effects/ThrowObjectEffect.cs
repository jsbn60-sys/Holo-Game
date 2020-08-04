using System.Collections;
using System.Collections.Generic;
using Multiplayer.Lobby;
using UnityEngine;

/// <summary>
/// This class represents an effect which throws an object from the players position.
/// IMPORTANT: THIS EFFECT ONLY WORKS FOR PLAYERS AND DRONES.
/// </summary>
public class ThrowObjectEffect : PermanentEffect
{
	[SerializeField]
	private Projectile projectileToThrow;

	[SerializeField] private bool isDrone;
	protected override void execEffect()
	{
		if (isDrone)
		{
			target.GetComponent<Drone>().CmdShoot(LobbyManager.Instance.getIdxOfPrefab(projectileToThrow.gameObject),0f,0f);
		}
		else
		{
			target.GetComponent<Player>().shoot(LobbyManager.Instance.getIdxOfPrefab(projectileToThrow.gameObject),0);
		}
	}
}
