using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect which stuns the target temporarily.
/// </summary>
public class StunTempEffect : DurationEffect
{
	protected override void execEffect(){
		target.changeStunned(true);
	}

	protected override void turnOffEffect()
	{
		target.changeStunned(false);
	}
}
