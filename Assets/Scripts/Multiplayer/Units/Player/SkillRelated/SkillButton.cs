using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class represents a button in the skillMenu.
/// It adds functionality for containing a skill and unlocking it.
/// Also adds a color system for the different button states.
/// It has a custom editor: SkillButtonEditor.cs
/// </summary>
public class SkillButton : Button
{
	[Header("Skill Button : Skill")]
	[SerializeField] private Skill skill;
	private bool isUnlocked;
	public Skill Skill => skill;
	public bool IsUnlocked => isUnlocked;

	[Header("Skill Button : Colors")]
	[SerializeField] private Color NOT_UNLOCKABLE_COLOR;
	[SerializeField] private Color UNLOCKABLE_COLOR;
	[SerializeField] private Color UNLOCKED_COLOR;
	[SerializeField] private Color HOVER_COLOR;


	private SkillMenu skillMenu;

	private DroneSkillMenu droneSkillMenu;

	public SkillMenu SkillMenu
	{
		set => skillMenu = value;
	}

	/// <summary>
	/// Start is called before the first frame update.
	/// </summary>
	private void Start()
	{
		isUnlocked = false;
	}

	/// <summary>
	/// Sets button to not unlockable.
	/// </summary>
	public void setNotUnlockable()
	{
		GetComponent<Image>().color = NOT_UNLOCKABLE_COLOR;
	}

	/// <summary>
	/// Sets button to unlockable.
	/// </summary>
	public void setUnlockable()
	{
		GetComponent<Image>().color = UNLOCKABLE_COLOR;
		interactable = true;
	}

	/// <summary>
	/// Sets button to unlocked.
	/// </summary>
	public void setUnlocked()
	{
		isUnlocked = true;
		GetComponent<Image>().color = UNLOCKED_COLOR;
	}

	/// <summary>
	/// Used by EventTrigger to update button.
	/// </summary>
	public void OnHoverEntry()
	{
		if (GetComponent<Image>().color.Equals(UNLOCKABLE_COLOR))
		{
			GetComponent<Image>().color = HOVER_COLOR;
		}

		if (skillMenu != null)
		{
			skillMenu.LoadSkillPopup(skill.SkillName,skill.PopupContent,Skill.EffectsList);
		}
		else
		{
			droneSkillMenu.HideSkillPopup();
		}
	}

	/// <summary>
	/// Used by EventTrigger to update button.
	/// </summary>
	public void OnHoverExit()
	{
		if (GetComponent<Image>().color.Equals(HOVER_COLOR))
		{
			GetComponent<Image>().color = UNLOCKABLE_COLOR;
		}
		skillMenu.HideSkillPopup();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		OnHoverExit();
	}
}



