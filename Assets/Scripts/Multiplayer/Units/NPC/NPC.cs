using System.Collections;
using System.Collections.Generic;
using Leap;
using Multiplayer.Lobby;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

/// <summary>
/// This class represents an non-player character
/// that chases and attacks the players.
/// NPCs are only handled by the server.
/// NPCs are spawned in Groups for more information look at NPCGroups class.
/// </summary>
public abstract class NPC : Unit
{
	[Header("NPC : Attributes")]
	[SerializeField] private NavMeshAgent agent;
	[SerializeField] private LayerMask attackableLayer;
	[SerializeField] private float attackRange;
	[SerializeField] private float detectionRadius;
	protected Transform currentTarget;

	private NPCGroup group;

	public NPCGroup Group
	{
		set => group = value;
	}

	/// <summary>
	/// Update is called once per frame.
	/// Updates the closest target and checks if the npc can attack.
	/// If this runs on the server, it also checks if the NPC position is accurate on the clients.
	/// </summary>
	protected void Update()
    {
	    base.Update();

	    if (canAttack())
	    {
		    stopAgent();
		    execCanAttackActions();
	    }
	    else
	    {
		    agent.isStopped = false;
		    updateCurrTarget();
	    }

	    if (isInRange())
	    {
		    RotateTowards();
		    execInRangeActions();
	    }
	    else
	    {
			execTargetNotInRangeActions();
	    }

	    if (isServer)
	    {
		    RpcCheckPosition(this.transform.position, this.transform.rotation);
	    }
    }

	/// <summary>
	/// Decides what to do, when the NPC has reached its target.
	/// </summary>
	protected abstract void execCanAttackActions();

	/// <summary>
	/// Executes any action that the npc should do, while the target is not in range yet.
	/// </summary>
	protected abstract void execTargetNotInRangeActions();

	/// <summary>
	/// Updates the NPC position on all clients.
	/// </summary>
	/// <param name="actualPos">The accurate position of the NPC</param>
	/// <param name="actualRotation">The accurate rotation of the NPC</param>
	[ClientRpc]
	private void RpcCheckPosition(Vector3 actualPos, Quaternion actualRotation)
	{
		if (!isServer)
		{
			checkNetworkPosition(actualPos,actualRotation);
		}
	}

	/// <summary>
	/// Drops an item on death, removes npc from group and destroys it.
	/// </summary>
	protected override void onDeath()
	{
		if (isServer)
		{
			group.removeNpc(this);
			NetworkServer.Destroy(gameObject);
		}
	}

	/// <summary>
	/// Returns if the NPC can attack.
	/// </summary>
	/// <returns>Can this NPC attack</returns>
	private bool canAttack()
	{
		return readyToAttack() && hasValidTarget() && isInRange() && !agent.pathPending;
	}

	/// <summary>
	/// Checks if the the NPC is in range and has an active path.
	/// </summary>
	/// <returns>Is the NPC in range</returns>
	protected bool isInRange()
	{
		return currentTarget!=null && Vector3.Distance(currentTarget.transform.position,this.transform.position) <= attackRange;
	}


	/// <summary>
	/// Stops the navmeshagent and stops movement.
	/// </summary>
	private void stopAgent()
	{
		agent.isStopped = true;
		GetComponent<Rigidbody>().velocity = Vector3.zero;
	}

	/// <summary>
	/// A NPC attacks by hitting the current target.
	/// </summary>
	protected override void execAttack()
	{
		attack.onHit(currentTarget.GetComponent<Unit>());
	}

	/// <summary>
	/// Updates the current target,
	/// by looking at all targets that are in range.
	/// If there is at least one target in range,
	/// that is a dummy, it is prioritised over all
	/// non-dummy targets. Other dummy targets in range are still
	/// compared so the closest target dummy is found.
	/// Targets that are invisible are skipped.
	/// If there was no target found the npc stops working.
	/// </summary>
	private void updateCurrTarget()
	{
		Collider[] targetsInRange = Physics.OverlapSphere(this.transform.position, detectionRadius, attackableLayer);

		bool isClosestTargetDummy = false;
		bool isCurrentTargetDummy;
		float shortestDist = Mathf.Infinity;
		Transform closestTarget = null;

		foreach (Collider targetInRange in targetsInRange)
		{
			if (targetInRange.GetComponent<Unit>().IsInvisible)
			{
				continue;
			}

			isCurrentTargetDummy = targetInRange.tag.Equals("Dummy");
			Transform target = targetInRange.transform;
			float dist = Vector3.Distance(this.transform.position, target.position);

			if (dist < shortestDist && (!isClosestTargetDummy || isCurrentTargetDummy))
			{
				if (isCurrentTargetDummy)
				{
					isClosestTargetDummy = true;
				}
				shortestDist = dist;
				closestTarget = target;
			}
		}

		if (closestTarget != null)
		{

			currentTarget = closestTarget;
			agent.isStopped = false;
			agent.speed = speed;
			agent.SetDestination(currentTarget.position);
		}
		else
		{
			stopAgent();
		}
	}


	/// <summary>
	/// Checks if the current target is valid.
	/// It has to be a player or dummy,
	/// visible, not dead and not null.
	/// </summary>
	/// <returns>Is the target valid</returns>
	private bool hasValidTarget()
	{
		return currentTarget != null &&
		       (currentTarget.tag.Equals("Player") || currentTarget.tag.Equals("Dummy")) &&
		       !currentTarget.GetComponent<Unit>().IsInvisible &&
		       !currentTarget.GetComponent<Unit>().isDead();
	}

	/// <summary>
	/// NPCs can only push players.
	/// </summary>
	/// <param name="target">Target that collided</param>
	/// <returns>If the target was a player</returns>
	protected override bool canPushTarget(Unit target)
	{
		return target.tag.Equals("Player");
	}

	/// <summary>
	/// Nothing happens to NPCs when they are hit.
	/// </summary>
	protected override void hitEffects() { }

	/// <summary>
	/// Executes the actions that the npc should perform, when he is in range of the target.
	/// </summary>
	protected abstract void execInRangeActions();

	/// <summary>
	/// Fixes rotation towards the target, even if navmeshagent is already stopped.
	/// </summary>
	private void RotateTowards()
	{
		Vector3 direction = (currentTarget.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
	}

	/// <summary>
	/// NPCs can only be hit on the server.
	/// </summary>
	/// <returns>Is the localPlayer the server</returns>
	protected override bool canBeHit()
	{
		return LobbyManager.Instance.LocalPlayerObject.GetComponent<NetworkBehaviour>().isServer;
	}
}
