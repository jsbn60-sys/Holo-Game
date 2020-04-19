using NPC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNPC : MonoBehaviour
{
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.G))
		{
			NPCManager.Instance.SpawnNewNPCGroup(10);
		}
	}
}
