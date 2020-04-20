using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : Effect
{
	[SerializeField]
	private float shieldAmount;

	public override void execEffect()
	{
		target.giveShield(shieldAmount);
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
