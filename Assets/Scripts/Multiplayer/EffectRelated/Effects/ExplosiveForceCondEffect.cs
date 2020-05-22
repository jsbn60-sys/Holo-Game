using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect that causes an explosion
/// on the target, when it loses its shield.
/// </summary>
public class ExplosiveForceCondEffect : ConditionEffect
{

	[SerializeField] private LayerMask explosionLayer;
	[SerializeField] private float explosionForce;

    protected override bool isActive()
    {
	    return target.Shield > 0;
    }

    protected override void turnOffEffect()
    {
		target.explode(explosionForce,explosionLayer);
    }

    // Update is called once per frame
    protected override void execEffect() { }
}
