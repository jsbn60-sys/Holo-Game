using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class represents a button in the SkillMenu.
/// </summary>
public class SkillButton : Button
{
	[SerializeField]
	private Skill skill;

	public bool isUnlocked;

	protected override void Start()
	{
		isUnlocked = false;
	}

	public Skill Skill
	{
		get
		{
			return skill;
		}
	}
}
