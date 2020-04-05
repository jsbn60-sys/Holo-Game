/* author: SWT-P_WS_2018_Holo */
using System;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


/// <summary>
/// This class manages the inventory system of the drone. The drone inventory has
/// four slots. Three of them are for storing collected students. A crafted
/// Item is placed in the last slot.
/// </summary>
public class DroneAccessBar : NetworkBehaviour {
	private const int SLOTS = 3; // 3 slots for collecting students
	public int[] students = new int[SLOTS]; // list of student types
	public GameObject itemImagePrefab;
	private GameObject craftedItem;


	public void OnStartServer(){
		for (int i = 0; i < students.Length; i++){
			students[i] = -1;
		}
	}


	/// <summary>
	/// Checks if the student slots are full.
	/// </summary>
	/// <returns>true if all students slots are full, if not: false</returns>
	public bool IsFull()
	{
		for (int i = 0; i < SLOTS; i++)
		{
			if (transform.GetChild(i).transform.childCount < 2)
			{
				return false;
			}
		}
		return true;
	}
	/// <summary>
	/// Puts the crafted item from the docking station into the fourth slot of the inventory.
	/// </summary>
	/// <param name="i">The crafted item</param>
	public void CollectCraftedItem(GameObject i){
		Item item = i.GetComponent<Item>();
		if (item != null){
			Transform slot = gameObject.transform.GetChild(3); //itemslot
			if (slot.transform.childCount <= 1)
			{
				// instantiate ItemImage in the inventory
				GameObject itemImage = Instantiate(itemImagePrefab);
				itemImage.transform.SetParent(slot, false);
				itemImage.GetComponent<InventoryItem>().item = item;
			}
			craftedItem = i;
			// get the right sprite to represent the item in the inventory
			slot.GetChild(1).GetChild(1).GetComponent<Image>().sprite = item.sprite;
		}
	}

	/// <summary>
	/// Adds a student to the inventory.
	/// </summary>
	/// <param name="type">The student's type</param>
	/// <param name="sprite">The sprite to represent the student in the inventory</param>
	/// <returns></returns>
	public int AddStudent(int type, Sprite sprite){
		for (int i = 0; i < SLOTS; i++)
		{
			Transform slot = gameObject.transform.GetChild(i);
			if (slot.childCount < 2)
			{
				// instantiate ItemImage of the student in the Inventory
				GameObject itemImage = Instantiate(itemImagePrefab);
				itemImage.transform.SetParent(slot, false);
				students[i]=type;
				// get the right sprite to represent the student in the inventory
				slot.GetChild(1).GetChild(1).GetComponent<Image>().sprite = sprite;
				return i;
			}

		}
		return 0;
	}

	
	/// <summary>
	/// Deletes all students from the inventory
	/// </summary>
	public void ClearStudentList(){
		for(int i = 0; i < SLOTS; i++)
		{
			if (students[i] != -1){
				ClearItem(i); // ItemImage in accessbar gets destroyed
				students[i] = -1; // to mark this slot as empty
			}
		}
	}

	/// <summary>
	/// Drops the Item from the fourth slot.
	/// </summary>
	public void Drop()
    {
        if (craftedItem != null) {
			gameObject.GetComponentInParent<Drone>().CmdDrop();
			ClearItem(3);
			craftedItem = null;
		}
    }


	/// <summary>
	/// Removes the ItemImage from a desired slot.
	/// </summary>
	/// <param name="x">The desired slot number</param>
	private void ClearItem(int x) {		
        Transform slot = gameObject.transform.GetChild(x);
		for (int i=0; i < slot.childCount; i++) {
			if (slot.transform.childCount > 1)
			{
				Destroy(slot.GetChild(1).gameObject);
			}
		}
	}

	/// <summary>
	/// Returns the image from the last created prefab.
	/// </summary>
	/// <returns>ItemImage prefab</returns>
	public GameObject GetItemImagePrefab()
	{
		return itemImagePrefab;
	}
}
