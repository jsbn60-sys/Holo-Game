using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// This class represents a skill.
/// It implements the Slotable interface so it can be sloted in the SkillQuickAccess.
/// </summary>
public abstract class Skill : NetworkBehaviour, Slotable
{
	public int skillId;
	public Effect effect;
	public Sprite icon;
	public int cost;
	public bool isActiveSkill;
	public int cooldown;

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

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
