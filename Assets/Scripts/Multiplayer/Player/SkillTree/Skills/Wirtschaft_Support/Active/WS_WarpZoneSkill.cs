/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the warp zone skill
/// </summary>
public class WS_WarpZoneSkill : Skill
{
	private int cd;

	public override void Ability(PlayerController player)
	{

		if (!onCooldown)
		{
			if (player.isCRSkilled)
			{
				cd = 50;
			}
			else
			{
				cd = 60;
			}

			player.CmdSupportWarpZone();

			SkillCooldownController.Instance.startCooldown(duration, slot, cd);
			player.StartCoroutine(Cooldown());
			onCooldown = true;
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
