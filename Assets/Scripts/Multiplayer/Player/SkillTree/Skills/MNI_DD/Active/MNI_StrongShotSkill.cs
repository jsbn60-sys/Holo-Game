/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// This class implements the StrongShot Active Skill of a MNI Player.
/// When this active Skill is activated, the Player will shoot one of x charges
/// of a Strong Bullet that deals high dmg. Each charge regenerates separately
/// Calls the CmdStrongShot Method from the PlayerController which fires a Bullet with
/// the Dmg passed to it
///</summary>
public class MNI_StrongShotSkill : Skill
{
	public int charges;
	private int restCharges;
	public int bullet_dmg;
	private int dmg;

	public override void Ability(PlayerController player)
	{
		if (restCharges > 0)
		{
			player.CmdStrongShot(dmg);
			restCharges -= 1;
			player.StartCoroutine(Cooldown());
		}
		SkillCooldownController.Instance.strongShotOverlay(slot, restCharges);
	}

	public override void Activate(PlayerController player)
	{
		dmg = bullet_dmg;
		restCharges = charges;
		SkillCooldownController.Instance.strongShotOverlay(slot, restCharges);
	}

	private IEnumerator Cooldown()
	{
		//count to duration and increase the Number of charges afterwards
		yield return new WaitForSecondsRealtime(cooldown);
		restCharges += 1;
		SkillCooldownController.Instance.strongShotOverlay(slot, restCharges);
	}
}
