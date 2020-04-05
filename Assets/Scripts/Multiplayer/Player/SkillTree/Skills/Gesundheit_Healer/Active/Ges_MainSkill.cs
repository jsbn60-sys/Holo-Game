/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The first and main active skill of the Healer(Gesundheit) class.
/// Its a buff that changes your default left click attack.
/// The player shoots bullets that heal his allies instead of damaging enemies.
/// The Skill gets modified by some other Abilities that are passive upgrades of this Skill.
/// The implementation of the Bullets can be found in the HealingBullet script.
/// </summary>
public class Ges_MainSkill : Skill
{
	private int skillDuration;
	private int cd;

	public override void Ability(PlayerController player)
	{
		if (player.permaHealingMode)
		{
			player.CmdToggleHealingMode(slot);
			player.CmdToggleGesMainSkillVar();

		} else
		{
			if (!onCooldown)
			{
				cd = cooldown;
				player.StartCoroutine(ResetShootingMode(player));
				player.StartCoroutine(Cooldown());
				player.CmdToggleHealingMode(slot);
				player.CmdToggleGesMainSkillVar();

				onCooldown = true;
				SkillCooldownController.Instance.startCooldown(duration, slot, cd);
			}
		}
		
	}

	public override void Activate(PlayerController player)
	{
		onCooldown = false;
		skillDuration = duration;
	}

	private IEnumerator ResetShootingMode(PlayerController player)
	{
		yield return new WaitForSecondsRealtime(skillDuration);
		player.healingMode = false;
		player.CmdToggleGesMainSkillVar();
	}

	public IEnumerator Cooldown()
	{
		yield return new WaitForSecondsRealtime(cd);
		onCooldown = false;
	}
}
