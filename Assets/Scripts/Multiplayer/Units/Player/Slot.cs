using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

/// <summary>
/// This class represents any type of slot.
/// It is used for the quickAccessBars and the inventory.
/// </summary>
/// <typeparam name="T">Object that is slotted, needs to implement Slotable interface</typeparam>
public abstract class Slot<T> : MonoBehaviour where T : Slotable
{
	[SerializeField] private GameObject icon;
	[SerializeField] private GameObject tooltip;
	[SerializeField] private bool isReusable;

	protected T content;
	private bool isEmpty;

	public virtual void Start()
	{
		isEmpty = true;
	}

	/// <summary>
	/// Inserts content into the slot.
	/// </summary>
	/// <param name="content">Content to insert.</param>
	public virtual void insertContent(T content)
	{
		this.content = content;
		icon.GetComponent<Image>().sprite = content.getIcon();
		isEmpty = false;
	}

	/// <summary>
	/// Uses content in the slot.
	/// </summary>
	/// <param name="player">Player that used it.</param>
	public virtual void useContent(Unit player)
	{
		if (!isEmpty)
		{
			content.activate(player);
			if (!isReusable)
			{
				icon.GetComponent<Image>().sprite = null;
				isEmpty = true;
			}
		}
	}

	public bool IsEmpty => isEmpty;


}
