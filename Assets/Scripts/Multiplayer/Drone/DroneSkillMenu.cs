using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class represents a skill menu for the drone.
/// It handles 5 skills for a tree that looks like the following:
///
///              1
///
///			2	 3    4
///
/// 			 5
/// </summary>
public class DroneSkillMenu : SkillMenu
{
	/// <summary>
	/// At the start the first skill is unlockable.
	/// </summary>
	protected override void SetFirstButtonsUnlockable()
	{
		skillButtons[0].setUnlockable();
	}

	/// <summary>
	/// Inserts skill into intended slot.
	/// </summary>
	/// <param name="id">Id of skill to insert</param>
	protected override void insertSkill(int id)
	{
		Skill skill = skillButtons[id - 1].Skill;
		skillQuickAccess.addContent(skill, id - 1);
	}

	/// <summary>
	/// Checks if skill can be unlocked.
	/// </summary>
	/// <param name="id">Id of skill to check</param>
	/// <returns>Can skill be unlocked</returns>
	/// <exception cref="ArgumentException">If skill id is not legal</exception>
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
			case 1:
				return true;
			case 2: case 3: case 4:
				return skillButtons[0].IsUnlocked;
			case 5:
				return skillButtons[1].IsUnlocked || skillButtons[2].IsUnlocked || skillButtons[3].IsUnlocked;
			default:
				throw new ArgumentException("Illegal SkillId");
		}
	}

	/// <summary>
	/// Returns skill for skill id and
	/// Checks if a double upgrade has been unlocked.
	/// </summary>
	/// <param name="id">Id of skill to get</param>
	/// <returns>Skill to unlock</returns>
	/// <exception cref="ArgumentException">If skill id is not legal</exception>
	protected override void activateNextButtons(int id)
	{
		switch (id)
		{
			case 1:
				skillButtons[1].setUnlockable();
				skillButtons[2].setUnlockable();
				skillButtons[3].setUnlockable();
				break;
			case 2: case 3: case 4:
				if (!skillButtons[4].IsUnlocked)
				{
					skillButtons[4].setUnlockable();
				}
				break;
			case 5:
				break;
			default:
				throw new ArgumentException("Illegal SkillId");

		}
	}
}
