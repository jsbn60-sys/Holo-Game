using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class represents a button in the skillMenu.
/// It adds functionality for containing a skill and unlocking it.
/// It has a custom editor: SkillButtonEditor.cs
/// </summary>

public class SkillButton : Button
{
	[SerializeField] private Skill skill;
	private bool isUnlocked;

	public Skill Skill => skill;

	private void Start()
	{
		isUnlocked = false;
	}

	public bool IsUnlocked
	{
		get => isUnlocked;
		set => isUnlocked = value;
	}
}



