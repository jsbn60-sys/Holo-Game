/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using UnityEngine;

///<summary>
/// This class implements the Speed item which the player can use to gain a huge speed advantage briefly
/// An icon will be displayed at the left side of the screen while the item is active 
/// This class calls a function in the PlayerController script which executes the effects of the item
///</summary>
public class SpeedBoost : Item
{

	public int duration; //Amount of time this item will last
	public GameObject icon;
	public float maxSpeed=14;
	public float speedBoostFactor=2; //Speed will increase and decrease by this factor

	public override void Ability(PlayerController player)
	{

		if (player.speed * speedBoostFactor <= maxSpeed)
		{
			player.speed *= speedBoostFactor;
			player.StartCoroutine(resetSpeed(player));
			icon = new GameObject();
			DisplayIcon(icon, new Vector3(30.5f, 81.5f, 0f));
		}

	}
	private IEnumerator resetSpeed(PlayerController player)
	{
		yield return new WaitForSeconds(duration);
		player.speed /= speedBoostFactor;
		Destroy(icon);
	}
}
