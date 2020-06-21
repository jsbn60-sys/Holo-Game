using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an NPC that shoots its target with a beam.
/// The laser is activated if the npc is in range and has line of sight to the target.
/// </summary>
public class Beam_NPC : NPC
{
	[Header("Throwing_NPC : Attributes")]
	[SerializeField] private LineRenderer laserPointer;
	[SerializeField] private LayerMask hittableLayer;

	/// <summary>
	/// Attacks the target if it has line of sight.
	/// </summary>
	protected override void execCanAttackActions()
	{
		if (hasLineOfSight())
		{
			useAttack();
		}
	}

	/// <summary>
	/// Deactivates the laser if the target is not in range.
	/// </summary>
	protected override void execTargetNotInRangeActions()
	{
		laserPointer.gameObject.SetActive(false);
	}

	/// <summary>
	/// Activates the laser if the target is in range and has line of sight.
	/// If it doesn't have line of sight, the laser is deactivated.
	/// </summary>
	protected override void execInRangeActions()
	{
		if (hasLineOfSight())
		{
			laserPointer.gameObject.SetActive(true);
			updateLaserPointer();
		}
		else
		{
			laserPointer.gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Checks if the npc can see its target.
	/// </summary>
	/// <returns>Can the npc see the target</returns>
	private bool hasLineOfSight()
	{

		RaycastHit hit;

		if(Physics.Linecast(transform.position,currentTarget.transform.position,out hit,hittableLayer))
		{
			return hit.collider.tag.Equals("Player");
		}

		return false;
	}

	/// <summary>
	/// Updates positions of laserPointer.
	/// </summary>
	private void updateLaserPointer()
	{
		laserPointer.SetPosition(0,transform.position);
		laserPointer.SetPosition(1,currentTarget.transform.position);
	}
}
