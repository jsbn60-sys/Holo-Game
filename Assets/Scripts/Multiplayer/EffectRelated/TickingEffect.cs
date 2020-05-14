using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect, which is active for a duration and is ticking at a certain rate.
/// A good example for this is the ShieldOverTimeEffect, which gives shield over a certain duration
/// at a certain tickRate.
///
/// The abstract method execEffect is in this case to be understood as one tick of the effect.
/// </summary>
public abstract class TickingEffect : Effect
{
	[SerializeField] protected float duration;

	[SerializeField] protected int tickAmount; // IMPORTANT: tickAmount shouldnt be more than 2x duration or coroutine will be off!

	protected float tickRate;

	private float tickTimer;

	private float durationTimer;

	private bool isRunning;

	/// <summary>
	/// Start is called before the first frame update.
	/// </summary>
	protected void Start()
	{
		tickRate = duration / tickAmount;
		durationTimer = duration;
		isRunning = false;
		tickTimer = tickRate;
	}

	/// <summary>
	/// Is called once per frame.
	/// Starts the effectRunner once
	/// and checks if it is active after that.
	/// </summary>
	protected override void updateEffect()
	{
		if (!isRunning)
		{
			isRunning = true;
		}
		else
		{
			durationTimer -= Time.deltaTime;

			if (tickTimer > 0f)
			{
				tickTimer -= Time.deltaTime;
			}
			else
			{
				execEffect();
				tickTimer = tickRate;
			}

			if (!isActive())
			{
				Destroy(gameObject);
			}
		}
	}



	/// <summary>
	/// Checks if timer ran out yet.
	/// </summary>
	/// <returns>Has timer run out</returns>
	private bool isActive()
	{
		return durationTimer > 0f;
	}
}
