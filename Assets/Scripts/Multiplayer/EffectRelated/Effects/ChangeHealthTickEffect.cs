using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

/// <summary>
/// This class represents an effect which changes a units health over time in ticks.
/// This can be damage or healing.
/// </summary>
public class ChangeHealthTickEffect : TickingEffect
{
	[SerializeField] private float changeAmount;
	protected override void execEffect()
    {
		target.changeHealth(changeAmount/tickAmount);
    }
}
