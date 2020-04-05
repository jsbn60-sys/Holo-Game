/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Multiplayer;

namespace Multiplayer
{
	///<summary>
	///	This class implements the slowbullet. It spawns a slowfield if it hits a collider.
	///</summary>
	public class SupportBulletHandlerArea : NetworkBehaviour
	{
		[SyncVar]
		public GameObject slowfieldAreaIncreased;
		private GameObject field;

		private void Start()
		{
			ClientScene.RegisterPrefab(slowfieldAreaIncreased);
		}

		/// <summary>
		/// If bullet hits the ground, slow field will be spawned
		/// </summary>
		private void OnTriggerEnter(Collider other)
		{
			if (other.tag != "Player")
			{
				Vector3 ground = transform.position;
				ground.y = -1;
				field = Instantiate(slowfieldAreaIncreased, ground, Quaternion.Euler(0, 0, 0));
				CmdFieldSpawn();
				Destroy(gameObject);
			}
		}

		[Command]
		private void CmdFieldSpawn()
		{
			NetworkServer.Spawn(field);
		}
	}
}

