using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents an effect which increases the units damage for a certain duration.
/// </summary>
public class DamageBoostEffect : DurationEffect
{
	[SerializeField]
	private float dmgBoostAmount;

	protected override void execEffect()
	{
		target.getAttack().increaseDmg(dmgBoostAmount);
	}

	protected override void turnOffEffect()
	{
		target.getAttack().decreaseDmg(dmgBoostAmount);
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
