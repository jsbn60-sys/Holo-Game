/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
	/// <summary>
	/// This Class represents a Healing Bullet that can be fired by the Gesundheit Prof
	/// </summary>
	public class HealingBullet : MonoBehaviour
	{
		public int baseHeal = 4;
		public int shieldValue = 4;
		public bool onHitHot = false;
		public int hot_heal_per_tick = 1; //heal per tick
		public int hot_ticks = 5; //number of ticks
		public int pierces = 0; // Number of times the Bullet pierces through an Enemy
		public bool speedBuff = false;
		public bool shieldBuff = false;
		public bool superBullet = false;
		public float speedyDuration = 0.5f;
		public float speedyValue = 2;

		
		///<summary>
		/// In the this method players heal their allies through their bullets.
		/// The amount of healing is determined by the values set in the prefab
		/// If the bullet doesn't hit an ally it will be destroyed.
		/// All special effects that depend on the healing bullet are also implemented here
		///</summary>	
		private void OnTriggerEnter(Collider collision)
		{
			var hit = collision.gameObject;
			if (hit.tag == "Player")
			{
				if (hit.GetComponent<Health>() != null)
				{
					var health = hit.GetComponent<Health>();
					if(shieldBuff && health.MAX_HEALTH == health.GetCurrentHealth() && !superBullet)
					{
						health.CmdShield(shieldValue);
					} else
					{
						health.CmdHeal(baseHeal);
					}
					if (speedBuff && !superBullet)
					{
						if (!hit.GetComponent<PlayerController>().speedy)
						{
							Speedy(hit.GetComponent<PlayerController>());
						}
					}
					if (onHitHot)
					{
						health.StartCoroutine(health.HealOverTime(hot_heal_per_tick, hot_ticks, 0.5f));
					}
					if (pierces == 0)
					{
						Destroy(gameObject);
					}
					else
					{
						pierces--;
					}

				}
			}
		}
		/// <summary>
		/// This function increases the movement speed of the player
		/// Gets called onHit if the healer (Gesundheit) has Skilled Speedboost
		/// The speedboost is not stackable
		/// </summary>
		public void Speedy(PlayerController player)
		{
			player.StartCoroutine(ResetSpeed(player));
			player.classSpeed += speedyValue;
			player.speed += speedyValue;
			player.speedy = true;
		}
		/// <summary>
		/// This function resets the movement speed of the player after he became "Speedy" from the Healer onHit effect
		/// </summary>
		private IEnumerator ResetSpeed(PlayerController player)
		{
			yield return new WaitForSecondsRealtime(speedyDuration);
			player.classSpeed -= speedyValue;
			player.speed -= speedyValue;
			player.speedy = false;
		}
	}
}
