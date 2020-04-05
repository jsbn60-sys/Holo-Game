/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportSlowField : MonoBehaviour
{
	//the value by which enemies are getting slowed.(0.9 for 10% slow)
	public float slowFactor;
	public float skillSlowMult = 1.3f;
	public float slowFieldAreaIncrease = 2;
	//the duration of the slowfield being active
	public float duration;
	public float standardSpeed;
	//this variable is to collect enemies in the slowfield for the speedreset
	private List<GameObject> npcs = new List<GameObject>();
	public GameObject playerSupport;

	public bool areaIncreased = false;
	public bool effectIncreased = false;

	/// <summary>
	/// This function starts a coroutine to reset the speed of enemies who never left the slowfield("npc" = enemies in the slowfield)
	/// Slowfield destroys itself after the "duration"+1
	/// </summary>
	void Start()
	{
		StartCoroutine(ResetSpeed(duration));
		Destroy(gameObject, duration + 1);


		if ( effectIncreased == true)
		{
			slowFactor = slowFactor * skillSlowMult;
		}
		if (areaIncreased == true)
		{
			transform.localScale = new Vector3(transform.localScale.x * slowFieldAreaIncrease, transform.localScale.y, transform.localScale.z * slowFieldAreaIncrease);
		}

	}

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
	/// This function slows all enemies who enter the collider and adds them in to the npcs
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
	/// This function is called when the slowfield destroys itself. It resets the speed of the remaining npcs.
	/// </summary>
	private void OnDestroy()
	{
		foreach(GameObject npc in npcs)
		{
			if(npc != null)
			{
				npc.GetComponent<NPC.NPC>().move.agent.speed = standardSpeed;
			}
		}
	}
}
