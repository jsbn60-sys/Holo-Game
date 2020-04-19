using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an item with an effect.
/// Implements the Slotable interface so it can be sloted in the ItemQuickAccess and the Inventory.
/// </summary>
public class Item : MonoBehaviour , Slotable
{
	Effect effect;

	[SerializeField]
	private Sprite sprite;

	public void activate(Player player) {

	}

	public Sprite getIcon()
	{
		return sprite;
	}

	public GameObject getInstance()
	{
		return this.gameObject;
	}

}
