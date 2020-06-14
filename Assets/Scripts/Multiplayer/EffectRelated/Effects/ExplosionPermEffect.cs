using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect that causes an explosion on the target.
/// </summary>
public class ExplosionPermEffect : PermanentEffect
{
	[SerializeField] private LayerMask explosionLayer;
	[SerializeField] private float explosionForce;
	[SerializeField] private float explosionRadius;
	protected override void execEffect()
	{
		target.explode(explosionRadius,explosionForce,explosionLayer);
	}
}
