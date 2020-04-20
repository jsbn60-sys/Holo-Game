using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffect : Effect
{
	[SerializeField]
	private float healAmount;

	public override void execEffect()
	{
		target.heal(healAmount);
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
