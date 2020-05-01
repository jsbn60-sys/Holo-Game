using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect that changes the cooldown of all player skills permanently.
/// IMPORTANT: THIS EFFECT CAN ONLY BE APPLIED TO PLAYERS.
/// </summary>
public class SkillsCooldownChangeEffect : PermanentEffect
{
	[SerializeField] private float cooldownChangeFactor;
	protected override void execEffect()
	{
		target.GetComponent<Player>().changeSkillsCooldown(cooldownChangeFactor);
	}
}
