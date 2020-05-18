using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple dummy unit, that will be targeted by npcs in range.
/// A dummy may have a auraEffect it will attach on it self.
/// </summary>
public class Dummy : Unit
{
	[SerializeField] private GameObject attachAuraEffect;
	/// <summary>
	/// Destroys itself on death.
	/// </summary>
	protected override void onDeath()
    {
	    Destroy(gameObject);
    }

	/// <summary>
	/// Start is called before the first frame update.
	/// Attaches the aura effect, if the dummy has one.
	/// </summary>
	public void Start()
	{
		base.Start();
		if (attachAuraEffect != null)
		{
			attachEffect(attachAuraEffect.GetComponent<Effect>());
		}
	}
}
