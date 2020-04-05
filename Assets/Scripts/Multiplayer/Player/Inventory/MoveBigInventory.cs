/* edited by: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
///<summary>
///this script allows to move the bigInventory
///</summary>
public class MoveBigInventory : MonoBehaviour, IDragHandler{

	//sets BigInventory position at mousecurrent position
	public void OnDrag(PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}
}

