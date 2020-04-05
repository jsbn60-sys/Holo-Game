/* edited by: SWT-P_SS_2019_Holo */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
///<summary>
///This script handles the slowfield. It decreases the movement speed of the enemies who enter the collider.
///When they exit the collider the movement speed is set to standard speed.
///</summary>
public class Slowfield : Multiplayer.Glue
{
	//the value by which enemies are getting slowed (0.9 for 10% slow)
	public float slowFactor;
	//the duration of the slowfield being active
	public float duration;
	public float standardSpeed;
	//this variable is to collect enemies in the slowfield for the speedreset
	private List<GameObject> npcs = new List<GameObject>();

	//this function starts a coroutine to reset the speed of enemies who never left the slowfield("npc" = enemies in the slowfield)
	//Slowfield destroys himself after the "duration"+1
	void Start()
	{
		StartCoroutine(ResetSpeed(duration));
		Destroy(gameObject,duration+1);
	}

	private IEnumerator ResetSpeed(float resetTime)
	{
		yield return new WaitForSecondsRealtime(resetTime);
		for (int i = 0; i < npcs.Count; i++) {
			if (npcs[i] == null)
				continue;
			if (npcs[i].GetComponent<NPC.NPC>().move != null)
			{
				npcs[i].GetComponent<NPC.NPC>().move.agent.speed = standardSpeed;
			}
		}
	}

	//this function slows all enemies who enter the collider and adds them in to the "npcs"
	private void OnTriggerEnter(Collider other)
	{
		var enemy = other.gameObject;
		if (other.tag == "Enemy")
		{
			if (enemy.GetComponent<NPC.NPC>() != null)
			{
				if (enemy.GetComponent<NPC.NPC>().move != null) {
	
					if (enemy.GetComponent<NPC.NPC>().move.agent.speed > 0.3f)
					{
						enemy.GetComponent<NPC.NPC>().move.agent.speed *= slowFactor;
						npcs.Add(enemy);
					}

				}
		
			}
		}
	}

	//this function resets the speed of the enemies after they exit the slow field
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Enemy") { 
		
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

	//this function is called when the slowfield destroys itself. It resets the speed of the remaining npcs.
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
