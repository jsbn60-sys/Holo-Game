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

	protected Unit target;

	private bool wasStarted;

	[SerializeField]
	protected float duration;

	[SerializeField]
	protected float tickRate;

	private float durationTimer;

	public abstract void execEffect();

	private IEnumerator effectRunner;

	protected void Start()
	{
		effectRunner = startEffect();
		durationTimer = duration;
		wasStarted = false;
	}

	public void startEffect(Unit target)
	{
		this.target = target;
		wasStarted = true;

		if (duration == 0f)
		{
			execEffect();
		}
		else
		{
			StartCoroutine(effectRunner);
		}
	}

	protected void Update()
	{
		if (wasStarted)
		{
			durationTimer -= Time.deltaTime;

			if (!isActive())
			{
				StopCoroutine(effectRunner);
				wasStarted = false;
			}
		}
	}

	public bool isActive()
	{
		return durationTimer > 0;
	}

	private IEnumerator startEffect()
	{
		while (isActive())
		{
			execEffect();
			yield return new WaitForSeconds(tickRate);
		}
	}
}
