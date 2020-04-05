/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportBullet : MonoBehaviour
{
	public string bulletType;
	Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	/// <summary>
	/// Depending on the bulletType, the correct item will be spawned
	/// </summary>
	private void Update()
	{

		if (gameObject.transform.position.y <= -1)
		{
			if (bulletType == "damageBoostBullet")
			{
				StartCoroutine(ItemDrop.instance.spawnDamageBoost(gameObject.transform.position.x, 0, gameObject.transform.position.z));
			}

			if(bulletType == "randomItemBullet")
			{
				StartCoroutine(ItemDrop.instance.spawnItem(gameObject.transform.position.x, 0, gameObject.transform.position.z, true));
			}
		
			Destroy(gameObject);
		}
	}
}
