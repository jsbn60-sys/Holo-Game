/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


///<summary>
/// This class provides the general structure of an skill. All skills inherit from this class.
/// A Skill contains:
/// a spirte that is used in the skill Acces bar
/// a name
/// a id
/// a list of skill id its unlock is dependent from
/// a unlocked var that shows if the skill is unlocked
/// its cost
///</summary>
[System.Serializable]
public class Skill : NetworkBehaviour 
{
	public Sprite sprite; // the sprite to represent the skill in the skillbar
	public int id; // id of this skill
	public int[] skill_Dependencies; //ids of skills needed to unlock this skill
	public bool unlocked = false; // checks if skill is already activated
	public int cost;     // tracks the cost of this skill
	public bool isActiveSkill; // tracks if Skill is an active Skill
	public int cooldown; // if Skill is active, this is field is used to specifie the cooldowwn
	public bool onCooldown = false;
	public int duration;
	public int slot;

	///<summary>
	/// Performs the unique ability of an Skill.
	///</summary>
	public virtual void Ability(PlayerController player)
	{

	}

	///<summary>
	/// Activates a passive ability of a Skill
	///</summary>
	public virtual void Activate(PlayerController player)
	{

	}

	///<summary>
	/// This function displays the icon of the Skill. It creates a new GameObject and puts it on the canvas in a predefined position.
	/// The icon will consists of the sprite image.
	///</summary>
	public void DisplayIcon(GameObject icon, Vector3 position)
	{
		Image img = icon.AddComponent<Image>();
		img.sprite = sprite;
		icon.GetComponent<RectTransform>().SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
		icon.GetComponent<RectTransform>().transform.position = position;
		icon.GetComponent<RectTransform>().sizeDelta = new Vector2(50f, 50f);
		icon.SetActive(true);
	}

}
