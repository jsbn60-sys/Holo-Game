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
	[SerializeField] private float explosionRadius;

	protected override bool isActive()
    {
	    return target.Shield > 0;
    }

    protected override void turnOffEffect()
    {
		target.explode(explosionRadius,explosionForce,explosionLayer);
    }

    protected override void execEffect() { }
}
