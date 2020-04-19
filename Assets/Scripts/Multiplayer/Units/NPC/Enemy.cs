using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a basic enemy in terms of basic attributes and interaction.
/// The actual KI for the NPC behaviour is implemented in the NPCController/NPC class.
/// </summary>
public class Enemy : Unit
{
	// Start is called before the first frame update
	void Start()
	{
		attackRate = 1f;
		jumpForce = 55.0f;
		speed = 12f;

		base.Start();
    }

    // Update is called once per frame
    void Update()
    {
		base.Update();
    }

	// drops item and destroys enemy
	protected override void onDeath()
	{
		StartCoroutine(ItemDrop.instance.spawnItem(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z, false));
		Destroy(gameObject);
	}

	// interface function called by NPCAttack to attack.
	public void hit(Unit target) {
		if (canAttack())
		{
			attack.onHit(target);
			base.useAttack();
		}
	}
}
