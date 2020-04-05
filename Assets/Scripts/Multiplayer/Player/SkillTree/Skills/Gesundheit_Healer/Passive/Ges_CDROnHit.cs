/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class implements a passive Ability that reduces the Cooldown of active ability ShieldItemDrop on Hit.
/// A variable in the player script that gets checked by the bullet script is set to true for that.
/// The implementation can be found in the Bullet Script.
/// </summary>
public class Ges_CDROnHit : Skill
{
	public override void Activate(PlayerController player)
	{
		player.shieldItemDropCDRonHit = true;
	}
}
