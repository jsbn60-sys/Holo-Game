/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class activates the dmg buff aura
/// </summary>
public class WS_DmgBuffIncrease : Skill
{
	public override void Activate(PlayerController player)
	{
		player.CmdSetAura(0);
	}
}
