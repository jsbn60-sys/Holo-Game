using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostEffect : DurationEffect
{
	[SerializeField]
	private float speedBoostFactor;

	protected override void execEffect()
	{
		target.changeSpeed(true, speedBoostFactor);
	}

	protected override void turnOffEffect()
	{
		target.changeSpeed(false, speedBoostFactor);
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
