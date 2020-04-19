/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

///<summary>
///this is a simple DragHandler. GameObject with DragHandler attached can be draged freely in Canvas
///and can be droped on gameobjects with DropHandler attached. If there is already a gameobject in a Slot the Items will swap
///and resize there Icons filling the parents size.
///Simple tutorial: https://www.youtube.com/watch?v=Pc8K_DVPgVM Darg and Drop Basic
///</summary>

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{


	
	//this variable saves the origin position. it changes if you drop the gameobject on a gameobject with Drophandler attached
	public Transform parentToReturn = null;

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (Cursor.visible)
		{
			parentToReturn = this.transform.parent;
			this.transform.SetParent(this.transform.parent.parent.parent);
			GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (Cursor.visible)
		{
			transform.position = Input.mousePosition;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (Cursor.visible)
		{
			this.transform.SetParent(parentToReturn);
			transform.localPosition = Vector3.zero;
			GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
	}

	//swaps items
	public void Swap()
	{
		this.transform.SetParent(parentToReturn);
		transform.localPosition = Vector3.zero;
	}

}
