/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///this script closes the BigInventory.
///Its a Buttonscript
///</summary>
public class InventoryCloseButton : MonoBehaviour {

	public void CloseInventory()
	{
		transform.parent.gameObject.SetActive(false);
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	
}
