/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Multiplayer;
using UnityEngine.UI;

///<summary>
/// This class implements the DamageBoost item which the player can use to deal more damage over a certain amount of time.
/// An icon will be displayed at the left side of the screen while the item is active.
/// This class calls a function in the PlayerController script which executes the effects of the item.
///</summary>
public class DamageBoost : Item{
	public int duration; //Amount of time this item will last
	private GameObject icon;
	
	public override void Ability(PlayerController player){
		player.CmdCreateBoostBullet(true);
		player.StartCoroutine(ResetDamage(player));
		player.GetComponent<AudioManager>().PlaySound(player.transform.position,6);
		icon = new GameObject();
		DisplayIcon(icon, new Vector3(30.5f,431.5f,0f));		
	}

	private IEnumerator ResetDamage(PlayerController player){
		//count to duration
		yield return new WaitForSecondsRealtime(duration);
		player.CmdCreateBoostBullet(false);
		Destroy(icon);
	}
}
