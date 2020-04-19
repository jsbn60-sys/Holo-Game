/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// This class manages showing the PopUp of a Skill Button on Mouse Over
/// </summary>
public class SkillOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public GameObject popUp;
	public bool open = false;

	// Shows a PopUp when Mouse is Over the Button
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!open)
		{
			popUp.SetActive(true);
			open = true;
		}
	}

	// Disables PopUp when Mouse leaves SkillButton
	public void OnPointerExit(PointerEventData eventData)
	{
		if (open)
		{
			popUp.SetActive(false);
			open = false;
		}
	}

}
