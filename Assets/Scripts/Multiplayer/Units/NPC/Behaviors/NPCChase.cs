using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
	class NPCChase : NPCBehavior
	{
		private float minDistance;
		private NavMeshAgent agent;
		private VisibleTargets visibleTragets;

		public NPCChase(float minDistance, NavMeshAgent agent, VisibleTargets visibleTragets)
		{
			this.minDistance = minDistance;
			this.agent = agent;
			this.visibleTragets = visibleTragets;
		}

		public override void Act(Transform npc, Transform target)
		{
			if (visibleTragets.ContainsTarget(target))
			{
				agent.SetDestination(target.position);
			}
		}

		public override void Reason(Transform npc, Transform target, NPCController controller)
		{
			if (agent.remainingDistance <= minDistance)
			{
				controller.SetTransition(Transition.ReachedTarget);
			}
			if (!visibleTragets.ContainsTarget(target))
			{
				controller.SetTransition(Transition.LostTarget);
			}
		}
	}
}
