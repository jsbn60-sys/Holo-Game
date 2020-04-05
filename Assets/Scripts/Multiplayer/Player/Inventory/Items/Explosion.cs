/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace Multiplayer
{
	///<summary>
	/// This class implements an explosion that deals AoE Damage.
	///</summary>
	public class Explosion : NetworkBehaviour
	{
		public LayerMask enemy = 11;
		public float maxDamage = 100f;
		public float explosionForce = 100f;
		public float maxLifeTime = 12f;
		public float explosionRadius = 5f; //The maximum distance away from the explosion are still affected.
		public GameObject confetti;
		private GameObject confClone;

		private void Start()
		{
			ClientScene.RegisterPrefab(confetti);
		}

		///<summary>
		/// When the explosion collides with a gameobject, confetti is instantiated.
		/// If the collider is an NPC, AoE damage is delt.
		///</summary>
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag != "Slowfield"){
				confClone = Instantiate(confetti, transform.position, Quaternion.Euler(-90, 0, 0));
				Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, enemy);

				for (int i = 0; i < colliders.Length; i++)
				{
					var target = colliders[i].gameObject;
					if (target.GetComponent<Health>() && target.GetComponent<NPC.NPC>() != null)
					{
						var health = target.GetComponent<Health>();
						Vector3 pos = target.transform.position;
						health.TakeDamage((int)(CalculateDamage(pos)));
					}
				}
				CmdCreateConfetti();
				Destroy(gameObject);
			}
		}

		private float CalculateDamage(Vector3 targetPosition)
		{
			Vector3 explosionToTarget = targetPosition - transform.position;
			float explosionDistance = explosionToTarget.magnitude;
			float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;
			float damage = relativeDistance * maxDamage;
			damage = Mathf.Max(0f, damage);
			return damage;
		}

		[Command]
		private void CmdCreateConfetti() {
			NetworkServer.Spawn(confClone);
		}

	}
}
