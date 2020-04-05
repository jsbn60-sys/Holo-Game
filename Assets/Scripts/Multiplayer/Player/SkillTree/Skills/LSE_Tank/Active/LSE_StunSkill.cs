/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class implements the stun skill of the tank/LSE.
/// It calls the CmdStun() method of the PlayerController.
/// </summary>
public class LSE_StunSkill : Skill
{
	public override void Ability(PlayerController player)
	{
		if (!onCooldown)
		{
			player.CmdStun(duration);
			player.StartCoroutine(Cooldown(player));
			SkillCooldownController.Instance.startCooldown(duration, slot, cooldown);
			onCooldown = true;
		}
	}

	public override void Activate(PlayerController player)
	{
		onCooldown = false;
	}

	private IEnumerator Cooldown(PlayerController player)
	{
		yield return new WaitForSecondsRealtime(cooldown);
		onCooldown = false;
	}
}
