using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an NPC that pushes its target once its reached.
/// </summary>
public class Pushing_NPC : NPC
{
	/// <summary>
	/// Because pushing is handled over the pushForce and trigger collision, we don't need any additional implementations.
	/// </summary>
	protected override void execInRangeAction() { }
}
