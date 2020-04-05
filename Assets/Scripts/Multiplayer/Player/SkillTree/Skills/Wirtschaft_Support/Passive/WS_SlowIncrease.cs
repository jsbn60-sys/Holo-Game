/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class activates Slowfield effect increase
/// </summary>
public class WS_SlowIncrease : Skill
{
	public override void Activate(PlayerController player)
	{
		player.isSlowEffectIncreased = true;
	}

}
