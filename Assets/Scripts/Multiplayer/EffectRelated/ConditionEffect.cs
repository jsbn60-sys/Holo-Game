using System;
using UnityEngine;

/// <summary>
/// This class represents an effect which is active while a certain condition is met.
/// This condition has to be implemented by subclasses throug the isActive() method.
/// A good example for this is the ChangeAttackRateCondEffect that changes the attackRate
/// of a unit as long as it has a shield.
///
/// The abstract method execEffect is in this case to be understood as turning the effect on.
/// </summary>
public abstract class ConditionEffect : Effect
{
	private bool isRunning;

	/// <summary>
	/// Start is called before the first frame update
	/// </summary>
	public void Start()
	{
		isRunning = false;
	}

	/// <summary>
	/// Is updated every frame.
	/// execEffect is called once,
	/// after that it checks if the condition is still met.
	/// </summary>
	protected override void updateEffect()
	{
		if (!isRunning)
		{
			isRunning = true;
			execEffect();
		}
		else if (!isActive())
		{
			turnOffEffect();
			Destroy(gameObject);
		}
	}


	/// <summary>
	/// Needs to be implemented by subclasses to check if effect condition is still met.
	/// </summary>
	/// <returns>Is condition met</returns>
	protected abstract bool isActive();

	/// <summary>
	/// Needs to be implemented by subclasses to reverse effect.
	/// </summary>
	protected abstract void turnOffEffect();
}
