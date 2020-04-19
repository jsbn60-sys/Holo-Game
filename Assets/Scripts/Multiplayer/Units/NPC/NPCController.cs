using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

namespace NPC
{
	/// <summary>
	/// NPCController handles the transition and execution for behaviors.
	/// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
	public abstract class NPCController : NetworkBehaviour
	{
		protected List<INPCBehavior> behaviors = new List<INPCBehavior>();
		protected INPCBehavior currentBehavior;
		[SerializeField]
		protected Transform target;

		public virtual void SetTransition(Transition transition)
		{
			currentBehavior = currentBehavior.GetOutputState(transition);
		}

		protected virtual void Start()
		{
			InitialiseBehavior();
		}

		public void SetTarget(Transform target)
		{
			this.target = target;
		}

		protected abstract void InitialiseBehavior();

		protected virtual void FixedUpdate()
		{
			if (currentBehavior != null)
			{
				currentBehavior.Reason(transform, target, this);
				currentBehavior.Act(transform, target);
			}
		}

		protected void AddBehavior(INPCBehavior behavior)
		{
			behaviors.Add(behavior);
		}


	

	}
}
