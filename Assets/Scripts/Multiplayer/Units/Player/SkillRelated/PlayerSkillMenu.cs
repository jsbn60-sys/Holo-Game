using System;
using UnityEngine;


public class PlayerSkillMenu : SkillMenu
{
	[SerializeField] protected Skill[] doubleUpgradeSkills;


	/// <summary>
	/// At the start the skills with the id 0 and 1 are unlockable.
	/// </summary>
	protected override void SetFirstButtonsUnlockable()
	{
		skillButtons[0].setUnlockable();
		skillButtons[1].setUnlockable();
	}

	/// <summary>
	/// Inserts skill into intended slot.
	/// Checks if a double upgrade has been unlocked,
	/// in this case the id of the double upgrade is irrelevant.
	/// </summary>
	/// <param name="id">Id of skill to insert</param>
	/// <exception cref="ArgumentException">If skill id is not legal</exception>
	protected override void insertSkill(int id)
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
	/// Activates the next buttons after a skill was unlocked.
	/// </summary>
	/// <param name="id">Id of skill that was unlocked</param>
	protected override void activateNextButtons(int id)
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
	/// Checks if skill can be unlocked.
	/// </summary>
	/// <param name="id">Id of skill to check</param>
	/// <returns>Can skill be unlocked</returns>
	protected override bool canBeUnlocked(int id)
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
