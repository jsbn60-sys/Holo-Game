using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a trigger, that can be activated by collision.
/// </summary>
public class Trigger : MonoBehaviour
{
	/// <summary>
	/// Mesh that will change color of if active.
	/// </summary>
	[SerializeField] private MeshRenderer meshRenderer;

	/// <summary>
	/// Inactive color of trigger.
	/// </summary>
	[SerializeField] private Material inactiveColor;

	/// <summary>
	/// Active color of trigger.
	/// </summary>
	[SerializeField] private Material activatedColor;

	/// <summary>
	/// Tag of objects that will activate this trigger.
	/// </summary>
	[SerializeField] private string activatingTag;

	/// <summary>
	/// Will this trigger stay active, even if all colliders leave
	/// </summary>
	[SerializeField] private bool isPermanent;

	/// <summary>
	/// Amount of colliders inside the trigger.
	/// </summary>
	private int triggersInside;

	/// <summary>
	/// Start is called before the first frame update.
	/// </summary>
	private void Start()
	{
		triggersInside = 0;
		meshRenderer.material = inactiveColor;
		this.gameObject.SetActive(false);
	}

	/// <summary>
	/// Checks if the collider has the correct tag
	/// and activates trigger if.
	/// </summary>
	/// <param name="other">Any trigger that entered</param>
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals(activatingTag))
		{
			triggersInside++;
			meshRenderer.material = activatedColor;
		}
	}

	/// <summary>
	/// Checks if the collider has the correct tag
	/// and if no colliders are left inside, deactivates the trigger.
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerExit(Collider other)
	{
		if (isPermanent) return;
		if (other.tag.Equals(activatingTag))
		{
			triggersInside--;

			if (triggersInside <= 0)
			{
				meshRenderer.material = inactiveColor;
			}
		}
	}
	/// <summary>
	/// Returns if this trigger is active.
	/// </summary>
	/// <returns>Is this trigger active</returns>

	public bool isTriggered()
	{
		return triggersInside > 0;
	}
}
