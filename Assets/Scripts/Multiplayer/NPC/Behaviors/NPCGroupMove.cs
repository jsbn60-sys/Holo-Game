using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
	public class NPCGroupMove : NPCBehavior
	{
		private NavMeshAgent navMeshAgent;

		public NPCGroupMove(NavMeshAgent navMeshAgent)
		{
			this.navMeshAgent = navMeshAgent;
		}
		public override void Act(Transform npc, Transform target)
		{
			if (target == null) return;
			navMeshAgent.SetDestination(target.position);
		}

		public override void Reason(Transform npc, Transform target, NPCController controller)
		{

		}

	}
}
