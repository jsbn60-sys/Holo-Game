/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
/// This class implements the Main Active Skill of a MNI Player.
/// The active Abillity of this Skill increases the dmg the player deals.
/// The Skill calls a function from the PlayerController that sets the dmg Multiplier to its boost value.
/// Boosts Dmg by "boostValue".
/// A call of this Ability starts two Coroutines, one that manages the Cooldown and one that keeps the skill active for its duration
///</summary>
public class MNI_MainSkill : Skill
{
	private int skillDuration;
	public float boostValue = 1.2f; // boost value 
	private int cd;

	public override void Ability(PlayerController player)
	{

		if (!onCooldown)
		{
			// checks if CoolDownSkill is active and reduces cooldown acordingly
			if (player.coolDownSkill)
			{
				cd = cooldown - (player.investedSkillPoints * 3);
			} else
			{
				cd = cooldown;
			}
			SkillCooldownController.Instance.startCooldown(duration,slot,cd);
			player.StartCoroutine(ResetDamage(player));
			player.StartCoroutine(Cooldown());
			player.CmdChangeDmgMultiplier(true);
			player.CmdToggleMniMainSkillVar();
	
		
			onCooldown = true;
		}

	}

	public override void Activate(PlayerController player)
	{
		onCooldown = false;
		player.boostDmg = boostValue;
		skillDuration = duration;
	}

	private IEnumerator ResetDamage(PlayerController player)
	{
		//count to duration and Reset Multiplier afterwards
		yield return new WaitForSecondsRealtime(duration);
		player.CmdChangeDmgMultiplier(false); // Reset Multiplier
		player.CmdToggleMniMainSkillVar();
	}

	private IEnumerator Cooldown()
	{
		//count to duration and set onCooldown to false
		yield return new WaitForSecondsRealtime(cd);
		onCooldown = false;
	}

}
