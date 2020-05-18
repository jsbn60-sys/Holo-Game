using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class represents any kind of effect that can be applied to a Unit.
/// The actual effect has to be implemented by subclasses via the execEffect function.
/// The effect will only update after wasActivated is set true.
/// How the effect should be updated has to be implemented by subclasses via the updateEffect function.
///
/// An effect should always be a separate gameObject,
/// because the method attachEffect, will attach a copy of the gameObject to the target
/// which will be later destroyed.
/// Multiple effects should not be attached to one gameObject, because they may end at different times
/// and the effects would also be applied multiple times.
/// </summary>
public abstract class Effect : NetworkBehaviour
{
	protected Unit target;

	protected bool wasActivated;

	/// <summary>
	/// The actual execution of the effect.
	/// What this means in the context of the overall effect
	/// varies.
	/// Look for additional explanation in the subclasses.
	/// </summary>
	protected abstract void execEffect();

	/// <summary>
	/// How the effect will update once activated.
	/// </summary>
	protected abstract void updateEffect();


	/// <summary>
	/// This method attaches effects to a unit
	/// by making a copy of a given effect.
	/// </summary>
	/// <param name="effect">Effect to copy and attach</param>
	/// <param name="target">Unit to give effect</param>
	public static void attachEffect(GameObject effect, Unit target)
	{
		Effect copiedEffect = Instantiate(effect, target.transform).GetComponent<Effect>();
		copiedEffect.target = target;
		copiedEffect.wasActivated = true;
	}


	private bool setupTarget;
	/// <summary>
	/// Update is called once per frame.
	/// Calls updateEffect after the effect was activated.
	/// </summary>
	public void Update()
	{
		if (wasActivated)
		{
			updateEffect();
		}
	}

}
