/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class implements a passive ability that grants the Main Ability an extra effect
/// The extra effect is a short heal over time.
/// The implementation can be found in the HealingBullet script.
/// The ability also increases the attack speed of the player.
/// </summary>
public class Ges_HotOnHit : Skill
{
	public float atkSpdMultiplier = 0.8f; //1*0.8 = 0.8 -> 20% Mehr atkspd
	public override void Ability(PlayerController player)
	{
		base.Ability(player);
	}

	public override void Activate(PlayerController player)
	{
		player.onHitHot = true;
		player.healingBullet.GetComponent<Multiplayer.HealingBullet>().onHitHot = true;
		player.fireRate = player.fireRate * atkSpdMultiplier;
	}
}
