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
		NPC.NPCManager.Instance.stunAllNPCs(true);
	}

	protected override void turnOffEffect()
	{
		NPC.NPCManager.Instance.stunAllNPCs(false);
	}

	// Start is called before the first frame update
	void Start()
    {
		base.Start();
    }

    // Update is called once per frame
    void Update()
    {
		base.Update();
    }
}
