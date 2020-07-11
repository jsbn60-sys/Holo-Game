using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a wave that is active for a certain time.
/// </summary>
public class TimeWave : Wave
{
	/// <summary>
	/// Duration of the wave
	/// </summary>
	[SerializeField] private float waveDuration;

	/// <summary>
	/// Rest time of the wave
	/// </summary>
	private float waveTimer;

	/// <summary>
	/// Starts the timer.
	/// </summary>
	protected override void initWave()
	{
		waveTimer = waveDuration;
	}

	/// <summary>
	/// Checks and updates timer.
	/// </summary>
	protected override void UpdateActiveCondition()
	{
		if (waveTimer > 0f)
		{
			waveTimer -= Time.deltaTime;

			if (waveTimer <= 0f)
			{
				isActive = false;
			}
		}
	}

	/// <summary>
	/// Returns timer as integer.
	/// </summary>
	/// <returns>Timer</returns>
	protected override string getDoorText()
	{
		return ((int)waveTimer).ToString();
	}
}
