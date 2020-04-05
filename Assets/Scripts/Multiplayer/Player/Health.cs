/* edited by: SWT-P_SS_2019_Holo */
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Multiplayer
{
	///<summary>
	/// This class manages the health of players and NPCs.
	/// </summary>
	public class Health : NetworkBehaviour
	{
		public int MAX_HEALTH = 100;
		public int MAX_SHIELD = 60;
		[SyncVar(hook = "OnChangeHealth")]
		public int currentHealth;
		[SyncVar(hook = "OnChangeShield")]
		public int currentShield;
		public RectTransform healthBar;
		public RectTransform shieldBar;
		public bool destroyOnDeath;
		private NetworkStartPosition[] spawnPoints;
		private bool vulnerable = true; //only take damage if vulnerable
		private bool dot_isActive = false;
		public bool explosiveShield = false;
		public bool pushbackShield = false;
		public bool buffedShield = false;
		public bool tauntDmgReduce = false;

		/// <summary>
		/// In this function the health script gets initialized
		/// Shield and Healthbars are updated to the set MAX values of the prefab
		/// </summary>
		private void Start()
		{
			currentHealth = MAX_HEALTH;
			
			if (this.gameObject.tag.Equals("Player"))
			{
				UpdateShieldbarSize();
				currentShield = 0;
			}
			UpdateHealthbarSize();
			if (this.gameObject.tag.Equals("Player"))
			{
				OnChangeShield(0);
			}
			OnChangeHealth(MAX_HEALTH);
			if (isLocalPlayer)
			{
				spawnPoints = FindObjectsOfType<NetworkStartPosition>();

			}
		}

		
		private IEnumerator CheckHot()
		{
			yield return new WaitForSecondsRealtime(1);

		}

		/// <summary>
		/// This function reduces the current shield/health
		/// It also triggers some class effects that happen on shield breaking
		/// Additionaly it handles what happens to the object if the health drops to 0
		/// </summary>
		/// <param name="amount">Amount of damage that will be dealt</param>
		public void TakeDamage(int amount)
		{
			if (!isServer)
			{
				return;
			}
			if (tauntDmgReduce)
			{
				amount /= 2;
			}
			if (vulnerable) //only take damage if vulnerable
			{
				if(currentShield <= 0)
				{
					currentHealth -= amount;
				} else
				{
					if(currentShield - amount <= 0) //If the shield is broken by the damage, the excessive damage is dealt to the health
					{
						PlayerController player = GetComponent<PlayerController>();
						if (explosiveShield)
						{
							player.CmdShieldExplode();
						}
						if (pushbackShield)
						{
							player.CmdShieldPush();
						}
						if (buffedShield)
						{
							player.CmdShieldBuff(false);
						}
						currentHealth = currentHealth - (currentShield - amount);
						currentShield = 0;
					} else
					{
						currentShield = currentShield - amount;
					}
				}
				
			}
			if (currentHealth > 0) return;
			if (destroyOnDeath)
			{
				StartCoroutine(ItemDrop.instance.spawnItem(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z,false));

				Destroy(gameObject);
			}
		}

		/// <summary>
		/// Set's the vulnerable flag.
		/// TakeDamage() will not deal damage if set to true.
		/// </summary>
		public void SetVulnerability(bool b)
		{
			vulnerable = b;
		}

		public bool IsZero()
		{
			return currentHealth <= 0;
		}

		/// <summary>
		/// This method increases the player's health.
		/// </summary>
		///<param name="health">amount of health to heal</param>
		public void Heal(int health)
		{
			if (!isServer) return;
			//Add wanted healing amount to currentHealth. If it exceeds MAX_HEALTH, heal to MAX_HEALTH
			if (currentHealth + health >= MAX_HEALTH)
			{
				currentHealth = MAX_HEALTH;
			}
			else
			{
				currentHealth += health;
			}
		}

		[Command]
		public void CmdHeal(int hp)
		{
			Heal(hp);
		}

		[Command]
		public void CmdShield(int hp)
		{
			Shield(hp);
		}
		/// <summary>
		/// This methos adds an amount of shield to the players current shield
		/// </summary>
		/// <param name="shield">value of the shield</param>
		public void Shield(int shield)
		{
			if (!isServer) return;
			if(currentShield + shield >= MAX_SHIELD)
			{
				currentShield = MAX_SHIELD;
			} else
			{
				currentShield += shield;
			}
			if (buffedShield)
			{
				PlayerController player = GetComponent<PlayerController>();
				player.CmdShieldBuff(true);
			}
		}


		/// <summary>
		/// Set player's health to MAX_HEALTH
		/// </summary>
		public void Recover()
		{
			CmdHeal(MAX_HEALTH);
		}

		/// <summary>
		/// Set player's health to MAX_HEALTH in synchronization with the server.
		/// Used by the HealPotion item.
		/// </summary>
		[Command]
		public void CmdhealFull()
		{
			currentHealth = MAX_HEALTH;
		}

		/// <summary>
		/// This method calls the Ignited function repeatedly to deal damage over time.
		/// </summary>
		public void Ignite(){
			StartCoroutine("ResetIgnite");
			InvokeRepeating("Ignited", 0.0f, 1.0f);
		}

		/// <summary>
		/// This method stops the repeated Ignite damage after 10 seconds.
		/// </summary>
		IEnumerator ResetIgnite(){
			yield return new WaitForSecondsRealtime(10.0f);
			CancelInvoke();
		}

		/// <summary>
		/// This method decreases the health by 6.
		/// It is called by the Ignite method.
		/// </summary>
		void Ignited(){
			TakeDamage(6);
		}

		public int GetCurrentHealth()
		{
			return currentHealth;
		}

		/// <summary>
		/// This method updates the amount of health the player currently has
		/// </summary>
		/// <param name="currentHealth">New amount of health the player has</param>
		private void OnChangeHealth(int currentHealth)
		{
			healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
		}

		/// <summary>
		/// This method updates the amount of shield the player currently has
		/// </summary>
		/// <param name="currentShield">New amount of shield the player has</param>
		private void OnChangeShield(int currentShield)
		{
			shieldBar.sizeDelta = new Vector2(currentShield, healthBar.sizeDelta.y);
		}

		/// <summary>
		/// This function updates the healthbar size(red background part, so the bar updates to the starting MAX_HP as max length)
		/// </summary>
		public void UpdateHealthbarSize()
		{
			RectTransform redHealthTransform = healthBar.GetComponentInParent<RectTransform>().parent.GetComponent<RectTransform>();
			redHealthTransform.sizeDelta = new Vector2(MAX_HEALTH, redHealthTransform.sizeDelta.y);
		}

		/// <summary>
		/// This function updates the shieldBar size(blue foreground part, so the bar updates to the starting MAX_SHIELD as max length)
		/// </summary>
		private void UpdateShieldbarSize()
		{
			shieldBar = healthBar.GetChild(0).GetComponent<RectTransform>();
			shieldBar.sizeDelta = new Vector2(MAX_SHIELD, shieldBar.sizeDelta.y);
		}

		

		[ClientRpc]
		private void RpcRespawn()
		{
			if (!isLocalPlayer) return;
			// Set the spawn point to origin as a default value
			Vector3 spawnPoint = Vector3.zero;

			// If there is a spawn point array and the array is not empty, pick a spawn point at random
			if (spawnPoints != null && spawnPoints.Length > 0)
			{
				spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
			}

			// Set the player’s position to the chosen spawn point
			transform.position = spawnPoint;
		}

		public void SetHealth() {
			currentHealth = MAX_HEALTH;
		}

		/// <summary>
		/// Represents a Damage over Time Effect on health.
		/// Ticks every second
		/// </summary>
		/// <param name="dmg">dmg done per Tick</param>
		/// <param name="amount">number of Ticks</param>
		/// <returns></returns>
		public IEnumerator DamageOverTime(int dmg, int amount)
		{
			if (!dot_isActive)
			{
				dot_isActive = true;
				for (int i = 1; i <= amount; i++)
				{
					yield return new WaitForSecondsRealtime(0.5f); // wait for half a second
					TakeDamage(dmg);
				}
				dot_isActive = false;
			}
		}

		/// <summary>
		/// Represents a Heal over Time effect on health
		/// </summary>
		/// <param name="heal_per_tick">healing done per tick</param>
		/// <param name="number_of_ticks">number of ticks</param>
		/// <param name="time_between_ticks">time between ticks in seconds</param>
		/// <returns></returns>
		public IEnumerator HealOverTime(int heal_per_tick, int number_of_ticks, float time_between_ticks)
		{
			for (int i = 1; i <= number_of_ticks; i++)
			{
				yield return new WaitForSecondsRealtime(time_between_ticks);
				CmdHeal(heal_per_tick);
			}
		}

		/// <summary>
		/// Represents a Shield over Time effect on health
		/// </summary>
		/// <param name="shield_per_tick">shield added per tick</param>
		/// <param name="number_of_ticks">number of ticks</param>
		/// <param name="time_between_ticks">time between ticks in seconds</param>
		/// <returns></returns>
		public IEnumerator ShieldOverTime(int shield_per_tick, int number_of_ticks, float time_between_ticks)
		{
			for (int i = 1; i <= number_of_ticks; i++)
			{
				yield return new WaitForSecondsRealtime(time_between_ticks); 
				CmdShield(shield_per_tick);
			}
		}
	}
}
