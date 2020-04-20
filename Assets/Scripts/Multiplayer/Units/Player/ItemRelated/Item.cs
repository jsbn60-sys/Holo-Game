using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an item with an effect.
/// Implements the Slotable interface so it can be sloted in the ItemQuickAccess and the Inventory.
/// </summary>
public class Item : MonoBehaviour , Slotable
{
	[SerializeField]
	private	Effect effect;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	public void activate(Player player) {
		effect.startEffect(player);
	
	}

	public Sprite getIcon()
	{
		return spriteRenderer.sprite;
	}

	public GameObject getInstance()
	{
		return this.gameObject;
	}

}
