using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	[SerializeField] private string skillName;
	[TextArea(3, 10)] [SerializeField] private string popupContent;
	[TextArea(3, 10)] [SerializeField] private string effectsList;

	public string SkillName => skillName;

	public string PopupContent => popupContent;

	public string EffectsList => effectsList;

	public void activate(Unit player)
	{
		player.getHit(0, effects.ToList());
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

	public float Cooldown => cooldown;

	public int MaxCharges => maxCharges;
}
