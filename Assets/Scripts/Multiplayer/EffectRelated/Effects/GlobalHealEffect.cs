using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect that heals all players in the game.
/// </summary>
public class GlobalHealEffect : PermanentEffect
{
	[SerializeField] private float changeHealthAmount;
	protected override void execEffect()
    {
	    GameOverManager.Instance.changeHealthAllPlayers(changeHealthAmount);
    }
}
