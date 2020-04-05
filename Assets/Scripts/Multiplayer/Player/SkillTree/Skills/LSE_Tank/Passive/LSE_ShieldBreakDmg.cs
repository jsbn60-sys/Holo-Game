/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class implements the shield break damage skill of the tank/LSE.
/// It sets the boolean variable explosiveShield of the players Health component to true.
/// </summary>
public class LSE_ShieldBreakDmg : Skill
{
	public override void Activate(PlayerController player)
	{
		player.GetComponent<Multiplayer.Health>().explosiveShield = true;
	}
}
