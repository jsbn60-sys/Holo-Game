/* edited by: SWT-P_WS_2018_Holo */
using Multiplayer;
using System.Collections;
using UnityEngine;

///<summary>
/// This class manages the attacks NPCs perform against players.
/// The NPCs choose this behaviour after chasing a player down and being close enough to attack.
/// After a NPC has attacked it waits a certain time until it will attack the player again.
/// The amount of damage a NPC deals to a player is determined by which role the player and NPC represent.
/// Every playertype will receive little damage by NPCs of the same role and there is one NPCrole that will deal lots of damage.
/// Special enemy types are handled in this script. The huge, slow enemy will deal very much damage and the small kamikaze enemies deal a considerable amount of damage, too.
/// Another special enemy is the pyro enemy, which will deal damage over time to the players but no instant damage on the hit.
/// The overall amount of damage can be manipulated by changing the value of parameter mul. It is set to 3 by standard to ensure balanced gameplay.
/// </summary>

namespace NPC
{
	public class NPCAttack : NPCBehavior
	{
		private int bigenemydmg = 25;
		private int kamikazedmg = 10;
		private int bigdmg = 4;
		private int avgdmg = 2;
		private int lildmg = 1;
		private int mul = 3;
		private int[,] dmgTable = new int[5, 6];
		private float maxDistance;
		private float attackDelay;
		private bool canAttack;
		public MonoBehaviour monoInstance;

		///<summary>
		/// In this function the damagetable is initialised as a 2 dimensional array.
		/// The first index resembles the players role as an int value. The second index is the NPCs role as an int value.
		/// <param name="maxDistance"> Distance between NPC and player in which NPCs are able to deal damage to players</param>
		/// <param name="attackDelay"> Amount of time NPCs take to regain their ability to attack </param>
		///</summary>
		public NPCAttack(float maxDistance, float attackDelay, MonoBehaviour monoInstance)
		{
			this.maxDistance = maxDistance;
			this.attackDelay = attackDelay;
			this.monoInstance = monoInstance;
			canAttack = true;
			dmgTable[0, 0] = avgdmg;
			dmgTable[0, 1] = avgdmg;
			dmgTable[0, 2] = avgdmg;
			dmgTable[0, 3] = avgdmg;
			dmgTable[0, 4] = bigenemydmg;
			dmgTable[0, 5] = kamikazedmg;
			dmgTable[1, 0] = lildmg; // mni-mni
			dmgTable[1, 1] = bigdmg;
			dmgTable[1, 2] = avgdmg;
			dmgTable[1, 3] = avgdmg;
			dmgTable[1, 4] = bigenemydmg;
			dmgTable[1, 5] = kamikazedmg;
			dmgTable[2, 0] = avgdmg;
			dmgTable[2, 1] = lildmg; // lse-lse
			dmgTable[2, 2] = bigdmg;
			dmgTable[2, 3] = avgdmg;
			dmgTable[2, 4] = bigenemydmg;
			dmgTable[2, 5] = kamikazedmg;
			dmgTable[3, 0] = avgdmg;
			dmgTable[3, 1] = avgdmg;
			dmgTable[3, 2] = lildmg; // wir-wir
			dmgTable[3, 3] = bigdmg;
			dmgTable[3, 4] = bigenemydmg;
			dmgTable[3, 5] = kamikazedmg;
			dmgTable[4, 0] = bigdmg;
			dmgTable[4, 1] = avgdmg;
			dmgTable[4, 2] = avgdmg;
			dmgTable[4, 3] = lildmg; // ges-ges
			dmgTable[4, 4] = bigenemydmg;
			dmgTable[4, 5] = kamikazedmg;
		}

		///<summary>
		/// In the Act method NPCs deal damage to players after calculating the amount of damage.
		/// To calculate the damage this script multiplies the entry in the damagetable according to player and NPC role with the overall multiplier
		/// After dealing damage, the NPC will be unable to attack and a coroutine to reset this ability
		///</summary>
		public override void Act(Transform npc, Transform target)
		{
			if (canAttack)
			{
				Health health = target.GetComponent<Health>();
				if (!target.GetComponent<PlayerController>().isInvincible)
				{
					if (!health.IsZero())
					{
						if (npc.transform.GetComponent<NPC>().type == 5)
						{
							// kamikaze enemies
							health.TakeDamage(dmgTable[target.GetComponent<PlayerController>().role, npc.GetComponent<NPC>().type] * mul);
							npc.GetComponent<Health>().TakeDamage(1000);
						} else if (npc.transform.GetComponent<NPC>().type == 6)
						{
							// fire enemies
							health.Ignite(); // ignite the player to deal DOT
							canAttack = false;
							monoInstance.StartCoroutine(ResetAttack());
						} else
						{
							health.TakeDamage(dmgTable[target.GetComponent<PlayerController>().role, npc.GetComponent<NPC>().type] * mul);
							GameObject.FindGameObjectWithTag("MainCamera").transform.Rotate(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0));
							target.transform.GetComponent<AudioManager>().PlaySound(target.transform.position, 12);
							canAttack = false;
							monoInstance.StartCoroutine(ResetAttack());

						}
					} else
					{
						NPCManager.Instance.RemoveTarget(target.transform);
						GameOverManager.Instance.RemoveProf(target.GetComponent<PlayerController>());
						GameObject[] npcs = GameObject.FindGameObjectsWithTag("Enemy");
						foreach (GameObject go in npcs)
						{
							go.GetComponent<NPC>().againLive();
						}
					}
				}
			}
		}

		///<summary>
		/// This method determines wether the player is still within maximum attacking distance after attacking
		///</summary>
		public override void Reason(Transform npc, Transform target, NPCController controller)
		{
			if (Vector3.Distance(npc.position, target.position) > maxDistance)
			{
				controller.SetTransition(Transition.LostTarget);
			}

		}

		private IEnumerator ResetAttack()
		{
			yield return new WaitForSeconds(attackDelay);
			canAttack = true;
		}
	}
}
