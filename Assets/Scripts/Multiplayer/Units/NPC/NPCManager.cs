using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

namespace NPC
{
	public class NPCManager : MonoBehaviour
	{
		public event EventHandler OnAllNPCsDied;

		public static NPCManager Instance;

		[SerializeField]
		private GameObject		npcGroupPrefab;
		[SerializeField]
		private GameObject		npcPrefab;
		[SerializeField]
		private List<Transform>	targets			= new List<Transform>();
		[SerializeField]
		private List<Transform> spawnPoints		= new List<Transform>();
		[SerializeField]
		private int				npcCount		= 0;

		private List<NPCGroup>	groups			= new List<NPCGroup>();
		private float time;
		
		void OnEnable()
		{
			Instance = this;
		}

		public void AddTarget(Transform target)
		{
			//if targets doesn't contain target
			if (!targets.Contains(target))
			{
				//add the target
				targets.Add(target);
			}

		}

	

		/// <summary>
		/// Removes a target 
		/// </summary>
		/// <param name="target"></param>
		public void RemoveTarget(Transform target)
		{
			//if targets doesn't contains target
			if (!targets.Contains(target))
			{
				//that should not be the case
			//	Debug.LogWarning("Tried to remove a target that has already been removed or has not been added.");
			}
			else
			{
				//remove the target
				targets.Remove(target);

				//update the target of every group
				foreach (var group in groups)
				{
					group.FetchNewTarget();
				}
			}
		}

		public bool ContainsTarget(Transform target)
		{
			return targets.Contains(target);
		}

		/// <summary>
		/// Return a random traget.
		/// </summary>
		/// <returns>The Traget</returns>
		public Transform GetRandomTarget()
		{
			//if targets count equals zero
			if (targets.Count != 0)
			{
				// return random target
				int index = UnityEngine.Random.Range(0, targets.Count - 1);
				return targets[index];
			}
			else 
			{
				//that should not be the case
			//	Debug.LogWarning("Can't get a target because targets are Empty!");
				return null;
			}
		}

		public void AddSpawnPoint(Transform spawnPoint)
		{
			//if spawnPoints doesn't contain the new spawnPoint
			if (!spawnPoints.Contains(spawnPoint))
			{
				//add it
				spawnPoints.Add(spawnPoint);
			}
			else
			{
				//that should not be the case
				Debug.LogWarning("Tried to add a spawnPoint that has already been added.");
			}
		}

		public void RemoveSpawnPoint(Transform spawnPoint)
		{
			//if spawnPoints doesn't contain the new spawnPoint
			if (spawnPoints.Contains(spawnPoint))
			{
				//remove it
				spawnPoints.Remove(spawnPoint);
			}
			else
			{
				//that should not be the case
				Debug.LogWarning("Tried to remove a spawnPoint that has already been removed or has not been added.");
			}
		}

		public void RemoveGroup(NPCGroup group)
		{
			groups.Remove(group);
		}

		/// <summary>
		/// Spawn a new group of NPC's
		/// </summary>
		/// <param name="count">
		/// the number of NPC's </param>
		public NPCGroup SpawnNewNPCGroup(int count)
		{
			//if spawnPoints count isn't equals zero
			if (spawnPoints.Count != 0)
			{
				//spawn a new NPC group
				int index = UnityEngine.Random.Range(0, spawnPoints.Count);
				return SpawnNewNPCGroup(npcPrefab, count, spawnPoints[index].localPosition);
			}
			else 
			{
				//that should not be the case
				Debug.LogError("Can't get a spawnpoint because spawnpoints are Empty!");
				return null;
			}
		}

		/// <summary>
		/// Spawn a new group of NPC's
		/// </summary>
		/// <param name="count">
		/// the number of NPC's </param>
		public void SpawnNewNPCGroup(GameObject prefab,int count)
		{
			//if spawnPoints count isn't equals zero
			if (spawnPoints.Count != 0)
			{
				//spawn a new NPC group
				int index = UnityEngine.Random.Range(0, spawnPoints.Count);
				SpawnNewNPCGroup(prefab, count, spawnPoints[index].localPosition);
			}
			else
			{
				//that should not be the case
				Debug.LogError("Can't get a spawnpoint because spawnpoints are Empty!");
				return;
			}
		}

		/// <summary>
		/// Spawn a new group of NPC's in a given position
		/// </summary>
		/// <param name="count">
		/// the number of NPC's </param>
		/// <param name="pos">
		/// the spawn position for the NPC's
		/// </param>
		public NPCGroup SpawnNewNPCGroup(GameObject prefab,int count, Vector3 pos)
		{
			GameObject gm = Instantiate(npcGroupPrefab, pos, Quaternion.identity);
			NPCGroup npcGroup = gm.GetComponent<NPCGroup>();
			NavMeshAgent groupNavMeshAgent = gm.GetComponent<NavMeshAgent>();

			groups.Add(npcGroup);

			//You have to warp the player because the navMeshAGent will restet the position.
			groupNavMeshAgent.Warp(pos);
			
			for (int i = 0; i < count; i++)
			{
				GameObject npcGO = Instantiate(prefab, pos, Quaternion.identity);
				NPC npc = npcGO.GetComponent<NPC>();
				npc.OnNPCDestroyed += Npc_NPCDestroyed;
				NavMeshAgent navMeshAgent = npcGO.GetComponent<NavMeshAgent>();
				switch (prefab.name)
				{
					case "EnemyMNI": npc.type = 0; break;
					case "EnemyLSE": npc.type = 1; break;
					case "EnemyWirtschaft": npc.type = 2; break;
					case "EnemyGesundheit": npc.type = 3; break;
				}
				
				//You have to warp the player because the navMeshAgent will restet the position.
				npcGroup.AddNPC(npc);
				navMeshAgent.Warp(pos);
				if(NetworkServer.active) {
					NetworkServer.Spawn(npcGO);
				}
				
				npcCount++;
			}
			return npcGroup;
		}

		private void Npc_NPCDestroyed(object sender, System.EventArgs e)
		{
			npcCount--;
			if(npcCount == 0 && OnAllNPCsDied != null) 
			{
				OnAllNPCsDied(this, new EventArgs());
			}
			if(npcCount < 0) 
			{
				Debug.LogError("NPCCount is less then zero.This should not happen!");
			}
		}

		/// <summary>
		/// Is used by the globalStunEffect.
		/// </summary>
		/// <param name="turnOn">Should all NPCs be stunned</param>
		public void stunAllNPCs(bool turnOn)
		{
			foreach(NPCGroup group in groups)
			{
				foreach(NPC npc in group.npcList)
				{
					npc.changeStun(turnOn);
				}
			}
		}
	}


	}
