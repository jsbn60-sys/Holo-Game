/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using UnityEngine;


///<summary>
/// This class implements the Speed item which the player can use to gain a temporary speed advantage.
/// An icon will be displayed at the left side of the screen while the item is active 
/// This class calls a function in the PlayerController script which executes the effects of the item
///</summary>

public class Speed : Item
{
	public int duration; //Amount of time this item will last
	public GameObject icon;
	public float maxSpeed;
	public float speedBoostFactor = 1.5f; //speed of the player will be increased and decreased by this factor

	public override void Ability(PlayerController player)
	{
		if (player.speed * speedBoostFactor <= maxSpeed)
		{
			player.speed *= speedBoostFactor;
			player.StartCoroutine(resetSpeed(player));
			icon = new GameObject();
			DisplayIcon(icon, new Vector3(30.5f, 131.5f, 0f));
		}

	}

	private IEnumerator resetSpeed(PlayerController player)
	{
		//count to duration and add player to NPCs targets again to make them chase him again
		yield return new WaitForSeconds(duration);
		//isEffekt = false;
		player.speed /= speedBoostFactor;
		Debug.Log("resetSpeed: " + player.speed);
		Destroy(icon);
	}
}
