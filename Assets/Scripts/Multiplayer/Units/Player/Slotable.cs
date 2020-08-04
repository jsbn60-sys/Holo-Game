using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for something that can be inserted into a slot.
/// Used by Item and Skill class.
/// </summary>
public interface Slotable {

	Sprite getIcon();
	void activate(Unit player);
	GameObject getInstance();
}
