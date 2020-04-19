/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Multiplayer;

namespace Multiplayer
{
		///<summary>
		///	This class implements the  slowbullet. It spawns a slowfield if it hits a collider.
		///</summary>
    public class Glue : NetworkBehaviour
    {
		[SyncVar]
		public GameObject slowfield;
		private GameObject field;

		private void Start()
		{
			ClientScene.RegisterPrefab(slowfield);
		}

		private void OnTriggerEnter(Collider other)
        {
			if (other.tag != "Player")
			{
				Vector3 ground = transform.position;
				ground.y = -1;
				field = Instantiate(slowfield, ground, Quaternion.Euler(0, 0, 0));
				CmdFieldSpawn();
				Destroy(gameObject);
			}
		}


		[Command]
		private void CmdFieldSpawn() {
			NetworkServer.Spawn(field);
		}
	}
}
	
