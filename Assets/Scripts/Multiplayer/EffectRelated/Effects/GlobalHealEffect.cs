using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect that heals all players in the game fully.
/// </summary>
public class GlobalHealEffect : SingleUseEffect
{
	protected override void execEffect()
    {
	    GameOverManager.Instance.RecoverProfs();
    }
}
