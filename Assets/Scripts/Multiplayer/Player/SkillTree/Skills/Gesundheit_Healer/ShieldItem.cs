/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class represents the dropped shield of the healer skill Shield drop.
/// It activates on pickup and has a special effect unlockable by a passive skill of the healer(Ges_ShieldHot).
/// </summary>
public class ShieldItem : MonoBehaviour
{
	public int shieldValue = 50;
	public int heal_per_tick = 4;
	public int hot_tick_amount = 4;
	public float time_between_healticks = 5;
	public int shield_per_tick = 5;
	public int shield_tick_amount = 4;
	public float time_between_shieldticks = 4;
	public bool applyHots = false;
	void Start()
	{
		gameObject.GetComponent<Collider>().enabled = false;
		StartCoroutine(ActivateItemCollider());
	}
	void Update()
	{
		// prevent objects from falling through map
		if (gameObject.transform.position.y < 0)
		{
			Rigidbody rb = GetComponent<Rigidbody>();
			rb.useGravity = false;
			gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
			rb.constraints = RigidbodyConstraints.FreezePosition;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		var hit = other.gameObject;
		if (hit.tag == "Player")
		{
			if (hit.GetComponent<Multiplayer.Health>() != null)
			{
				var health = hit.GetComponent<Multiplayer.Health>();
				health.CmdShield(shieldValue);
				if (applyHots)
				{
					health.StartCoroutine(health.HealOverTime(heal_per_tick, hot_tick_amount, time_between_healticks));
					health.StartCoroutine(health.ShieldOverTime(shield_per_tick, shield_tick_amount, time_between_shieldticks));
				}
				Destroy(gameObject);
//				health.Shield()
			}
		}
		else if (hit.tag == "Enemy")
		{ //Wenn ein Gegner das Schilditem berührt, wird es zerstört

			Destroy(gameObject);
		}
	}

	private IEnumerator ActivateItemCollider()
	{
		yield return new WaitForSecondsRealtime(1);
		gameObject.GetComponent<Collider>().enabled = true;
	}

}
