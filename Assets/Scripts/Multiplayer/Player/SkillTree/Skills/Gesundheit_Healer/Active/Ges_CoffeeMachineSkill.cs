/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class implements a skill that spawns a healing Coffee Machine, which lasts 20 seconds.
/// It heals players in a radius around it
/// Its the ultimate Skill of the Healer class
/// </summary>
public class Ges_CoffeeMachineSkill : Skill
{
	public float radius = 15.0f;
	public override void Ability(PlayerController player)
	{
		if (!onCooldown)
		{
			player.CmdCreateCoffeeMachine(duration, radius);
			player.StartCoroutine(Cooldown(player));
			onCooldown = true;
			SkillCooldownController.Instance.startCooldown(duration, slot, cooldown);
		}
	}

	public override void Activate(PlayerController player)
	{
		onCooldown = false;
		duration = 20; //20s
		player.coffeemachine = true;
	}

	private IEnumerator Cooldown(PlayerController player)
	{
		//count to duration and increase the Number of charges afterwards
		yield return new WaitForSecondsRealtime(cooldown);
		onCooldown = false;
	}
}
