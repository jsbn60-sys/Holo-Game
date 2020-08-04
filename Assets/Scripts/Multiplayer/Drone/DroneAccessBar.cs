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
}
