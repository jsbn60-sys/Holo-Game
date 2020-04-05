/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the random item drop skill
/// </summary>
public class WS_MainSkill : Skill
{
	private int cd;

	public override void Ability(PlayerController player)
	{
		if (!onCooldown)
		{
			if (player.isCRSkilled)
			{
				cd = 15;
			}
			else
			{
				cd = 20;
			}

			player.CmdSupportSpawnItem();
			SkillCooldownController.Instance.startCooldown(duration, slot, cd);
			onCooldown = true;
			player.StartCoroutine(Cooldown());
		}
	}

	public override void Activate(PlayerController player)
	{
		onCooldown = false;
	}

	private IEnumerator Cooldown()
	{
		//count to duration and set onCooldown to false
		yield return new WaitForSecondsRealtime(cd);
		onCooldown = false;
	}
}
