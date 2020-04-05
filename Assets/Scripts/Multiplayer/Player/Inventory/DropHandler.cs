/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

///<summary>
///this is a simple DropHandler. GameObject with DropHandler attached can be a dropzone for
///gameobjects with ItemDragHandler attached. If there is already a gameobject in a Slot the Items will swap
///and resize there Icons filling the parents size.
///Simple tutorial: https://www.youtube.com/watch?v=Pc8K_DVPgVM Darg and Drop Basic
///</summary>
public class DropHandler : MonoBehaviour, IDropHandler /*IPointerEnterHandler, IPointerExitHandler*/ {
	//saves the size of parent for resizing itemicons
	private RectTransform parentSize;

	public void OnDrop(PointerEventData eventData)
	{
		if (this.transform.childCount <2)
		{
			//drops Item on the empty slot
			ItemDragHandler dragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();
			if (dragHandler != null)
			{		
					parentSize = this.gameObject.GetComponent<RectTransform>() as RectTransform;
					RectTransform sizeofitem = dragHandler.gameObject.GetComponent<RectTransform>() as RectTransform;
					sizeofitem.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parentSize.rect.height);
					sizeofitem.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentSize.rect.width);
					dragHandler.parentToReturn = this.transform;
					
			}
		}
		else
		{
			//if slot is full -swaps items and resize thier icons
			ItemDragHandler dragHandlerI1 = eventData.pointerDrag.GetComponent<ItemDragHandler>();
			ItemDragHandler dragHandlerI2 = this.transform.GetComponentInChildren<ItemDragHandler>();
			if (dragHandlerI1 != null)
			{	
				RectTransform parentSize = dragHandlerI2.gameObject.GetComponent<RectTransform>() as RectTransform;
				RectTransform sizeofitem = dragHandlerI1.gameObject.GetComponent<RectTransform>() as RectTransform;
				float y = parentSize.rect.height;
				float x = parentSize.rect.width;
				parentSize.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeofitem.rect.height);
				parentSize.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeofitem.rect.width);
				sizeofitem.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);
				sizeofitem.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);

				dragHandlerI2.parentToReturn = dragHandlerI1.parentToReturn;
				dragHandlerI1.parentToReturn = this.transform;
				dragHandlerI2.Swap();
			}
		}
	
		
	}

}
