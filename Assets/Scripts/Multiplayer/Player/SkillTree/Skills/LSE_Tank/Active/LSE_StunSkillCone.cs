/* author: SWT-P_SS_2019_Holo */
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class implements the behaviour of the stun cone, which is instatiated on the stun skill of the tank/LSE.
/// npcs is a list of all enemies currently alive.
/// stunFactor is the factor by which the speed of the enemies is multiplied.
/// </summary>

public class LSE_StunSkillCone : MonoBehaviour
{
	private List<GameObject> npcs = new List<GameObject>();
	public float stunFactor = 0.01f;


	private void OnTriggerEnter(Collider other)
	{
		var enemy = other.gameObject;
		if (other.tag == "Enemy")
		{
			if (enemy.GetComponent<NPC.NPC>() != null)
			{
				if (enemy.GetComponent<NPC.NPC>().move != null)
				{
					if (enemy.GetComponent<NPC.NPC>().move.agent.speed >= 0.1)
					{
						enemy.GetComponent<NPC.NPC>().move.agent.speed *= stunFactor;
						npcs.Add(enemy);
					}
				}
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject != null)
		{
			if (other.gameObject.GetComponent<NPC.NPC>() != null)
			{
				if (other.gameObject.GetComponent<NPC.NPC>().move != null)
				{
					if (other.gameObject.GetComponent<NPC.NPC>().move.agent.speed >= 0.1)
					{
						other.gameObject.GetComponent<NPC.NPC>().move.agent.speed /= stunFactor;
					}
				}
			}
		}
	}

	private void OnDestroy()
	{
		foreach (GameObject npc in npcs)
		{
			if (npc != null)
			{
				npc.GetComponent<NPC.NPC>().move.agent.speed /= stunFactor;
			}
		}
	}
}
