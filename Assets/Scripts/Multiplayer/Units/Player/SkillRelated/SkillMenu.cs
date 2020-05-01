using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

/// <summary>
/// This class represents the SkillMenu.
/// It provides functionality for adding/subtracting skillPoints and unlocking skills.
/// The method tryUnlockSkill is used by skillButtons to unlock skills.
/// Skill Ids should be:
///
/// 	1	     2
///   3  4	   5  6
///         7
/// 	  8   9
///        10
///
/// </summary>
public class SkillMenu : NetworkBehaviour
{
	[SerializeField]
	private GameObject[] skillButtons;

	[SerializeField]
	private Text skillPointsText;

	private SkillQuickAccess skillQuickAccess;

	private int skillPoints;

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
	/// <param name="skill">skill to unlock</param>
	public void tryUnlockSkill(Skill skill)
	{
		if (canBeUnlocked(skill.SkillId))
		{
			activateSkill(skill.SkillId);
		}
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
		GameObject skillButton = skillButtons[id - 1];

		skillButton.GetComponent<Skill>().IsUnlocked = true;
		activateNextButtons(id);

		subtractSkillPoints(skillButton.GetComponent<Skill>().Cost);

		skillQuickAccess.addContent(skillButton.GetComponent<Skill>());
	}

	/// <summary>
	/// Checks if skill can be unlocked.
	/// </summary>
	/// <param name="id">Id of skill to check</param>
	/// <returns>Can skill be unlocked</returns>
	private bool canBeUnlocked(int id)
	{
		// skill already unlocked or not enough skill Points
		if (skillButtons[id-1].GetComponent<Skill>().IsUnlocked || (skillButtons[id-1].GetComponent<Skill>().Cost > skillPoints))
		{
			return false;
		}

		// checks if previous skills are unlocked
		switch (id)
		{
			case 1: case 2:
				return true;
			case 3:	case 4:
				return skillButtons[0].GetComponent<Skill>().IsUnlocked;
			case 5: case 6:
				return skillButtons[1].GetComponent<Skill>().IsUnlocked;
			case 7:
				return skillButtons[2].GetComponent<Skill>().IsUnlocked ||
				       skillButtons[3].GetComponent<Skill>().IsUnlocked ||
				       skillButtons[4].GetComponent<Skill>().IsUnlocked ||
				       skillButtons[5].GetComponent<Skill>().IsUnlocked;
			case 8: case 9:
				return skillButtons[6].GetComponent<Skill>().IsUnlocked;
			case 10:
				return skillButtons[7].GetComponent<Skill>().IsUnlocked ||
				       skillButtons[8].GetComponent<Skill>().IsUnlocked;
			default:
				throw new System.ArgumentException("Illegal SkillId");
		}
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
				skillButtons[2].GetComponent<Button>().interactable = true;
				skillButtons[3].GetComponent<Button>().interactable = true;
				break;
			case 2:
				skillButtons[4].GetComponent<Button>().interactable = true;
				skillButtons[5].GetComponent<Button>().interactable = true;
				break;
			case 3: case 4: case 5: case 6:
				if (!skillButtons[6].GetComponent<Skill>().IsUnlocked)
				{
					skillButtons[6].GetComponent<Button>().interactable = true;
				}
				break;
			case 7:
				skillButtons[7].GetComponent<Button>().interactable = true;
				skillButtons[8].GetComponent<Button>().interactable = true;
				break;
			case 8: case 9:
				if (!skillButtons[9].GetComponent<Skill>().IsUnlocked)
				{
					skillButtons[9].GetComponent<Button>().interactable = true;
				}
				break;
		}
	}

	public void changeSkillsCooldown(float factor)
	{
		foreach (GameObject button in skillButtons)
		{
			button.GetComponent<Skill>().changeCooldown(factor);

		}
	}
}
