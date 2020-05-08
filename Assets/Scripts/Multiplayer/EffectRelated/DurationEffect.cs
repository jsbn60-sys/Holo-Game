using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect, which active for a duration and then turns off again.
/// A good example for this is the DamageBoostEffect, which gives a damageBoost for a certain duration
/// and turns off after that.
///
/// The abstract method execEffect is in this case to be understood as turning on the effect.
/// </summary>
public abstract class DurationEffect : Effect
{
	[SerializeField]
	protected float duration;

	private float durationTimer;

	private bool isRunning;

	 /// <summary>
	 /// Start is called before the first frame update.
	 /// </summary>
	protected void Start()
    {
	    durationTimer = duration;
	    isRunning = false;
    }

	 /// <summary>
	 /// Is updated every frame.
	 /// execEffect is called once,
	 /// after that it checks if the timer has run off.
	 /// </summary>
	protected override void updateEffect()
	{
		if (!isRunning)
		{
			isRunning = true;
			execEffect();
		}
		else
		{
			durationTimer -= Time.deltaTime;

			if (!isActive())
			{
				turnOffEffect();
				Destroy(gameObject);
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

	/// <summary>
	/// Needs to be implemented to turn the effect off again.
	/// </summary>
	protected abstract void turnOffEffect();
}
