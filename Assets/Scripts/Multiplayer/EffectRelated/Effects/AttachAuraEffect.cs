using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect that attaches an aura to the target.
/// It is not a DurationEffect, because the aura has its own duration.
/// </summary>
public class AttachAuraEffect : PermanentEffect
{
	[SerializeField] private AttachableAura auraPrefab;
	private AttachableAura copiedAura;

    protected override void execEffect()
    {
	    copiedAura = Instantiate(auraPrefab, target.transform.position, Quaternion.identity);
	    copiedAura.Target = target.transform;
    }
}
