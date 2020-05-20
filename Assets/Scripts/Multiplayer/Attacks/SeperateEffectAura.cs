using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an Aura that has additional effects
/// that are applied to all players in the game depending on the
/// amount of units inside.
/// </summary>
public class SeperateEffectAura : Aura
{
	private Player[] players;
	[SerializeField] private Effect[] onlyPlayerEffects;

	/// <summary>
	/// Start is called before the first frame update.
	/// Gets all players.
	/// </summary>
	private void Start()
	{
		base.Start();
		players = GameOverManager.Instance.getAllPlayers();
	}

	/// <summary>
	/// Calls tickHit of the base class
	/// and attaches effects to all players
	/// for each unit inside.
	/// </summary>
	protected override void tickHit()
	{
		base.tickHit();
		foreach (Player player in players)
		{
			for (int i = 0; i < unitsInside.Count; i++)
			{
				foreach (Effect effect in onlyPlayerEffects)
				{
					player.attachEffect(effect);
				}
			}
		}
	}
}
