/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;


/// <summary>
/// This class implements the skill slots of the player.
/// The quick access bar has three slots for adding skills.
/// </summary>
public class SkillSlots : MonoBehaviour
{
	public const int SLOTS = 3;
	private Skill[] skills = new Skill[SLOTS];
	public GameObject skillImagePrefab;

	///<summary>
	/// Adds an item to the quick access bar
	/// </summary>
	///<param name="skill">the Skill to add</param>
	public void AddSkill(Skill skill)
	{
		for (int i = 0; i < SLOTS; i++)
		{
			// get next empty slot
			Transform slot = gameObject.transform.GetChild(i);
			if (slot.childCount < 2)
			{
				// instantiate skillImage
				GameObject skillImage = Instantiate(skillImagePrefab);
				skillImage.transform.SetParent(slot, false);
				skillImage.GetComponent<SkillBarItem>().skill = skill;
				skills[i] = skill;
				skill.slot = i;
				// get the right sprite to represent the skill
				slot.GetChild(1).GetChild(1).GetComponent<Image>().sprite = skill.sprite;
				return;
			}
		}

	}

	/// <summary>
	/// Shows if all SkillSlots are filled
	/// </summary>
	/// <returns> true if all Slots are Full, else false</returns>
	public bool IsFull()
	{
		for (int i = 0; i < SLOTS; i++)
		{
			if (transform.GetChild(i).transform.childCount < 2)
			{
				return false;
			}
		}
		return true;
	}
}
