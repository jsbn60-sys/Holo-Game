using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class represents a slot for an skill in the skillQuickAccess.
/// It extends the generic class Slot and adds functionality for skill cooldowns
/// and for skills having multiple charges that load up over time.
/// </summary>
public class SkillSlot : Slot<Skill>
{
	[SerializeField] private Text cooldownText;
	[SerializeField] private Text chargesText;

	private float cooldownTimer;

	private int currCharges;

	/// <summary>
	/// Inserts a skill into a skillSlot.
	/// Overrides the Slot function insertContent()
	/// and initializes all necessary values.
	/// </summary>
	/// <param name="content">Skill to insert</param>
	public override void insertContent(Skill content)
	{
		base.insertContent(content);
		currCharges = 1;
		cooldownTimer = content.Cooldown;
		updateCooldownText();
		updateChargesText();
	}

	/// <summary>
	/// Uses a skill from a skillSlot.
	/// Overrides the Slot function useContent().
	/// Only executes if there is atleast one charge.
	/// </summary>
	/// <param name="player">Player that used skill</param>
	public override void useContent(Unit player)
	{
		if (currCharges > 0)
		{
			currCharges--;
			if (cooldownTimer <= 0f)
			{
				cooldownTimer = content.Cooldown;
			}
			updateChargesText();
			base.useContent(player);
		}
	}

	/// <summary>
	/// Updates the cooldown and charges.
	/// </summary>
	private void Update()
	{
		if (!IsEmpty && !isFullyCharged())
		{
			if (cooldownTimer > 0)
			{
				cooldownTimer -= Time.deltaTime;
			}
			else
			{
				currCharges++;
				updateChargesText();

				if (!isFullyCharged())
				{
					cooldownTimer = content.Cooldown;
				}
			}
			updateCooldownText();
		}
	}

	/// <summary>
	/// Utility function for checking if the slot has max. amount of charges.
	/// </summary>
	/// <returns>Is the slot fully charged</returns>
	private bool isFullyCharged()
	{
		return currCharges >= content.MaxCharges;
	}


	/// <summary>
	/// Updates the cooldownText.
	/// </summary>
	private void updateCooldownText()
	{
		if (isFullyCharged())
		{
			setText(cooldownText,"");
		}
		else if (cooldownTimer > 0)
		{
			setText(cooldownText,((int)cooldownTimer).ToString());
		}
	}

	/// <summary>
	/// Updates the chargesText.
	/// Charges are only shown for skills that have more than one maxCharge.
	/// </summary>
	private void updateChargesText()
	{
		if (content.MaxCharges > 1)
		{
			setText(chargesText,currCharges.ToString());
		}
		else
		{
			setText(chargesText,"");
		}
	}

	/// <summary>
	/// Utility function for updating a text element.
	/// </summary>
	/// <param name="textToSet">Text element to update</param>
	/// <param name="text">text to insert</param>
	private void setText(Text textToSet, string text)
	{
		textToSet.text = text;
	}
}
