using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class represents any kind of effect that can be applied to a Unit.
/// The actual effect has to be implemented by subclasses via the execEffect function.
///
/// An effect should always be a seperate gameObject and the script should always be disabled,
/// because the method attachEffect, will attach a copy of the gameObject to the target
/// and then enabled it.
/// </summary>
public abstract class Effect : MonoBehaviour
{
	protected Unit target;

	/// <summary>
	/// The actual execution of the effect.
	/// What this means in the context of the overall effect
	/// varies, so there is additional explanation in the subclasses.
	/// </summary>
	protected abstract void execEffect();

	/// <summary>
	/// This method is attaches effects to a unit.
	/// It is used by the Item, Skill and Attack class.
	/// </summary>
	/// <param name="effect">Effect to copy and attach</param>
	/// <param name="target">Unit to give effect</param>
	public static void attachEffect(GameObject effect, Unit target)
	{
		Effect copiedEffect = Instantiate(effect, target.transform).GetComponent<Effect>();
		copiedEffect.target = target;
		copiedEffect.enabled = true;
	}

}
