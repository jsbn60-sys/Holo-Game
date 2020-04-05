/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This Class unlocks a passive Skill that applies a Shield on Hit with a healing Bullet when the targets hitpoints are at max.
/// The actual implementation can be found in the HealingBullet script.
/// </summary>
public class Ges_ShieldOnHit : Skill
{
	
	public override void Activate(PlayerController player)
	{
		player.onHitShield = true;
		player.healingBullet.GetComponent<Multiplayer.HealingBullet>().shieldBuff = true;
	}
}
