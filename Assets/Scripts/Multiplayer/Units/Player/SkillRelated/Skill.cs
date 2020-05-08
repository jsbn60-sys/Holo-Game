using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

/// <summary>
/// This class represents a skill.
/// It implements the Slotable interface so it can be sloted in the SkillQuickAccess.
/// </summary>

public class Skill : NetworkBehaviour, Slotable
{
	[SerializeField] private int skillId;
	[SerializeField] private Effect[] effects;
	[SerializeField] private Sprite icon;
	[SerializeField] private int cost;
	[SerializeField] private int maxCharges;
	[SerializeField] private float cooldown;

	public void activate(Player player)
	{
		foreach (Effect effect in effects)
			{
				player.attachEffect(effect);
			}
	}

	public Sprite getIcon()
	{
		return icon;
	}

	public GameObject getInstance()
	{
		return this.gameObject;
	}

	public int Cost => cost;

	public int SkillId => skillId;


	public void changeCooldown(float factor)
    {
	    cooldown *= factor;
    }

	public float Cooldown => cooldown;

	public int MaxCharges => maxCharges;
}
