using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an NPC that simply uses its attack on its target once it is in range.
/// </summary>
public class Punching_NPC : NPC
{
	/// <summary>
	/// Use attack once in range.
	/// </summary>
	protected override void execInRangeAction()
    {
	    useAttack();
    }
}
