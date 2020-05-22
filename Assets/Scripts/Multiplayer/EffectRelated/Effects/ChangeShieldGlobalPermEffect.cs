using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect that changes the shield of all players.
/// </summary>
public class ChangeShieldGlobalPermEffect : PermanentEffect
{
	[SerializeField] private float changeShieldAmount;

	protected override void execEffect()
	{
		GameOverManager.Instance.changeShieldAllPlayers(changeShieldAmount);
	}
}
