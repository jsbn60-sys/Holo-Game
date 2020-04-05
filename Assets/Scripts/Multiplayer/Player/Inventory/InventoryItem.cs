/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

/// <summary>
///This class resembles an item after it is picked up and situated in the players inventory or quickaccessbar.
///It manages displaying tooltips for the player, if the player hovers their mouse over the item.
///If the player then moves his mouse away, the tooltip box will disappear.
///The tooltips will only work if the inventory is opened and the cursor is visible.
/// </summary>
public class InventoryItem : NetworkBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Item item;
	public GameObject tooltip;
	private bool droneFlag; //if Item is in DroneAccessBar

	void Start() {
		FindTooltip();
	}


	void Update() {
		if(droneFlag){
			return;
		}
		if (Input.GetKeyDown(KeyCode.I))
		{ if (tooltip.activeSelf)
				tooltip.SetActive(false); //don't show tooltip whenever inventory is inactive (not opened)
		}
	}

	/// <summary>
	/// Display the tooltip when mouse is on the InventoryItem
	/// </summary>
	public void OnPointerEnter(PointerEventData eventData)
	{
		if(droneFlag){
			return;
		}
		if (EventSystem.current.IsPointerOverGameObject()) {
			string tooltipText= tooltip.transform.GetChild(0).gameObject.GetComponent<Text>().text = item.name.Replace("(Clone)", "");
				if (tooltipText.Substring(tooltipText.Length - 1).Equals(")"))
				{
					tooltip.transform.GetChild(0).gameObject.GetComponent<Text>().text = tooltipText.Substring(0, (tooltipText.Length - 3));
				}
				else
				{
					tooltip.transform.GetChild(0).gameObject.GetComponent<Text>().text = tooltipText;
				}
		

			if (gameObject.transform.position.y > 100)
		{
			tooltip.transform.position = gameObject.transform.position + new Vector3(150, -70, 0);
		}
		else {

			tooltip.transform.position = gameObject.transform.position + new Vector3(150, 70, 0);
		}
		tooltip.SetActive(true);
	}


	}

	/// <summary>
	/// Hide tooltip when mouse is not on the InventoryItem any more
	/// </summary>
	public void OnPointerExit(PointerEventData eventData)
	{
		if(droneFlag){
			return;
		}
		tooltip.SetActive(false); //hide tooltip if mouse isn't hovering anymore
	}

	/// <summary>
	/// This function helps finding the tooltip in the game hierarchy after is it set inactive at the start,
	/// because gameObject.Find() etc won't find inactive gameobjects
	/// </summary>
	private void FindTooltip(){
		Transform[] trs;
		if(gameObject.transform.parent.parent.name == "QuickAccessBar"){
			trs= gameObject.transform.parent.parent.parent.GetComponentsInChildren<Transform>(true);
		}
		else if(gameObject.transform.parent.parent.name == "DroneAccessBar"){
			droneFlag = true;
			return;
		}
		else{
			trs= gameObject.transform.parent.parent.parent.parent.GetComponentsInChildren<Transform>(true);
		}
		foreach(Transform t in trs){
			if(t.name == "Tooltip"){
				tooltip = t.gameObject;
			}
		}
	}
	
}
