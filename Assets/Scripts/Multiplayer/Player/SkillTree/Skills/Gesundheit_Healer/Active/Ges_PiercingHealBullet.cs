/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class implements an active ability of the Healer.
/// Fires a strong healing bullet that can heal up to 2 allies by piercing the first ally hit.
/// The cooldown is reduced for every normal attack of the Healer that hits an enemy.
/// The bullet fired is a modified HealingBullet
/// </summary>
public class Ges_PiercingHealBullet : Skill
{
	public int healValue = 50;
	private int cdi = 0; //cooldown counter for cooldown reduction on hit
	public int cdrVal = 1; //cooldown reduction value on hit in seconds

	public override void Ability(PlayerController player)
	{
		if (!onCooldown)
		{
			player.cdPiercingHealBullet = cooldown;
			player.CmdStrongHealingBullet(healValue);
			player.StartCoroutine(Cooldown(player));
			onCooldown = true;
			SkillCooldownController.Instance.StartReducableCooldown(duration, slot, cooldown, id);
		}
	}

	public override void Activate(PlayerController player)
	{
		onCooldown = false;
		player.piercingHealBullet = true;
	}

	private IEnumerator Cooldown(PlayerController player)
	{
		//count to duration and increase the Number of charges afterwards
		for(cdi = 0; cdi < player.cdPiercingHealBullet; cdi++)
		{
			yield return new WaitForSecondsRealtime(1);
		}
		onCooldown = false;
	}

	
}
