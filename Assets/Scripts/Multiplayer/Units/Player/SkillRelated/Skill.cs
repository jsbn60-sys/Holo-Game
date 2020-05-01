using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class represents a skill.
/// It implements the Slotable interface so it can be sloted in the SkillQuickAccess.
/// </summary>
public class Skill : NetworkBehaviour, Slotable
{
	[SerializeField]
	private int skillId;
	[SerializeField]
	private Effect effect;
	[SerializeField]
	private Sprite icon;
	[SerializeField]
	private int cost;
	[SerializeField]
	private bool isActiveSkill;
	[SerializeField]
	private float cooldown;

	private bool isUnlocked;

	public void activate(Player player)
	{

	}

	public Sprite getIcon()
	{
		return icon;
	}

	public GameObject getInstance()
	{
		return this.gameObject;
	}

	public bool IsUnlocked
	{
		get => isUnlocked;
		set => isUnlocked = value;
	}

	public int Cost => cost;

	public int SkillId => skillId;

	// Start is called before the first frame update
	void Start()
	{
		isUnlocked = false;
	}

    // Update is called once per frame
    void Update()
    {

    }

    public void changeCooldown(float factor)
    {
	    cooldown *= factor;
    }
}
