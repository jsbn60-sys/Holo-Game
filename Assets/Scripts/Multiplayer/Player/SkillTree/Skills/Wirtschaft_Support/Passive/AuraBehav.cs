/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class handles the behavior of the support aura
/// </summary>
public class AuraBehav : MonoBehaviour
{
	public GameObject playerSupport;

	private int count = 0;
	private float timeStep = 2f;
	private float timer;
	private int radius = 15;

	private Collider[] colliders;
	

	private void Start()
	{
		// First timer is set here
		timer = Time.time + timeStep;
	}

	private void Update()
	{
		transform.position = new Vector3(playerSupport.transform.position.x, transform.position.y, playerSupport.transform.position.z);

		// If time is bigger than timer function gets triggered
		if(Time.time > timer)
		{
			colliders = Physics.OverlapSphere(gameObject.transform.position, radius);

			// iterate through overlapped objects
			for (int i = 0; i < colliders.Length; i++)
			{
				if(colliders[i] == null)
				{
					continue;
				}

				var target = colliders[i].gameObject;
				
				if (target.tag == "Player")
				{
					playerSupport.GetComponent<PlayerController>().CmdAuraBoost(gameObject, playerSupport, target);
				}
			}
			timer = Time.time + timeStep;
		}
	}
}
