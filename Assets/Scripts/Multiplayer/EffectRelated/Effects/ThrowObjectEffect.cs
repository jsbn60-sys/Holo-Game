using System.Collections;
using System.Collections.Generic;
using Multiplayer.Lobby;
using UnityEngine;

/// <summary>
/// This class represents an effect which throws an object from the players position.
/// IMPORTANT: THIS EFFECT ONLY WORKS FOR PLAYERS.
/// </summary>
public class ThrowObjectEffect : PermanentEffect
{
	[SerializeField]
	private Projectile explosionProjectile;

	protected override void execEffect()
	{

		target.GetComponent<Player>().shoot(LobbyManager.Instance.getIdxOfPrefab(explosionProjectile.gameObject));
	}
}
