/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class implements the main ability of the tank/LSE.
/// It uses the Health class to give the player a shield.
/// </summary>
public class LSE_MainSkill : Skill
{

	public override void Ability(PlayerController player)
	{
		if (!onCooldown)
		{
			if (player.GetComponent<Multiplayer.Health>() != null)
			{
				var health = player.GetComponent<Multiplayer.Health>();
				int shieldValue = health.MAX_SHIELD;
				health.CmdShield(shieldValue);
				player.transform.GetComponent<AudioManager>().PlaySound(player.transform.position, 8);

			}
			SkillCooldownController.Instance.startCooldown(1,slot, cooldown);
			player.StartCoroutine(Cooldown(player));
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
