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

	private bool hasBeenPickedUp;

	private void Start()
	{
		hasBeenPickedUp = false;
	}

	public void activate(Player player) {
		effect.turnOnEffect(player);
	}

	public Sprite getIcon()
	{
		return spriteRenderer.sprite;
	}

	public GameObject getInstance()
	{
		return this.gameObject;
	}

	public void pickUp()
	{
		spriteRenderer.enabled = false;
		hasBeenPickedUp = true;
	}

	public bool wasPickedUp()
	{
		return hasBeenPickedUp;
	}

}
