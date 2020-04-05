/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the behavior of the warpzone.
/// Slow Enemies, DMG & Speed Buff for allies.
/// </summary>
public class WarpZoneBehav : MonoBehaviour
{
	//the value enemies getting slowed.(0.9 for 10% slow)
	public float slowFactor;
	//the duration of the slowfield being active
	public float duration = 5;
	public float standardSpeed;
	public float standardDmg = 10;
	public int dmgIncrease = 2;
	public ParticleSystem particleSystem;

	//this variable is to collect enemies in the slowfield for the speedreset
	private List<GameObject> npcs = new List<GameObject>();
	private List<GameObject> players = new List<GameObject>();

	public float maxSpeed = 16;
	public float speedBoostFactor = 2; //Speed will increase and decrease by this factor

	/// <summary>
	/// This function starts a coroutine to reset the speed of enemies who never left the slowfield("npc" = enemies in the slowfield)
	/// Slowfield destroys itself after the "duration"+1
	/// </summary>
	void Start()
	{
		StartCoroutine(ResetSpeed(duration));
		Destroy(gameObject, duration + 1);
		Destroy(particleSystem, duration);
	}

	/// <summary>
	/// This method resets the speed after certain time
	/// </summary>
	/// <param name="resetTime">Time after which the speed will be reset.</param>
	private IEnumerator ResetSpeed(float resetTime)
	{
		yield return new WaitForSecondsRealtime(resetTime);
		for (int i = 0; i < npcs.Count; i++)
		{
			if (npcs[i] == null)
			{
				continue;
			}
			if (npcs[i].GetComponent<NPC.NPC>().move != null)
			{
				npcs[i].GetComponent<NPC.NPC>().move.agent.speed = standardSpeed;
			}
		}
	}

	/// <summary>
	/// This method resets the dmg after certain time
	/// </summary>
	/// <param name="resetTime">Time after which the damage will be reset.</param>
	private IEnumerator ResetDmg(float resetTime)
	{
		yield return new WaitForSecondsRealtime(resetTime);
		for (int i = 0; i < players.Count; i++)
		{
			if (players[i].GetComponent<PlayerController>().hasWarpZoneBuff == true)
			{
				players[i].GetComponent<PlayerController>().baseDmg -= dmgIncrease;

				players[i].GetComponent<PlayerController>().speed /= speedBoostFactor;

			}
		}
	}

	/// <summary>
	/// This function slows all enemies wo enter the collider and adds them in to the npcs list
	/// </summary>
	private void OnTriggerEnter(Collider other)
	{
		// Debuff Enemy ------------------------------
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
		//----------------------------------------------
		// Buff Allies
		var player = other.gameObject;
		if (other.tag == "Player")
		{
			if (player.GetComponent<PlayerController>().hasWarpZoneBuff == false)
			{
				player.GetComponent<PlayerController>().baseDmg += dmgIncrease;

				if (player.GetComponent<PlayerController>().speed * speedBoostFactor <= maxSpeed)
				{
					player.GetComponent<PlayerController>().speed *= speedBoostFactor;
				}
				player.GetComponent<PlayerController>().hasWarpZoneBuff = true;
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

		var player = other.gameObject;
		if (other.tag == "Player")
		{
			if (player.GetComponent<PlayerController>().hasWarpZoneBuff == true)
			{
				player.GetComponent<PlayerController>().baseDmg -= dmgIncrease;
				player.GetComponent<PlayerController>().speed /= speedBoostFactor;
				player.GetComponent<PlayerController>().hasWarpZoneBuff = false;
			}
		}
	}
}
