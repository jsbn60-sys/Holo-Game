/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ClassButtonMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public GameObject popUp;
	public Button roleButton;
	public bool open = false;

	public Text choosenClass;
	public Text className;
	public Text classDescription;

	private string currClass = "";

	// Array that holds all Class Descriptions (Order: MNI, Gesundheit, Wirtschaft, LSE, Drone
	private readonly string[] classDescriptions = new string[] { "Der MNI Professor repräsentiert die Schadens-Klasse, welche hauptsächlich DMG-Skills zur Verfügung hat.",
			"Der Gesundheit Professor repräsentiert die Heiler-Klasse. Er kann als einzige Klasse andere Professoren heilen.",
			"Der Wirtschaft Professor repräsentiert den Support, welcher über Unterstützende Fähigkeiten verfügt.",
			"Der LSE Professor stellt die Tank Klasse dar. Er hat die meisten Lebenspunkte aller Klassen.",
			"Die Drone untersützt Profressoren indem sie Studenten einsammelt und daraus Items herstellt.",
	};

	void Start()
	{
		popUp.SetActive(false);
		open = false;

		ChangePopUptext();
	}

	void Update()
	{
		if(open && !Cursor.visible)
		{
			popUp.SetActive(false);
			open = false;
		}
		ChangePopUptext();		
	}

	/// <summary>
	/// Changes the PopUp Text to match the selected Class.
	/// </summary>
	private void ChangePopUptext()
	{
		if (!string.Equals(choosenClass.text, currClass))
		{
			currClass = choosenClass.text;

			switch (currClass.Trim())
			{
				case "MNI":
					className.text = "Schaden(MNI)";
					classDescription.text = classDescriptions[0];
					break;
				case "Gesundheit":
					className.text = "Heiler(Gesundheit)";
					classDescription.text = classDescriptions[1];
					break;
				case "Wirtschaft":
					className.text = "Support(Wirtschaft)";
					classDescription.text = classDescriptions[2];
					break;
				case "LSE":
					className.text = "Tank(LSE)";
					classDescription.text = classDescriptions[3];
					break;
				case "Drone":
					className.text = "Drone";
					classDescription.text = classDescriptions[4];
					break;
			}
		}
	}

	// Deactivates the PopUp and resets the Var
	public void DeactivatePopUp()
	{
		popUp.SetActive(false);
		open = false;
	}

	// Shows a PopUp when Mouse is Over the Button
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (roleButton.IsInteractable() && !open)
		{
			popUp.SetActive(true);
			open = true;
		}
	}

	// Disables PopUp when Mouse leaves SkillButton
	public void OnPointerExit(PointerEventData eventData)
	{
		if (roleButton.IsInteractable() && open)
		{
			popUp.SetActive(false);
			open = false;
		}
	}
}
