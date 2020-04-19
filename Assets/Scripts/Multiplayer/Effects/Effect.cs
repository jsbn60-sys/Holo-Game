using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class represents any kind of effect that can be applied to a Unit.
/// The actual effect has to be implemented by subclasses via the execEffect function.
///
/// This class is not finished.
/// </summary>
public abstract class Effect : NetworkBehaviour {

	public Effect()
	{
		timeAlive = 0f;
	}

	protected Unit target;

	[SerializeField]
	protected float duration;

	private float timeAlive;

	public abstract void execEffect();

	public void setTarget(Unit target)
	{
		this.target = target;
	}

	public virtual void update()
	{
		timeAlive += Time.deltaTime;
	}

	public bool isActive()
	{
		return timeAlive >= duration;
	}
}
