using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Popup : MonoBehaviour
{
	private void Start()
	{
		DeactivePopup();
	}

	[SerializeField] private Text headline;
	[SerializeField] private Text content;
	[SerializeField] private Text effectsList;
	[SerializeField] private Text effectsHeader;
	public void ActivatePopup(string headline, string content,string effectsList)
	{
		effectsHeader.text = "Effects:";

		this.headline.text = headline;
		this.content.text = content;
		this.effectsList.text = effectsList;
	}

	public void DeactivePopup()
	{
		this.effectsHeader.text = "";
		this.headline.text = "";
		this.content.text = "";
		this.effectsList.text = "";
	}
}
