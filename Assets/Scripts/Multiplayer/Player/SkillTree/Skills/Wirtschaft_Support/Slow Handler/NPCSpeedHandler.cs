/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using UnityEngine;

public class NPCSpeedHandler : MonoBehaviour
{
	public static NPCSpeedHandler instance;


	public void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	/// <summary>
	/// This methods resets the NPC speed to standard speed
	/// </summary>
	/// <param name="enemy">Enemy object whos speed will be reset</param>
	/// <returns></returns>
	public static IEnumerator ResetSpeed(GameObject enemy)
	{
		yield return new WaitForSecondsRealtime(1f);

		if (enemy != null)
		{
			if (enemy.GetComponent<NPC.NPC>() != null)
			{
				if (enemy.GetComponent<NPC.NPC>().move != null)
				{
					if (enemy.GetComponent<NPC.NPC>().move.agent.speed != null)
					{
						enemy.GetComponent<NPC.NPC>().move.agent.speed = 7; // 7 == standard speed
					}
				}
			}
		}
	}
}
