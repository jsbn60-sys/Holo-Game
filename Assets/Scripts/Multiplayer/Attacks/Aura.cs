﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class represents any field on the ground that applies effects to units inside.
/// It hits the units inside in an interval (tickRate).
/// IMPORTANT: The duration of any onHitEffect should be equal to the tickRate otherwise
/// the effect will stack multiple times.
/// </summary>
public class Aura : Attack
{
	[SerializeField] private float duration;
	[SerializeField] private float tickRate;

	[SerializeField] private bool hitsPlayers;
	[SerializeField] private bool hitsEnemies;

	private float durationTimer;
	private float tickTimer;

	protected List<Unit> unitsInside;

	/// <summary>
	/// Start is called before the first frame update.
	/// </summary>
	protected void Start()
	{
		unitsInside = new List<Unit>();
		durationTimer = duration;
	}

	/// <summary>
	/// Update is called once per frame.
	/// Updates timers and hits unitsInside.
	/// </summary>
	protected void Update()
	{
		durationTimer -= Time.deltaTime;
		tickTimer -= Time.deltaTime;

		if (tickTimer <= 0f)
		{
			tickHit();
			tickTimer = tickRate;
		}

		if (durationTimer <= 0)
		{
			Destroy(gameObject);
		}
	}

	protected virtual void tickHit()
	{
		for (int i =  unitsInside.Count -1; i >= 0; i--)
		{
			Unit unit = unitsInside[i];
			if (unit==null || unit.isDead())
			{
				unitsInside.RemoveAt(i);
			}
			else
			{
				onHit(unit);
			}
		}
	}

	/// <summary>
	/// Adds unit entering to the unitInside list,
	/// depending on if the aura hitPlayers or/and hitsEnemies.
	/// </summary>
	/// <param name="other">Any collsion</param>
	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Unit>() != null)
		{
			if (other.tag.Equals("Player") && hitsPlayers ||
			    other.tag.Equals("NPC") && hitsEnemies)
			{
				unitsInside.Add(other.GetComponent<Unit>());
			}
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
