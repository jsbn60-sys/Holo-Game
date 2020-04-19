using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

/// <summary>
/// This class represents the SkillMenu.
/// It provides functionality for adding skillPoints and unlocking skills.
///
///	Skill Ids should be:
//           1
//        2     3
//       4 5   6 7
//        8     9
//           10
/// </summary>
public class SkillMenu : NetworkBehaviour
{
	[SerializeField]
	private SkillButton[] skillButtons;

	[SerializeField]
	private Text skillPointsText;

	private SkillQuickAccess skillQuickAccess;

	private int skillPoints;

	// Start is called before the first frame update
	void Start()
	{
		skillPoints = 1;
	}

	/// <summary>
	/// SkillMenu needs the SkillQuickAccess for adding skills to it
	/// </summary>
	/// <param name="skillQuickAccess">The players skillQuickAccess</param>
	public void setSkillQuickAccess(SkillQuickAccess skillQuickAccess)
	{
		this.skillQuickAccess = skillQuickAccess;
	}

	/// <summary>
	/// Is called by the skillButtons to unlock skills if possible.
	/// </summary>
	/// <param name="id">id of skill to unlock</param>
	public void tryUnlockSkill(int id)
	{
		/* disabled until skills are implemented
		if (canBeUnlocked(id))
		{
			
		}
		*/

		activateSkill(id);
	}

	/// <summary>
	/// Adds skill points to the player.
	/// </summary>
	/// <param name="amount">Amount of skill points to add</param>
	public void addSkillPoints(int amount)
	{
		skillPoints += amount;
		skillPointsText.text = "Points: " + skillPoints;
	}

	/// <summary>
	/// Subtracts skill points from the player.
	/// </summary>
	/// <param name="amount">Amount of skill points to subtract</param>
	public void subtractSkillPoints(int amount)
	{
		skillPoints -= amount;
		skillPointsText.text = "Points: " + skillPoints;
	}

	/// <summary>
	/// Activates a skill.
	/// </summary>
	/// <param name="id">Id of skill</param>
	private void activateSkill(int id)
	{
		SkillButton skillButton = skillButtons[id - 1];

		skillButton.isUnlocked = true;
		deactivateButton(id);
		activateNextButtons(id);

		// subtractSkillPoints(skillButton.Skill.cost);
		// disabled until skills are implemented
		// if(skillButton.Skill.isActiveSkill)
		{
			skillQuickAccess.addContent(skillButton.Skill);
		}
	} 

	/// <summary>
	/// Checks if skill can be unlocked.
	/// </summary>
	/// <param name="id">Id of skill to check</param>
	/// <returns>Can skill be unlocked</returns>
	private bool canBeUnlocked(int id)
	{
		// skill already unlocked or not enough skill Points
		if (skillButtons[id-1].isUnlocked || (skillButtons[id-1].Skill.cost > skillPoints))
		{
			return false;
		}

		switch (id)
		{
			case 1:
				return true;
			case 2:
				return skillButtons[0].isUnlocked;
			case 3:
				return skillButtons[0].isUnlocked;
			case 4:
				return skillButtons[1].isUnlocked;
			case 5:
				return skillButtons[1].isUnlocked;
			case 6:
				return skillButtons[2].isUnlocked;
			case 7:
				return skillButtons[2].isUnlocked;
			case 8:
				return skillButtons[3].isUnlocked || skillButtons[4].isUnlocked;
			case 9:
				return skillButtons[5].isUnlocked || skillButtons[6].isUnlocked;
			case 10:
				return skillButtons[7].isUnlocked || skillButtons[8].isUnlocked;
			default:
				throw new System.ArgumentException("SkillId not between 1-10!");
		}
	}

	private void deactivateButton(int id)
	{
		skillButtons[id - 1].interactable = false;
	}

	/// <summary>
	/// Activates the next buttons after a skill was unlocked.
	/// </summary>
	/// <param name="id">Id of skill that was unlocked</param>
	private void activateNextButtons(int id)
	{
		switch (id)
		{
			case 1:
				skillButtons[1].interactable = true;
				skillButtons[2].interactable = true;
				break;
			case 2:
				skillButtons[3].interactable = true;
				skillButtons[4].interactable = true;
				break;
			case 3:
				skillButtons[5].interactable = true;
				skillButtons[6].interactable = true;
				break;
			case 4:
				if (!skillButtons[4].isUnlocked)
				{
					skillButtons[7].interactable = true;
				}
				break;
			case 5:
				if (!skillButtons[3].isUnlocked)
				{
					skillButtons[7].interactable = true;
				}
				break;
			case 6:
				if (!skillButtons[6].isUnlocked)
				{
					skillButtons[8].interactable = true;
				}
				break;
			case 7:
				if (!skillButtons[5].isUnlocked)
				{
					skillButtons[8].interactable = true;
				}
				break;
			case 8:
				if (!skillButtons[8].isUnlocked)
				{
					skillButtons[9].interactable = true;
				}
				break;
			case 9:
				if (!skillButtons[7].isUnlocked)
				{
					skillButtons[9].interactable = true;
				}
				break;
			case 10:
				break;
		}
	}
}
