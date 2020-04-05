/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class activates the attack speed aura
/// </summary>
public class WS_AttackSpeed : Skill
{
	public override void Activate(PlayerController player)
	{
		player.CmdSetAura(1);
	}
}
