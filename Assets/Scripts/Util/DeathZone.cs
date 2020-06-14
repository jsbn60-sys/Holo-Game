using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Unit unitHit;
		if ((unitHit = other.GetComponent<Unit>()) != null && unitHit.isLocalPlayer)
		{
			unitHit.CmdChangeHealth(-100000);
		}
	}
}
