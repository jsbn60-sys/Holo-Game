/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// This class implements the PierceShot Passive Skill of a MNI Player.
/// This Skill turns all Bullet of a Player into Piercing Bullets, that will
/// pierce thorugh enemys.
/// Changes the number of Pierces using a variable in the PlayerController
///</summary>
public class MNI_PierceShot : Skill
{
	public override void Activate(PlayerController player)
	{
		player.numberPierces += 1;
	}
}
