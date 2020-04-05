using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace NPC
{
	public enum GroupState
	{
		Idle,
		Move,
		Attack
	}
	/// <summary>
	///  NPCGroup manages a group of NPC's.
	/// </summary>
	[RequireComponent(typeof(NavMeshAgent))]
	public class NPCGroup : NPCController
	{
		NavMeshAgent navMeshAgent;
		public List<NPC> npcList = new List<NPC>();
		public Dictionary<NPC, Vector3> npcGroupPositions = new Dictionary<NPC, Vector3>();
		public float groupRadius = 20;
	
		public List<string> attackableTags = new List<string>();
		public bool enableMerging = true;

		public LayerMask layer;

		override protected void Start()
		{
			navMeshAgent = GetComponent<NavMeshAgent>();
			navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
			target = NPCManager.Instance.GetRandomTarget();
			base.Start();
		}
		
		void OnTriggerEnter(Collider other)
		{
			//Add npc to group, if npc doesn't have one.
			NPC npc = other.GetComponent<NPC>();
			if (npc != null)
			{
				if (npc.IsInGroup()) return;
				AddNPC(npc);
				return;
			}
			//Merge groups.
			NPCGroup group = other.GetComponent<NPCGroup>();
			if (group != null)
			{
				MergeGroup(group);
				return;
			}

			if (attackableTags.Contains(other.tag))
			{
				target = other.transform;
			}
		}

		override protected void FixedUpdate()
		{
			if(target == null) {
				target = NPCManager.Instance.GetRandomTarget();
			}
			base.FixedUpdate();
		}

		/// <summary>
		/// Adds a new NPC
		/// </summary>
		/// <param name="newNPC">The new NPC to add</param>
		public void AddNPC(NPC newNPC)
		{
			if (!npcList.Contains(newNPC))
			{
				npcList.Add(newNPC);

				npcGroupPositions.Add(newNPC, GetNewPosInGroup());
				newNPC.SetNPCGroup(this);
			}

		}

		/// <summary>
		/// Forces Group to fetch a new Target from NPCManager
		/// </summary>
		/// <returns></returns>
		public void FetchNewTarget()
 		{
 			target = NPCManager.Instance.GetRandomTarget();
 		}

		/// <summary>
		/// Returns a position inside the group radius
		/// </summary>
		/// <returns></returns>
		Vector3 GetNewPosInGroup()
		{
			Vector3 pos = Random.insideUnitCircle * groupRadius;
			pos.z = pos.y;
			pos.y = 0;
			return pos;
		}

		/// <summary>
		///  Get the target for a NPC in the group.
		/// </summary>
		/// <param name="npc">
		/// The NPC we need the target from</param>
		public virtual Vector3 GetGroupPositionForNPC(NPC npc)
		{
			//if npcGroupPosiitons contains the npc
			if (npcGroupPositions.ContainsKey(npc))
			{
				//Transform relativ position
				Vector3 pos = transform.TransformPoint(npcGroupPositions[npc]);
				RaycastHit rayHit;

				if (Physics.Linecast(transform.position, pos, out rayHit, layer))
				{
					return rayHit.point;
				}
				else
				{
					return pos;
				}
			}
			else
			{
				return Vector3.zero;
			}
		}

		public void RemoveNPC(NPC npc)
		{
			//if the NPC list contains npc
			if (npcList.Contains(npc))
			{
				//remove it
				npcList.Remove(npc);
				npcGroupPositions.Remove(npc);

				if (npcList.Count == 0)
				{
					NPCManager.Instance.RemoveGroup(this);
					Destroy(this.gameObject);
				}
			}
		}

		/// <summary>
		///  Merge the Group with an other group.
		/// </summary>
		/// <param name="group">
		/// The NPC group we are merging with</param>
		void MergeGroup(NPCGroup group)
		{
			if (enableMerging == false) return;
			if ((target == null && group.target != null) || (target == null && group.target == null)) return;
			if (target != null && group.target)
			{
				if ((target.position - transform.position).magnitude >
				(group.target.position - group.transform.position).magnitude) return;
			}
			group.enableMerging = false;
			NPC[] npcs = new NPC[group.npcList.Count];
			group.npcList.CopyTo(npcs);
			for (int i = 0; i < npcs.Length; i++)
			{
				AddNPC(npcs[i]);
			}

		}

		protected override void InitialiseBehavior()
		{
			NPCGroupMove move = new NPCGroupMove(navMeshAgent);

			currentBehavior = move;
		}

#if UNITY_EDITOR
		/// <summary>
		///  Draw the fixed positions for the NPC's in the editor.
		/// </summary>
		private void OnDrawGizmosSelected()
		{
			UnityEditor.Handles.color = Color.green;
			UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, groupRadius);
			//draw a circle for each NPC.
			foreach (Vector3 item in npcGroupPositions.Values)
			{
				UnityEditor.Handles.color = Color.blue;
				UnityEditor.Handles.DrawSolidDisc(item + transform.position, transform.up, 0.1f);
			}
		}

		
#endif
	}
}
