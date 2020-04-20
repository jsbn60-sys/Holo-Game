using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Effect that causes the player to go invisible and lose enemies.
/// IMPORTANT: CAN ONLY BE USED ON PLAYER UNIT!
/// </summary>
public class InvisibilityEffect : DurationEffect
{
	protected override void execEffect()
	{
		target.GetComponent<Player>().changeInvisibility(true);
	}

	protected override void turnOffEffect()
	{
		target.GetComponent<Player>().changeInvisibility(false);
	}

	// Start is called before the first frame update
	void Start()
    {
		base.Start();
    }

    // Update is called once per frame
    void Update()
    {
		base.Update();
    }
}
