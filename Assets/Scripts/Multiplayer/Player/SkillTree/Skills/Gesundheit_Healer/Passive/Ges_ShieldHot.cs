/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class implements a passive ability that grants the Shield an extra effect
/// The extra effect is a heal over time and shield over time effect.
/// It regenerates shield and hitpoints. The implementation can be found in the ShieldItem.cs
/// </summary>
public class Ges_ShieldHot: Skill
{

	public override void Activate(PlayerController player)
	{
		player.applyHotsOnShieldItemPickup = true;
		player.shieldItem.GetComponent<ShieldItem>().applyHots = true;

	}
}
