/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class activates cooldown reduction skill
/// </summary>
public class WS_Cooldown : Skill
{
	public override void Activate(PlayerController player)
	{
		player.isCRSkilled = true;
	}
}
