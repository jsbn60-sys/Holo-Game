using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

/// <summary>
/// This class represents an aura, which is attached to a target.
/// </summary>
public class AttachableAura : Aura
{
	private Transform target;

	public Transform Target
	{
		set => target = value;
	}

	/// <summary>
	/// Update is called once per frame.
	/// Updates the aura position to the targets position.
	/// </summary>
	protected void Update()
	{
		this.transform.position = target.position;
		base.Update();
	}
}
