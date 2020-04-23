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
	[SerializeField]
	protected float duration;

	[SerializeField]
	protected int tickAmount;	// IMPORTANT: tickAmount shouldnt be more than 2x duration or coroutine will be off!

	protected float tickRate;

	private IEnumerator effectRunner;

	private float durationTimer;

	private bool isRunning;

	// Start is called before the first frame update
	protected void Start()
    {
		tickRate = duration / tickAmount;
		effectRunner = runEffect();
		durationTimer = duration;
		StartCoroutine(effectRunner);
	}

    // Update is called once per frame
    protected void Update()
    {
		if (isRunning)
		{
			durationTimer -= Time.deltaTime;

			if(!isActive())
			{
				StopCoroutine(effectRunner);
				isRunning = false;
				Destroy(gameObject);
			}
		}
	}

    /// <summary>
	/// Runs the effect and waits for tickRate
	/// </summary>
	/// <returns></returns>
	private IEnumerator runEffect()
	{
		while (isActive())
		{
			execEffect();
			yield return new WaitForSeconds(tickRate);
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
