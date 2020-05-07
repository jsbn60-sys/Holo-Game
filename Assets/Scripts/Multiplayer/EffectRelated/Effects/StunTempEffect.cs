using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect which stuns the target temporarily.
/// IMPORTANT: THE TARGET CAN ONLY BE AN NPC.
/// </summary>
public class StunTempEffect : DurationEffect
{
	protected override void execEffect()
	{
		target.GetComponent<NPC.NPC>().changeStun(true);
	}

	protected override void turnOffEffect()
	{
		target.GetComponent<NPC.NPC>().changeStun(false);
	}
}
