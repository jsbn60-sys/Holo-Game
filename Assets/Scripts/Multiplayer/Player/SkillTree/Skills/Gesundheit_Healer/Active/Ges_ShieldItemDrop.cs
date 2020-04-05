/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class implements an active ability of the Healer class.
/// Drops a shield item that grants x shield on pickup.
/// Its cooldown can be reduced on Hit with normal bullets by unlocking a passive Skill(Ges_CDROnHit).
/// The implementation of the ShieldItem that gets dropped can be found in the ShieldItem script.
/// </summary>
public class Ges_ShieldItemDrop : Skill
{
	public int shieldValue = 60; //shield value

	private int cdi = 0; //cooldown counter for cooldown reduction on hit

	public bool cdrOnHit = false; //activated by another passive skill 
	public int cdrVal = 1; //cooldown reduction value on hit in seconds
	public override void Ability(PlayerController player)
	{
		if (!onCooldown)
		{
			player.cdShieldItemDrop = cooldown;
			//player.CmdStrongHealingBullet(healValue);
			player.CmdDropShieldItem(shieldValue); //actual ability call
			player.StartCoroutine(Cooldown(player));
			onCooldown = true;
			SkillCooldownController.Instance.StartReducableCooldown(0, slot, cooldown, id);
		}
	}

	public override void Activate(PlayerController player)
	{
		onCooldown = false;
		player.shieldItemDrop = true;
		player.shieldItem.GetComponent<ShieldItem>().shieldValue = shieldValue;
	}

	private IEnumerator Cooldown(PlayerController player)
	{
		//count to duration and increase the Number of charges afterwards
		for (cdi = 0; cdi < player.cdShieldItemDrop; cdi++)
		{
			yield return new WaitForSecondsRealtime(1);
		}
		onCooldown = false;
	}
}
