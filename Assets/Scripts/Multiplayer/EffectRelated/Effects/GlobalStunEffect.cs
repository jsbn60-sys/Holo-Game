using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a global stun effect on all NPCs.
/// The target of this effect is irrelevant.
/// </summary>
public class GlobalStunEffect : DurationEffect
{
	protected override void execEffect()
	{
		NPCController.Instance.changeAllStunned(true);
	}

	protected override void turnOffEffect()
	{
		NPCController.Instance.changeAllStunned(false);

	}

}
