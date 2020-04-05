/* author: SWT-P_SS_2019_Holo */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

///<summary>
///This script handles the slowfield. It decreases the movement speed of the enemies who enter the collider.
///If they exit the collider the movement speed is set to normal.
///</summary>
public class SlowfieldEffectIncrease : Multiplayer.Glue
{
	//the value enemies getting slowed by.(0.9 for 10% slow)
	public float slowFactor = 0.1f;
	//the duration of the slowfield being active
	public float duration = 15;
	public float standardSpeed = 7;
	//this variable is to collect enemies in the slowfield for the speedreset
	private List<GameObject> npcs = new List<GameObject>();

	/// <summary>
	/// This function starts a coroutine to reset the speed of enemies who never left the slowfield("npc" = enemies in the slowfield)
	/// Slowfield destroys himself after the "duration"+1
	/// </summary>
	void Start()
	{
		StartCoroutine(ResetSpeed(duration));
		Destroy(gameObject, duration + 1);
	}

	/// <summary>
	/// Resets enemy speed
	/// </summary>
	/// <param name="resetTime">Time after which the speed will be reset</param>
	private IEnumerator ResetSpeed(float resetTime)
	{
		yield return new WaitForSecondsRealtime(resetTime);
		for (int i = 0; i < npcs.Count; i++)
		{
			if (npcs[i] != null)
			{
				npcs[i].GetComponent<NPC.NPC>().move.agent.speed = standardSpeed;
			}
		}
	}

	/// <summary>
	/// This function slows all enemies who enter the collider and adds them to the npcs list
	/// </summary>
	private void OnTriggerEnter(Collider other)
	{
		var enemy = other.gameObject;
		if (other.tag == "Enemy")
		{
			if (enemy.GetComponent<NPC.NPC>() != null)
			{
				if (enemy.GetComponent<NPC.NPC>().move != null)
				{
					if (enemy.GetComponent<NPC.NPC>().move.agent.speed > 0.3f)
					{
						enemy.GetComponent<NPC.NPC>().move.agent.speed *= slowFactor;
						npcs.Add(enemy);
					}
				}
			}
		}
	}
	
	/// <summary>
	/// This function resets the speed of the enemies after they exit the slow field
	/// </summary>
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Enemy")
		{
			var enemy = other.gameObject;
			if (enemy.GetComponent<NPC.NPC>() != null)
			{
				if (enemy.GetComponent<NPC.NPC>().move != null)
				{
					enemy.GetComponent<NPC.NPC>().move.agent.speed = standardSpeed;
				}
			}
		}
	}
	/// <summary>
	/// Resets the speed of enemies when the field is destroyed
	/// </summary>
	private void OnDestroy()
	{
		foreach (GameObject npc in npcs)
		{
			if (npc != null)
			{
				npc.GetComponent<NPC.NPC>().move.agent.speed = standardSpeed;
			}
		}
	}
}
