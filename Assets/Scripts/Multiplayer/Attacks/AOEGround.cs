using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class represents any field on the ground that applies effects to units inside.
/// It hits the units inside in an intervall (tickRate).
/// IMPORTANT: The duration of any onHitEffect should be equal to the tickRate otherwise
/// the effect will stack multiple times.
/// </summary>
public class AOEGround : Attack
{
	[SerializeField] private float duration;
	[SerializeField] private float tickRate;

	private float durationTimer;
	private float tickTimer;

	private List<Unit> unitsInside;

	private void Start()
	{
		unitsInside = new List<Unit>();
		durationTimer = duration;
	}


	private void Update()
	{
		durationTimer -= Time.deltaTime;
		tickTimer -= Time.deltaTime;

		if (tickTimer <= 0f)
		{
			foreach (Unit unit in unitsInside)
			{
				if (unit==null || unit.isDead())
				{
					unitsInside.Remove(unit);
				}
				onHit(unit);
			}
			tickTimer = tickRate;
		}

		if (durationTimer <= 0)
		{
			Destroy(gameObject);
		}

	}


	/// <summary>
	/// Adds any unit entering to the unitInside list.
	/// </summary>
	/// <param name="other">Any collsion</param>
	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Unit>() != null)
		{
			unitsInside.Add(other.GetComponent<Unit>());
		}
	}

	/// <summary>
	/// Removes any unit entering from the unitInside list.
	/// </summary>
	/// <param name="other">Any collision exit</param>
	private void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<Unit>() != null)
		{
			unitsInside.Remove(other.GetComponent<Unit>());
		}
	}
}
