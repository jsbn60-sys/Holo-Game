/* edited by: SWT-P_WS_2018_Holo */
using System;
using System.Collections;
using System.Collections.Generic;
using Multiplayer;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

namespace NPC
{
	[RequireComponent(typeof(NavMeshAgent))]
	/// <summary>
	///  Basic NPC.
	/// </summary>
	public class NPC : NPCController
	{
		public event EventHandler OnNPCDestroyed;
		public NPCGroup npcGroup;
		public Vector3 groupPos;
		private NavMeshAgent navMeshAgent;
		public NPCMove move;
		private NPCChase chase;
		private NPCAttack attack;
		public float range = 2f;
		public float delay = 1f;
		public short type;
		public Sprite sprite;
		public Material trans; // Material when NPC is focused by drone
		public Material normal; // standard material
		private bool isTriggered = false; // if the NPC is focused by drone
		private float time = 0; // how long NPC is focused by drone


		public void SetTime()
		{
			time = 0;
		}
		public float GetTime()
		{
			return time;
		}
		public void OnTriggerEnter(Collider other)
		{

		}

		void OnTriggerExit(Collider other)
		{

		}
		public void SetTriggered(bool trigger)
		{
			isTriggered = trigger;
		}
		public bool GetTrigered()
		{
			return isTriggered;
		}

		/// <summary>
		/// Sets or updates the target
		/// </summary>
		/// <param name="target">The target point to navigate to</param>
		public void SetGroupPos(Vector3 pos)
		{
			this.groupPos = pos;

		}

		/// <summary>
		/// Sets or updates the NPCGroup
		/// </summary>
		/// <param name="group"></param>
		public void SetNPCGroup(NPCGroup group)
		{
			if (npcGroup != null)
			{
				npcGroup.RemoveNPC(this);//Remove NPC from previous group
			}
			npcGroup = group;

		}

		/// <summary>
		/// Returns if the NPC is in a group
		/// </summary>
		public bool IsInGroup()
		{
			if (npcGroup != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Remove the NPC from his group
		/// </summary>
		public void RemoveNPCFromGroup()
		{
			if (npcGroup != null)
			{
				npcGroup.RemoveNPC(this);
				npcGroup = null;
			}
		}

		override protected void Start()
		{
			navMeshAgent = GetComponent<NavMeshAgent>();//Set the navMeshAgent
			if (navMeshAgent == null)
			{
				Debug.LogError("Missing NavMeshAgent!");
			}
			base.Start();
		}
#if UNITY_EDITOR
		/// <summary>
		///  Draws the target in the editor.
		/// </summary>
		protected virtual void OnDrawGizmosSelected()
		{
			UnityEditor.Handles.color = Color.green;
			UnityEditor.Handles.DrawSolidDisc(groupPos, transform.up, 1);
		}
#endif
		void Update()
		{
			navMeshAgent.speed = GetComponent<Unit>().getSpeed();
			time += Time.deltaTime;
			if (!IsInGroup())
			{
				NPCGroup group = NPCManager.Instance.SpawnNewNPCGroup(null, 0, transform.position);
				group.AddNPC(this);
				npcGroup = group;
			}

		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();

			SetGroupPos(npcGroup.GetGroupPositionForNPC(this));
			move.SetTarget(groupPos);

		}
		protected virtual void OnDestroy()
		{
			if (npcGroup != null)
			{
				npcGroup.RemoveNPC(this);
			}
			if (OnNPCDestroyed != null)
			{
				OnNPCDestroyed(this, new EventArgs());
			}
		}
		protected override void InitialiseBehavior()
		{
			move = new NPCMove(navMeshAgent, GetComponent<VisibleTargets>());
			attack = new NPCAttack(range, delay, this);

			if(type == 4){
				// higher distance for bigEnemy
				chase = new NPCChase(4, navMeshAgent, GetComponent<VisibleTargets>());
			}
			else if(type == 6){
				// higher distance for fireEnemy
				chase = new NPCChase(3, navMeshAgent, GetComponent<VisibleTargets>());
			}
			else{
				chase = new NPCChase(1, navMeshAgent, GetComponent<VisibleTargets>());
			}

			move.AddTransition(Transition.SawTarget, chase);
			chase.AddTransition(Transition.LostTarget, move);
			chase.AddTransition(Transition.ReachedTarget, attack);
			attack.AddTransition(Transition.LostTarget, chase);

			AddBehavior(move);
			AddBehavior(chase);
			AddBehavior(attack);

			currentBehavior = move;
		}

		public void againLive() {
			InitialiseBehavior();
		}

		/// <summary>
		/// Stuns or unstuns the NPC.
		/// </summary>
		/// <param name="turnOn">Should NPC be stunned</param>
		public void changeStun(bool turnOn)
		{
			if (turnOn)
			{
				navMeshAgent.isStopped = true;
			} else
			{
				navMeshAgent.isStopped = false;

			}
		}
	}
}
