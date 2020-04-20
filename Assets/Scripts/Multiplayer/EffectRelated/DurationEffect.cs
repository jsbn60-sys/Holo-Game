using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect, which active for a duration and then turns off again.
/// A good example for this is the DamageBoostEffect, which gives a damageBoost for a certain duration
/// and turns off after that.
/// </summary>
public abstract class DurationEffect : Effect
{
	[SerializeField]
	protected float duration;

	private float durationTimer;

	private bool isRunning;

	/// <summary>
	/// Sets target and starts the effect.
	/// </summary>
	/// <param name="target">Unit to apply effect to</param>
	public override void turnOnEffect(Unit target)
	{
		this.target = target;
		isRunning = true;
		execEffect();
	}

	/// <summary>
	/// Needs to be implemented to turn the effect off again.
	/// </summary>
	protected abstract void turnOffEffect();

	// Start is called before the first frame update
	protected void Start()
    {
		durationTimer = duration;
		isRunning = false;
	}

	// Update is called once per frame
	protected void Update()
    {
		if (isRunning)
		{
			durationTimer -= Time.deltaTime;

			if (!isActive())
			{
				turnOffEffect();
				isRunning = false;
			}
		}
	}

	/// <summary>
	/// Checks if timer has run out.
	/// </summary>
	/// <returns>Has timer run out</returns>
	private bool isActive()
	{
		return durationTimer > 0f;
	}
}
