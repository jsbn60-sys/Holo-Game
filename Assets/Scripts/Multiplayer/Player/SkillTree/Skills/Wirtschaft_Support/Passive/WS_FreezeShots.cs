/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class activates freeze shots
/// </summary>
public class WS_FreezeShots : Skill
{
	public override void Activate(PlayerController player)
	{
		player.freezeShot = true;
	}
}
