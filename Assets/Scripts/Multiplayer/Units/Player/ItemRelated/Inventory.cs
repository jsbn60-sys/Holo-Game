/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


///<summary>
/// This class manages the inventory of the player. The inventory has 16 free slots to store items.
/// The items are saved in an array.
/// It is possible to add items and to check if the inventory is full.
/// </summary>
public class Inventory : MonoBehaviour {

	private Item[] items = new Item[slots];
	public const int slots = 16;
	public GameObject itemImagePrefab;
	
	///<summary>
	/// Adds an item to the inventory
	/// </summary>
	///<param name="item">the item to add</param>
	public void AddItem(Item item)
	{
		for (int i = 0; i < slots; i++)
		{
			Transform slot = gameObject.transform.GetChild(i);
			// get next empty slot
			if (slot.transform.childCount < 2)
			{
				// instantiate ItemImage in the Inventory
				GameObject itemImage = Instantiate(itemImagePrefab);
				itemImage.transform.SetParent(slot, false);
				itemImage.GetComponent<ItemSlot>().insertContent(item);
				items[i] = item;

				// get the right sprite to represent the item in the inventory
				slot.GetChild(1).GetChild(1).GetComponent<Image>().sprite = item.getIcon();
				return;
			}
		}
	}

	public bool IsFull()
	{
		for (int i = 0; i < slots; i++)
		{
			if (transform.GetChild(i).transform.childCount < 2)
			{
				return false;
			}
		}
		return true;
	}
}
