/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
/// This class manages the skillMenu of a Player.
/// This inlcudes activating and deactivating UI Buttons and calling Methods from the SkillTree class
/// to unlock skills
///</summary>
public class SkillMenu : MonoBehaviour
{
	public PlayerController player;
	public GameObject menu;
	public Transform popUp;
	public Text skillPointsText;

	public Button startSkill;
	public Button ultiSkill;

	//right skillTree
	public Button right1;
	public Button right2left;
	public Button right2right;
	public Button right3;

	//left skillTree
	public Button left1;
	public Button left2left;
	public Button left2right;
	public Button left3;

	private bool blockedRight = false;
	private bool blockedLeft = false;
	private bool ultiUnlocked = false;

	public Button[] skillButtons;
	public SkillTree skillTree;

	public int activeSkills = 0; // tracks how many active skills are activated

	void Start()
	{
		//skillTree = this.GetComponent<SkillTree>();
		skillButtons = new Button[] { startSkill, ultiSkill, right1, right2left, right2right, right3, left1,left2left, left2right,left3 };
	}

	// Updates the shown SkillPoints
	private void Update()
	{
		skillPointsText.text = "Points: " + skillTree.availablePoints;
		if (activeSkills >= 3 && ultiSkill.IsInteractable())
		{
			ultiSkill.interactable = false;
			//ColorBlock deactivate = ultiSkill.colors;
			//deactivate.disabledColor = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.3921569f);
			//ultiSkill.colors = deactivate;
		}

		if(activeSkills >= 3)
		{
			if (left1.interactable && skillTree.isActive(2)) left1.interactable = false;
			if (right1.interactable && skillTree.isActive(3)) right1.interactable = false;
			if (left2left.interactable && skillTree.isActive(4)) left2left.interactable = false;
			if (left2right.interactable && skillTree.isActive(5)) left2right.interactable = false;
			if (right2left.interactable && skillTree.isActive(6)) right2left.interactable = false;
			if (right2right.interactable && skillTree.isActive(7)) right2right.interactable = false;
			if (left3.interactable && skillTree.isActive(8)) left3.interactable = false;
			if (right3.interactable && skillTree.isActive(9)) right3.interactable = false;
		}
	}

	/// <summary>
	/// Deactivates a Button and changes its disabled color to standard
	/// </summary>
	/// <param name="btn"> btn that gets changed</param>
	private void DeactivateButton(Button btn)
	{
		ColorBlock cb = btn.colors;
		cb.disabledColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		btn.colors = cb;

		btn.interactable = false;
	}

	/// <summary>
	/// Deactivates all SkillPopUps after the Menu is closed
	/// </summary>
	public void DeactivatePopUps()
	{
		foreach(Button btn in skillButtons){
			btn.transform.Find("SkillPopUp").gameObject.SetActive(false);
			btn.transform.GetComponent<SkillOnMouseOver>().open = false;
		}
	}

	public void SetSkillPoints(int p)
	{
		skillTree.IncreaseAvailablePoints(p);
	}

	// Methods that handle the pressed Buttons to Unlock a Skill
	//    Skill Ids should be:
	//           1
	//        2     3
	//       4 5   6 7
	//        8     9
	//           10
	public void StartSkill()
	{
		if (skillTree.UnlockSkill(1))
		{
			DeactivateButton(startSkill);
			left1.interactable = true;
			right1.interactable = true;
		}
	}

	public void Left1OnClick() // Skill Id 2
	{
		if (skillTree.UnlockSkill(2))
		{
			DeactivateButton(left1);

			left2left.interactable = true;
			left2right.interactable = true;
		}
		
	}

	public void Right1OnClick() // Skill Id 3
	{
		if (skillTree.UnlockSkill(3))
		{
			DeactivateButton(right1);

			right2left.interactable = true;
			right2right.interactable = true;
		}
	}



	public void Left2LeftOnClick() // Skill Id 4
	{
		if (skillTree.UnlockSkill(4))
		{
			DeactivateButton(left2left);

			if (!blockedLeft)
			{
				left3.interactable = true;
				blockedLeft = true;
			}
		}
	}

	public void Left2RightOnClick() // Skill Id 5
	{
		if (skillTree.UnlockSkill(5))
		{
			DeactivateButton(left2right);

			if (!blockedLeft)
			{
				left3.interactable = true;
				blockedLeft = true;
			}
		}
	}

	public void Right2LeftOnClick() // Skill Id 6
	{
		if (skillTree.UnlockSkill(6))
		{
			DeactivateButton(right2left);

			if (!blockedRight)
			{
				right3.interactable = true;
				blockedRight = true;
			}
		}
	}

	public void Right2RightOnClick() // Skill Id 7
	{
		if (skillTree.UnlockSkill(7))
		{
			DeactivateButton(right2right);
			if (!blockedRight)
			{
				right3.interactable = true;
				blockedRight = true;
			}
		}
	}

	public void Left3OnClick() // Skill Id 8
	{
		if (skillTree.UnlockSkill(8))
		{
			DeactivateButton(left3);
			if (!ultiUnlocked && activeSkills < 3)
			{
				ultiSkill.interactable = true;
				ultiUnlocked = true;
			}
		}
	}

	public void Right3OnClick() // Skill Id 9
	{
		if (skillTree.UnlockSkill(9))
		{
			DeactivateButton(right3);
			if (!ultiUnlocked && activeSkills < 3)
			{
				ultiSkill.interactable = true;
				ultiUnlocked = true;
			}
		}
	}

	public void UltiOnKlick() // Skill Id 10
	{
		
		if (skillTree.UnlockSkill(10))
		{
			DeactivateButton(ultiSkill);
		}
	}

	// End of Button Methods
}
