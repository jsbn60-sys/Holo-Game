/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class implements a passive ability that grants the Main Ability(Healing Bullets) an extra effect
/// The extra effect is a short movement speed boost.
/// The implementation can be found in the HealingBullet script.
/// </summary>
public class Ges_SpeedBoost : Skill
{
	
	public override void Activate(PlayerController player)
	{
		player.onHitSpeedBuff = true;
		player.healingBullet.GetComponent<Multiplayer.HealingBullet>().speedBuff = true;
	}
}
