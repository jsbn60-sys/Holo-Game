	/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class activates slowfield area increase
/// </summary>
public class WS_SlowAreaIncrease : Skill
{
	public override void Activate(PlayerController player)
	{
		player.isSlowAreaIncreased = true;
		
	}
}
