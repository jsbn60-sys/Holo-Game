/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// This class implements the WrathOfDean Active Skill of a MNI Player.
/// When this active Skill is activated, a rain of Bullet will rain upon Enemys
/// Calls the Method from the Player Controller to use the Skill.
/// Bullets are spawnd in Waves around the Position the Player chooses with the Mouse
///</summary>
public class MNI_WrathOfDeanSkill : Skill
{
	public float radius = 5.0f;
	private int cd;

	public override void Ability(PlayerController player)
	{
		if (!onCooldown)
		{
			// checks if CoolDownSkill is active and reduces cooldown acordingly
			if (player.coolDownSkill)
			{
				cd = cooldown - (player.investedSkillPoints * 3);
			}
			else
			{
				cd = cooldown;
			}
			SkillCooldownController.Instance.startCooldown(duration, slot, cd);
			player.StartCoroutine(Cooldown());
			player.CmdActivateWrathOfDean(duration, radius);
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
