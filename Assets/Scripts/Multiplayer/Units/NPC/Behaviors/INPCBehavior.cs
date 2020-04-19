using UnityEngine;
namespace NPC
{
	public interface INPCBehavior
	{
		/// <summary>
		/// Returns an INPCBehavior mapped to a transition.
		/// </summary>
		INPCBehavior GetOutputState(Transition transition);

		void AddTransition(Transition transition, INPCBehavior behavior);

		/// <summary>
		/// Decide if the behavior should transition to another one.
		/// </summary>
        void Reason(Transform npc, Transform target, NPCController controller);

		/// <summary>
		/// Main behavior logic.
		/// </summary>
        void Act(Transform npc, Transform target);
	}
}
