/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;


/// <summary>
/// This simple class resembles an Skill after it is activated and set int the Skill Bar.
/// The member skill is used in the activation Process of a Skill.
/// </summary>
public class SkillBarItem : NetworkBehaviour
{
	public Skill skill;
}
