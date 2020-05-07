using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a generic QuickAccessBar
/// in which content can be added into slots.
/// This is used for ItemQuickAccess and SkillQuickAccess.
/// </summary>
/// <typeparam name="S">Slot to be used</typeparam>
/// <typeparam name="C">Content in the slot</typeparam>
public class QuickAccess<S,C> : MonoBehaviour where S : Slot<C> where C : Slotable
{
	[SerializeField]
	private S[] slots;

	/// <summary>
	/// Adds content into slot
	/// </summary>
	/// <param name="content"></param>
	public void addContent(C content)
	{
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].IsEmpty)
			{
				slots[i].insertContent(content);
				return;
			}
		}
	}

	public void addContent(C content, int idx)
	{
		if (idx > slots.Length)
		{
			throw new System.ArgumentException("Slot Idx too high!");
		}

		slots[idx].insertContent(content);
	}

	/// <summary>
	/// Uses content.
	/// </summary>
	/// <param name="idx">index of slot</param>
	/// <param name="player">player that used it</param>
	public void useContent(int idx, Player player)
	{
		if (!slots[idx].IsEmpty)
		{
			slots[idx].useContent(player);
		}
	}

	/// <summary>
	/// Checks if slots are full.
	/// </summary>
	/// <returns>Are slots full</returns>
	public bool isFull()
	{
		foreach (S slot in slots)
		{
			if (slot.IsEmpty)
			{
				return false;
			}
		}
		return true;
	}
}
