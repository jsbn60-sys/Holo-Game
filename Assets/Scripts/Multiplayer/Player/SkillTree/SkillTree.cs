/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

///<summary>
/// This class manages the Skills and their unlocking + activating
///</summary>
[System.Serializable]
public class SkillTree : NetworkBehaviour
{
	public const int numberOfSKill = 10;

	// Array with all Skills
	public Skill[] skills = new Skill[numberOfSKill];

	public int availablePoints = 2;

	//Dictionary with all Skills
	private Dictionary<int, Skill> _skills;

	// Currently Inspected Skill
	private Skill inspectedSkill;

	private SkillTree instance;

	private void Start()
	{
		instance = this;	
	}

	// Use this for initialization of the skill tree
	/// <summary>
	/// Puts the Skills of the skill Tree in a Dictionary
	/// </summary>
	/// <param name="skillList">List of Skills</param>
	public void SetUpSkillTree(Skill[] skillList)
	{
		skills = skillList;
		_skills = new Dictionary<int, Skill>();

		for (int i = 0; i < skills.Length; ++i)
		{
			if (!(skills[i] == null) && !(_skills.ContainsKey(skills[i].id))) // prevent errors by checking if Skill exists and no skill with same ID is in dictionary
			{
				_skills.Add(skills[i].id, skills[i]);
			} else
			{
				if (skills[i] == null)
				{
					Debug.Log("Skill at skills[" + i + "] is null");
				}
				else
				if (_skills.ContainsKey(skills[i].id))
				{
					Debug.Log("Key: " + skills[i].id + " is already in Dictionary");
				}
			}
		}
	}

	/// <summary>
	/// return true if a skill is active and not Unlocked
	/// </summary>
	/// <param name="skillId"></param>
	/// <returns></returns>
	public bool isActive(int skillId)
	{
		if (_skills.TryGetValue(skillId, out inspectedSkill))
		{
			if (inspectedSkill.unlocked)
			{
				return false; // skill already unlocked 
			}
			if (inspectedSkill.isActiveSkill)
			{
				return true;
			}

		}
		return false;
	}

	/// <summary>
	/// Increases the Number of SkillPoints available
	/// </summary>
	/// <param name="p">Number of Points to Increase</param>
	public void IncreaseAvailablePoints(int p)
	{
		availablePoints += p;
	}

	/// <summary>
	/// Shows if a Skill is Unlocked
	/// </summary>
	/// <param name="skillId"> Id of inspected Skill </param>
	/// <returns>true if Skill is unlocked, else false</returns>
	public bool IsUnlocked(int skillId)
	{
		if (_skills.TryGetValue(skillId, out inspectedSkill))
		{
			if (inspectedSkill.unlocked)
			{
				return true; // skill already unlocked 
			}
		}
		return false;
	}

	/// <summary>
	/// Unlocks a Skill and activates its passive SkillEffect
	/// </summary>
	/// <param name="skillId">Skill Id to be unlocked</param>
	/// <returns>returns true if Skill is succesfully unlocked, else false</returns>
	public bool UnlockSkill(int skillId)
	{


		if (_skills.TryGetValue(skillId, out inspectedSkill))
		{
			if (inspectedSkill.unlocked)
			{
				return false; // skill already unlocked 
			}

			if (inspectedSkill.cost > availablePoints)
			{
				return false; // not enough skill Points
			}

			if(inspectedSkill.isActiveSkill && GetComponent<SkillMenu>().activeSkills >= 3)
			{
				return false; // more than 3 active skills are tried to unlock
			}

			int[] dependencies = inspectedSkill.skill_Dependencies;
			Skill dependencie;
			if (dependencies.Length == 0)
			{
				// if skill has no Dependencies -> unlock
				ActivateSkill(skillId, inspectedSkill);
				return true;
			}
			for(int i = 0; i < dependencies.Length; i++)
			{
				if (_skills.TryGetValue(dependencies[i], out dependencie))
				{
					if (dependencie.unlocked)
					{
						// Dependencie met, skill can be unlocked -> unlock and activate
						ActivateSkill(skillId, inspectedSkill);
						return true;
					}
				}
				else // If one of the dependencies doesn't exist, the skill can't be unlocked.
				{
					return false;
				}
			}
			return false; // no dependencie met
		}
		return false; // skill doesnt exist
	}

	/// <summary>
	/// Activates the Passive Effect of a Skill and ads it to the Skill bar if Skill is an Active Skill
	/// </summary>
	/// <param name="id"> Id of the Skill</param>
	/// <param name="skill"> actual Skill to be activated</param>
	private void ActivateSkill(int id, Skill skill)
	{
		PlayerController player = GetComponent<SkillMenu>().player;
		availablePoints -= skill.cost;
		player.investedSkillPoints += skill.cost;
		inspectedSkill.unlocked = true;
		if (skill.isActiveSkill)
		{
			GetComponent<SkillMenu>().activeSkills += 1;
			GetComponent<SkillMenu>().player.AddActiveSkill(skill);
		}
		// entry on the dictionary gets replaced with a new, unlocked, entry of the skill
		_skills.Remove(id);
		_skills.Add(id, skill);
	   

		skill.Activate(player); // activates passive Effect of Skill
	}

}
