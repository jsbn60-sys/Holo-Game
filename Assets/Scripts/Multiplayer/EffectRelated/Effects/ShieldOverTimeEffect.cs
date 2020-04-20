using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect which gives the unit a certain amount of shield over a duration in ticks.
/// </summary>
public class ShieldOverTimeEffect : TickingEffect
{
	[SerializeField]
	private float shieldAmount;

	protected override void execEffect()
	{
		target.giveShield(shieldAmount / tickAmount);	
	}

	// Start is called before the first frame update
	void Start()
	{

		base.Start();

	}

	// Update is called once per frame
	void Update()
	{
		base.Update();
	}
}
