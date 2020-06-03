using System.Collections;
using System.Collections.Generic;
using Multiplayer.Lobby;
using UnityEngine;

/// <summary>
/// This class represents an effect that attaches a given amount of rotatingProjectiles to the target.
/// </summary>
public class AttachRotatingProjectileEffect : PermanentEffect
{
	[SerializeField] private RotatingProjectile rotatingProjectilePrefab;
	[SerializeField] private int amount;
 	protected override void execEffect()
	{
		target.spawnRotatingProjectiles(rotatingProjectilePrefab.gameObject,amount);
	}
}
