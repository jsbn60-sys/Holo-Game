using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;


/// <summary>
/// This class represents a wave that is finished when all triggers are activated.
/// </summary>
public class TriggerWave : Wave
{
	/// <summary>
	/// Triggers that need to be activated.
	/// </summary>
	[SerializeField] private List<Trigger> triggers;

	/// <summary>
	/// Marks if this trigger wave should remove triggers,
	/// if not enough players are there to activate them.
	/// </summary>
	[SerializeField] private bool dynamicTriggerSize;

	/// <summary>
	/// Activates all triggers on start.
	/// </summary>
	protected override void initWave()
	{
		foreach (Trigger trigger in triggers)
		{
			trigger.gameObject.SetActive(true);
		}
	}

	/// <summary>
	/// Checks if all triggers are active and removes overflow of triggers if needed.
	/// </summary>
	protected override void UpdateActiveCondition()
	{
		if (dynamicTriggerSize && activeTriggerAmount() > PlayerController.Instance.getPlayerCount())
		{
			CmdRemoveTriggers(triggers.Count - PlayerController.Instance.getPlayerCount(), 0f);
		}

		foreach (Trigger trigger in triggers)
		{
			if (trigger.enabled && !trigger.isTriggered())
			{
				return;
			}
		}

		CmdRemoveTriggers(triggers.Count, 1f);

		isActive = false;
	}

	/// <summary>
	/// Returns the amount of triggers that need to be activated.
	/// </summary>
	/// <returns>Unactive triggers</returns>
	protected override string getDoorText()
	{
		int missingTriggers = triggers.Count;

		foreach (Trigger trigger in triggers)
		{
			if (!trigger.enabled || trigger.isTriggered())
			{
				missingTriggers--;
			}
		}

		return missingTriggers.ToString();
	}


	/// <summary>
	/// Returns the amount of triggers that are actually used.
	/// (May be removed if overflow).
	/// </summary>
	/// <returns>Active triggers</returns>
	private int activeTriggerAmount()
	{
		int res = 0;

		foreach (Trigger trigger in triggers)
		{
			if (trigger.enabled)
			{
				res++;
			}
		}

		return res;
	}

	/// <summary>
	/// Removes triggers on all clients.
	/// </summary>
	/// <param name="amount">Amount of triggers to remove</param>
	/// <param name="removeTime">Time until removed</param>
	[Command]
	private void CmdRemoveTriggers(int amount, float removeTime)
	{
		RpcRemoveTriggers(amount, removeTime);
	}

	/// <summary>
	/// Removes triggers on all clients.
	/// </summary>
	/// <param name="amount">Amount of triggers to remove</param>
	/// <param name="removeTime">Time until removed</param>
	[ClientRpc]
	private void RpcRemoveTriggers(int amount, float removeTime)
	{
		StartCoroutine(RemoveTriggers(amount, removeTime));
	}

	/// <summary>
	/// Removes triggers after a given time.
	/// </summary>
	/// <param name="amount">Amount of triggers to remove</param>
	/// <param name="removeTime">Delay before remove</param>
	/// <returns></returns>
	private IEnumerator RemoveTriggers(int amount, float removeTime)
	{
		yield return new WaitForSeconds(removeTime);

		for (int i = 0; i < amount; i++)
		{
			Trigger trigger = triggers[i];
			trigger.enabled = false;
			trigger.gameObject.SetActive(false);
		}
	}
}
