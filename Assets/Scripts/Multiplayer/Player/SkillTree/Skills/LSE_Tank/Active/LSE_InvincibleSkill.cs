/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class implements the invincibility skill of the tank/LSE.
/// It calls the CmdInvc() method of the PlayerController.
/// </summary>
public class LSE_InvincibleSkill : Skill
{
	public override void Ability(PlayerController player)
	{
		if (!onCooldown)
		{
			player.CmdInvc(true);
			player.StartCoroutine(ResetDamage(player));
			player.StartCoroutine(Cooldown(player));
			SkillCooldownController.Instance.startCooldown(duration, slot, cooldown);
			onCooldown = true;
		}
	}

	public override void Activate(PlayerController player)
	{
		onCooldown = false;
	}

	//The CmdInvc() is called again to reverse the effects of the skill after waiting for <duration>
	private IEnumerator ResetDamage(PlayerController player)
	{
		yield return new WaitForSecondsRealtime(duration);
		player.CmdInvc(false);
	}

	private IEnumerator Cooldown(PlayerController player)
	{
		yield return new WaitForSecondsRealtime(cooldown);
		onCooldown = false;
	}
}
