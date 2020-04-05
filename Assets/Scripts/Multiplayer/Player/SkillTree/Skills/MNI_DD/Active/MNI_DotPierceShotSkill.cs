/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// This class implements the PierceShot Active Skill of a MNI Player.
/// When this active Skill is activated, all the Players shots will
/// pierce through enemys and Aplly DoT dmg on them.
/// Calls a Method from the Player controller that Changes the Bullets to Pierce and Apply Dots
///</summary>
public class MNI_DotPierceShotSkill : Skill
{
	private int cd;
	public int pierces = 4;

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
			player.StartCoroutine(ResetDamage(player));
			player.StartCoroutine(Cooldown());
			player.CmdChangePierces(true, pierces);
			player.activeDotSkill = true;
			onCooldown = true;
		}
	}

	public override void Activate(PlayerController player)
	{
		onCooldown = false;
	}

	private IEnumerator ResetDamage(PlayerController player)
	{
		//count to duration and Reset Vars after
		yield return new WaitForSecondsRealtime(duration);
		player.CmdChangePierces(false, pierces);
		player.activeDotSkill = false;
	}

	private IEnumerator Cooldown()
	{
		//count to duration and set onCooldown to false
		yield return new WaitForSecondsRealtime(cd);
		onCooldown = false;
	}

}
