using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class changes the units health.
/// This can be damage or healing.
/// </summary>
public class ChangeHealthPermEffect : PermanentEffect
{
	[SerializeField] private float healthChangeAmount;
	protected override void execEffect()
    {
	    target.changeHealth(healthChangeAmount);
    }
}
