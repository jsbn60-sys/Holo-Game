/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using UnityEngine;

/// <summary>
/// This class implements the taunt skill of the tank/LSE.
/// It calls the CmdTaunt() method of the PlayerController.
/// </summary>
public class LSE_TauntSkill : Skill
{
	public override void Ability(PlayerController player)
	{
		if (!onCooldown)
		{
			player.CmdTaunt(true);
			player.StartCoroutine(ResetTaunt(player));
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

	//The CmdTaunt() is called again to reverse the effects of the skill after waiting for <duration>
	private IEnumerator ResetTaunt(PlayerController player)
	{
		yield return new WaitForSecondsRealtime(duration);
		player.CmdTaunt(false);
		onCooldown = false;
	}
}
