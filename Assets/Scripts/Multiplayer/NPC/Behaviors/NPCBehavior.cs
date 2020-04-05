using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
	public abstract class NPCBehavior : INPCBehavior
	{
		protected Dictionary<Transition, INPCBehavior> transitionMap = new Dictionary<Transition, INPCBehavior>();

		public INPCBehavior GetOutputState(Transition transition)
		{
			if (transitionMap.ContainsKey(transition))
			{
				return transitionMap[transition];
			}
			else
			{
				Debug.LogWarning("transitionMap doesn't contain key!");
				return null;
			}
		}

		public void AddTransition(Transition transition, INPCBehavior behavior)
		{
			if (transitionMap.ContainsKey(transition))
			{
				Debug.LogWarning("transitionMap allready contains a transion!");
				return;
			}
			transitionMap.Add(transition, behavior);
		}

		public abstract void Reason(Transform npc, Transform target, NPCController controller);

		public abstract void Act(Transform npc, Transform target);
	}
}
