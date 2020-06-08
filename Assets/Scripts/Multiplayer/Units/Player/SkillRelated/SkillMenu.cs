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
/// If skills 3&4, 5&6 or 8&9 have both been skilled they combine into a new skill.
/// These skills are stored in doubleUpgradeSkills, because they don't have a SkillButton
/// that they can be stored in. The rule is:
///
/// Skill 3&4 => doubleUpgradeSkills[0]
/// Skill 5&6 => doubleUpgradeSkills[1]
/// Skill 8&9 => doubleUpgradeSkills[2]
///
/// </summary>
public class SkillMenu : NetworkBehaviour
{
	[SerializeField] private SkillButton[] skillButtons;

	[SerializeField] private Skill[] doubleUpgradeSkills;

	[SerializeField] private Text skillPointsText;

	private SkillQuickAccess skillQuickAccess;

	private int skillPoints;

	/// <summary>
	/// Start is called before the first frame update.
	/// Sets up buttons.
	/// </summary>
	void Start()
	{
		skillPoints = 10;
		skillPointsText.text = "Points: " + skillPoints;

		foreach(SkillButton button in skillButtons)
		{
			button.GetComponent<Image>().sprite = button.Skill.getIcon();
			button.setNotUnlockable();
			// Workaround because listener cant be added through editor
			button.onClick.AddListener(() => tryUnlockSkill(button.Skill));
		}

		skillButtons[0].setUnlockable();
		skillButtons[1].setUnlockable();
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
			unlockSkill(skill.SkillId);
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
	private void unlockSkill(int id)
	{
		SkillButton skillButton = skillButtons[id - 1];

		skillButton.setUnlocked();
		activateNextButtons(id);

		subtractSkillPoints(skillButton.Skill.Cost);

		insertSkill(id);
	}

	/// <summary>
	/// Inserts skill into intended slot.
	/// Checks if a double upgrade has been unlocked,
	/// in this case the id of the double upgrade is irrelevant.
	/// </summary>
	/// <param name="id">Id of skill to insert</param>
	/// <exception cref="ArgumentException">If skill id is not legal</exception>
	private void insertSkill(int id)
	{
		Skill skill = checkForDoubleUpgrade(id);
		switch (id)
		{
			case 1: case 3: case 4:
				skillQuickAccess.addContent(skill,0);
				break;
			case 2: case 5: case 6:
				skillQuickAccess.addContent(skill,1);
				break;
			case 7: case 8: case 9:
				skillQuickAccess.addContent(skill,2);
				break;
			case 10:
				skillQuickAccess.addContent(skill,3);
				break;
			default:
				throw new System.ArgumentException("Illegal SkillId");
		} ;
	}

	/// <summary>
	/// Checks if skill can be unlocked.
	/// </summary>
	/// <param name="id">Id of skill to check</param>
	/// <returns>Can skill be unlocked</returns>
	private bool canBeUnlocked(int id)
	{
		// skill already unlocked or not enough skill Points
		if (skillButtons[id-1].IsUnlocked || (skillButtons[id-1].Skill.Cost > skillPoints))
		{
			return false;
		}
		// checks if previous skills are unlocked
		switch (id)
		{
			case 1: case 2:
				return true;
			case 3:	case 4:
				return skillButtons[0].IsUnlocked;
			case 5: case 6:
				return skillButtons[1].IsUnlocked;
			case 7:
				return skillButtons[2].IsUnlocked ||
				       skillButtons[3].IsUnlocked ||
				       skillButtons[4].IsUnlocked ||
				       skillButtons[5].IsUnlocked;
			case 8: case 9:
				return skillButtons[6].IsUnlocked;
			case 10:
				return skillButtons[7].IsUnlocked ||
				       skillButtons[8].IsUnlocked;
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
				skillButtons[2].setUnlockable();
				skillButtons[3].setUnlockable();
				break;
			case 2:
				skillButtons[4].setUnlockable();
				skillButtons[5].setUnlockable();
				break;
			case 3: case 4: case 5: case 6:
				if (!skillButtons[6].IsUnlocked)
				{
					skillButtons[6].setUnlockable();
				}
				break;
			case 7:
				skillButtons[7].setUnlockable();
				skillButtons[8].setUnlockable();
				break;
			case 8: case 9:
				if (!skillButtons[9].IsUnlocked)
				{
					skillButtons[9].setUnlockable();
				}
				break;
		}
	}

	/// <summary>
	/// Returns skill for skill id and
	/// Checks if a double upgrade has been unlocked.
	/// </summary>
	/// <param name="id">Id of skill to get</param>
	/// <returns>Skill to unlock</returns>
	/// <exception cref="ArgumentException">If skill id is not legal</exception>
	private Skill checkForDoubleUpgrade(int id)
	{
		switch (id)
		{
			case 3: case 4:
				if (skillButtons[2].IsUnlocked && skillButtons[3].IsUnlocked)
				{
					return doubleUpgradeSkills[0];
				}
				else
				{
					return skillButtons[id - 1].Skill;
				}
			case 5: case 6:
				if (skillButtons[4].IsUnlocked && skillButtons[5].IsUnlocked)
				{
					return doubleUpgradeSkills[1];
				}
				else
				{
					return skillButtons[id - 1].Skill;
				}
			case 8: case 9:
				if (skillButtons[7].IsUnlocked && skillButtons[8].IsUnlocked)
				{
					return doubleUpgradeSkills[2];
				}
				else
				{
					return skillButtons[id - 1].Skill;
				}
			case 1: case 2: case 7: case 10:
				return skillButtons[id - 1].Skill;
			default:
				throw new System.ArgumentException("Illegal skillId!");
		}
	}
}
