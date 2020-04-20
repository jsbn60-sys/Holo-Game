using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class represents any kind of effect that can be applied to a Unit.
/// The actual effect has to be implemented by subclasses via the execEffect function.
/// The turnOnEffect function is used by Attack, Skill, Item.
/// </summary>
public abstract class Effect : NetworkBehaviour
{
	protected Unit target;

	protected abstract void execEffect();

	public abstract void turnOnEffect(Unit target);
}
