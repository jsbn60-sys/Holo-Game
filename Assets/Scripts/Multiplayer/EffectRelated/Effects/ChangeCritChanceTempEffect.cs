using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect which increases a units chance to critical strike for a certain duration.
/// </summary>
public class ChangeCritChanceTempEffect : DurationEffect
{
	[SerializeField] private float critChance;

    protected override void execEffect()
    {
	    target.getAttack().changeCritChance(critChance);

    }

    protected override void turnOffEffect()
    {
	    target.getAttack().changeCritChance(-critChance);

    }
}
