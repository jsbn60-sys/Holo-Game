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
public abstract class SkillMenu : MonoBehaviour
{
	[SerializeField] protected SkillButton[] skillButtons;

	[SerializeField] private Text skillPointsText;

	[SerializeField] private Popup skillPopup;

	protected SkillQuickAccess skillQuickAccess;

	protected int skillPoints;

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
			button.SkillMenu = this;
			// Workaround because listener cant be added through editor
			button.onClick.AddListener(() => tryUnlockSkill(button.Skill));
		}

		SetFirstButtonsUnlockable();
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
	/// Sets the first skills that are skillable as unlockable.
	/// </summary>
	protected abstract void SetFirstButtonsUnlockable();

	/// <summary>
	/// Inserts skill into intended slot.
	/// Checks if a double upgrade has been unlocked,
	/// in this case the id of the double upgrade is irrelevant.
	/// </summary>
	/// <param name="id">Id of skill to insert</param>
	/// <exception cref="ArgumentException">If skill id is not legal</exception>
	protected abstract void insertSkill(int id);


	/// <summary>
	/// Checks if skill can be unlocked.
	/// </summary>
	/// <param name="id">Id of skill to check</param>
	/// <returns>Can skill be unlocked</returns>
	protected abstract bool canBeUnlocked(int id);


	/// <summary>
	/// Activates the next buttons after a skill was unlocked.
	/// </summary>
	/// <param name="id">Id of skill that was unlocked</param>
	protected abstract void activateNextButtons(int id);

	/// <summary>
	/// Loads the skill popup.
	/// </summary>
	/// <param name="headline">Headline for the popup</param>
	/// <param name="content">Content description</param>
	/// <param name="effectsList">Description of effects</param>
	public void LoadSkillPopup(string headline, string content, string effectsList)
	{
		skillPopup.ActivatePopup(headline,content,effectsList);
	}

	/// <summary>
	/// Hides skill popup.
	/// </summary>
	public void HideSkillPopup()
	{
		skillPopup.DeactivePopup();
	}
}
