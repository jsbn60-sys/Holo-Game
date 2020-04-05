/* author: SWT-P_WS_2018_Holo */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class implements the quick access bar of the player.
/// The quick access bar has five slots for adding items.
/// </summary>
public class QuickAccessBar : MonoBehaviour {
	public const int SLOTS = 5;
	private Item[] items = new Item[SLOTS];
	public GameObject itemImagePrefab;  

	///<summary>
	/// Adds an item to the quick access bar
	/// </summary>
	///<param name="item">the item to add</param>
    public void AddItem(Item item)
    {
        for (int i = 0; i < SLOTS; i++)
        {
			// get next empty slot
			Transform slot = gameObject.transform.GetChild(i);
			if (slot.childCount < 2)
            {
					// instantiate ItemImage in the Inventory
					GameObject itemImage = Instantiate(itemImagePrefab);
					itemImage.transform.SetParent(slot, false);
					itemImage.GetComponent<InventoryItem>().item = item;
					items[i] = item;
					// get the right sprite to represent the item in the inventory
					slot.GetChild(1).GetChild(1).GetComponent<Image>().sprite = item.sprite;
					return;
            }
        }
    }
	public bool IsFull()
    {
		for(int i = 0; i < SLOTS; i++)
        {
			if(transform.GetChild(i).transform.childCount < 2)
			{
				return false;
			}
        }
		return true;
    }
}
