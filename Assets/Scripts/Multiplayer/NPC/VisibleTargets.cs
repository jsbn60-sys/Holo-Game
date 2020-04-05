using System.Collections;
using System.Collections.Generic;
using NPC;
using UnityEngine;

public class VisibleTargets : MonoBehaviour {
	public float fovUpdateRate = 0.5f;

	public float viewRadius;
	[Range(0, 360)]
	public float viewAngle;

	public LayerMask targetMask;
	public LayerMask obstacleMask;

	public List<Transform> visibleTargets = new List<Transform>();

	public Transform closestTarget;
	public float distanceToTarget;
	void Start()
	{
		StartCoroutine("FindTargetsWithDelay", fovUpdateRate);
	}
	/// <summary>
	/// Finds targets with a delay
	/// </summary>
	/// <param name="delay">The time for the next search</param>
	private IEnumerator FindTargetsWithDelay(float delay)
	{
		while (true)
		{
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
		}
	}
	/// <summary>
	/// Find visible targets
	/// </summary>
	void FindVisibleTargets()
	{
		visibleTargets.Clear();
		closestTarget = null;
		distanceToTarget = 0;
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++)
		{
			Transform target = targetsInViewRadius[i].transform;

			if (!NPCManager.Instance.ContainsTarget(target))
			{
				// if this target in view radius is not a valid target -> skip it
				continue;
			}

			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
			{
				float dstToTarget = Vector3.Distance(transform.position, target.position);

				if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
				{
					if (closestTarget == null || dstToTarget < distanceToTarget)
					{
						closestTarget = target;
						distanceToTarget = dstToTarget;
					}
					visibleTargets.Add(target);
				}
			}
		}
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	public bool ContainsTarget(Transform target) 
	{
		return visibleTargets.Contains(target);
	}
}
