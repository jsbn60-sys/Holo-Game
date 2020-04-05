/* edited by: SWT-P_SS_2019_Holo */
using System.Collections;
using UnityEngine;

namespace Multiplayer
{
	///<summary>
	/// This class manages the damage player inflict on NPCs when shooting them.
	/// It also handles special effects from Skills of the classes.
	/// The player can shoot the bullet which this script is attached to by clicking the left mouse button.
	/// If the bullet hits a NPC, the NPC will take damage and the bullet will disappear. The bullet also disappears when it hits a building.
	/// The overall amount of damage can be manipulated by changing the value of field mul.
	/// </summary>
	public class Bullet : MonoBehaviour
	{
		public float mul = 1.0f;
		public int basedamage = 10;
		public bool dot_Effect = false;
		public int dot_dmg = 2;
		public int dot_amount = 20;
		public int pierces = 0; // Number of times the Bullet pierces thorugh an Enemy
		public bool dotSkill = false;
		public bool wrath = false;
		public bool freezeShot = false;

		public float standardSpeed = 7;
		public float freezeTime = 0.25f;

		//the Player that shot the bullet
		private PlayerController sourcePlayer;
		int cdrValPiercingHealBullet = 1;
		int cdrValShieldItemDrop = 1;
		public void SetSourcePlayer(PlayerController sourcePlayer)
		{
			this.sourcePlayer = sourcePlayer;
		}

		///<summary>
		/// This function handles damage and special effects like dots/piercing shots/cooldown reduction onHit.
		/// It applies the damage via function calls of the Health script.
		/// If the bullet doesn't hit an enemy or other object besides the excluded ones it will be destroyed.
		///</summary>	
		private void OnTriggerEnter(Collider collision)
		{
			var hit = collision.gameObject;
			if (hit.tag != "Slowfield" && hit.tag != "Player" && hit.tag != "Item" && hit.name != "Sphere" && hit.tag != "Stuncone")
			{

				if (freezeShot && hit.tag == "Enemy")
				{
					sourcePlayer.CmdFreeze(hit);
					sourcePlayer.CmdFreezeReset(hit);
				}
				if (hit.tag.Equals("Enemy"))
				{
					if (sourcePlayer != null)
					{
						if (sourcePlayer.piercingHealBullet)
						{
							sourcePlayer.PiercingHealBulletReduceCooldown(cdrValPiercingHealBullet);
						}
						if (sourcePlayer.shieldItemDropCDRonHit)
						{
							sourcePlayer.ShieldItemDropReduceCooldown(cdrValShieldItemDrop);
						}
					}
				}

				if (hit.GetComponent<Health>() && hit.GetComponent<NPC.NPC>() != null)
				{
					short type = hit.GetComponent<NPC.NPC>().type;
					var health = hit.GetComponent<Health>();
					health.TakeDamage((int)(basedamage * mul));

					if (dot_Effect || dotSkill)
					{
						health.StartCoroutine(health.DamageOverTime(dot_dmg, dot_amount));
					}
					if (pierces == 0)
					{
						Destroy(gameObject);
					} else
					{
						pierces--;
						if (!dotSkill)
							dot_Effect = false; // dont apply dot Effects after first Target, except it is during Skill Time
					}
				} else
				{
					if (!(wrath && hit.tag == "Player" || hit.tag == "Plane"))
					{
						Destroy(gameObject);
					}
				}
			}
		}

		private IEnumerator ResetSpeed(GameObject enemy)
		{
			yield return new WaitForSecondsRealtime(freezeTime);
			if (enemy != null)
			{
				enemy.GetComponent<NPC.NPC>().move.agent.speed = standardSpeed;
			}
		}
	}
}
