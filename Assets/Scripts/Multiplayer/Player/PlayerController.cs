/* edited by: SWT-P_SS_2019_Holo */
using Multiplayer;
using NPC;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


///<summary>
/// In this class players and their actions are processed. 
/// These actions include using the inventory, the pause menu, usage of items, picking items up and player movement.
/// Also the script synchronizes the Player with the server.
/// The Health of players is not handled in this script but rather in the seperate Health script.
/// When a player dies, spectating mode is initiated.
/// The movement speed of a player can be changed by changing public variable speed to another float value. It is set to 3 by default.
///</summary>
public class PlayerController : NetworkBehaviour
{
	public short role;
	///<param name="playerName"> name chosen by player in the lobby. The name is displayed over the players head in game</param>
	[SyncVar]
	public string playerName;
	private GameObject mainCamera;
	public Multiplayer.Health health;
	public GameObject inventory;
	public GameObject quickAccessBar;
	public GameObject skillSlots;
	public GameObject menu;
	public GameObject gameOverMenu;
	public GameObject tutorialMenu;
	public GameObject helpButton;
	private Collider collectitems;

	[SyncVar(hook = "OnBurnout")]
	private bool isBurnedOut = false;

	// for shooting || base Values are Overwriten by Prefab
	private GameObject bullet;
	public GameObject bulletPrefab;
	public GameObject bulletBoost;
	public Transform bulletSpawn;
	public GameObject arm;
	private float charge;
	private bool isBulletBoost = false;
	private float nextShot = 0.0f;
	public float classFireRate;
	public float fireRate;
	public float bulletSize = 0.3f;
	public int bulletSpeed = 5;
	public float bulletRange = 2f;
	public int baseDmg = 10;

	// for Distraction item
	private GameObject dummy;
	private GameObject[] npcs;

	private string material = "";
	private string stunParty = "";
	public Material invisibleColor; // for Invisibility item
	public Material rainbow; // for Invulnerability item
	public Material normal; // standard material
	public Material strongMaterial; // for strong shot
	public Material strongHealingBulletMaterial; //For Active Big Heal Bullet

	// for speedItem
	public float classSpeed;
	public float speed; //should not be set
	private int speedTimeCounter;
	private GameObject speedEffektClone;
	public GameObject speedEffekt;

	// for teleporter
	public GameObject portEffektPrefab;
	private GameObject portEffektCloneStart;
	private GameObject portEffektCloneEnd;

	// for detonating items
	public GameObject explosion;

	// for slowItem
	[SyncVar]
	public GameObject slowbullet;
	public GameObject slowfield;

	//ChatVar
	GameObject chatObject;

	//forCamera
	public GameObject target;
	float vertical, horizontal;
	public float rotateSpeed = 1.0f;

	// for SkillMenu
	public SkillMenu skillMenu;
	public GameObject map; // MiniMap
	public const int NUM_SKILLS = 10;
	public Skill[] skills = new Skill[NUM_SKILLS];
	public int investedSkillPoints = 0;

	// MNI SkillTree vars
	public float multiplier = 1.0f; // Basic Damage Multiplier
	public float boostDmg = 1.2f;
	public bool dotEffectActive = false;
	public int dotMultiplier = 1;
	private bool applyDots = false;
	public bool coolDownSkill = false;
	public int numberPierces = 0;
	public bool activeDotSkill = false;
	public int critchance = 0;
	public bool canCrit = false;
	[SyncVar]
	private bool mni_skill1IsActive;

	private bool wrath = false;
	private float wrathDuration = 0.0f;
	private float wrathRadius = 0.0f;
	public GameObject wrathIndicator;

	[SyncVar]
	private int matKey;
	public Material MNI_skill1mat;
	public Material MNI_skill3mat;
	public Material dmgBoostItemMat;
	public Material standartBulletMat;

	// GES SkillTree vars
	[SyncVar]
	public bool speedy = false; //variable to check if the target hit already has an active speed buff
	public bool healingMode = false;
	public GameObject healingBullet;
	public bool permaHealingMode = false; //If true the healer main skill has no cooldown
	public bool onHitShield = false; //If true the healers healing bullets apply a shield on the player hit, if he is already at full hp
	public bool onHitSpeedBuff = false; //If true the healers healing bullets apply a short speed buff
	public bool onHitHot = false; // IF f true the healers healing bullets apply a hot (heal over time)
	public bool piercingHealBullet = false; //If true the healers skill Piercing Heal Bullets Cooldown gets reduced on Hit
	public int cdPiercingHealBullet = 60; //Actual CD that gets reduced
	public bool shieldItemDrop = false;
	public bool shieldItemDropCDRonHit = false;
	public int cdShieldItemDrop = 60;
	public GameObject shieldItem;
	public int shieldItemDropDuration = 120; //time in seconds the shield will remain on the ground
	public bool applyHotsOnShieldItemPickup = false;
	public bool coffeemachine = false;
	public GameObject coffeeMachineObject;
	public GameObject coffeeMachineIndicator;
	public int coffeeMachineHealPerTick = 8;


	//tank vars
	public Material tankMat;
	public Material tauntMat;
	public Material invincibleMat;
	public GameObject stunConeObject;
	public bool isInvincible = false;
	public int explosiveShieldDamage = 40;
	public bool tauntDmgReduce = false;
	public bool buffedAttack = false;
	public bool shieldBuff = false;
	public int baseDamageIncrease = 1;
	public float fireRateIncrease = 0.1f;
	private bool buffedTank = false;


	// Support Skilltree vars
	[SyncVar]
	public GameObject damageBoostItem;
	public GameObject supportBullet;
	public GameObject WarpZone;
	public bool hasWarpZoneBuff = false;
	public bool isSlowAreaIncreased = false;
	public bool isSlowEffectIncreased = false;
	public float slowMultiplier = 0.6f;

	public GameObject auraIndicator;
	public bool isDmgAuraActve = false;
	public bool dmgSet = false;

	private bool auraActive = false;

	public bool isAtckSpeedAuraActive = false;
	public bool attackSpeedSet = false;
	public bool isCRSkilled = false;
	public float cooldownReduction = 0.7f;
	public bool freezeShot = false;
	public GameObject bulletArea;
	public GameObject bulletAreaEffect;
	public GameObject bulletEffect;

	public GameObject AuraTest;

	[SyncVar]
	public GameObject slowFieldAreaIncreased;
	[SyncVar]
	public GameObject slowFieldEffectIncreased;
	[SyncVar]
	public GameObject slowFieldAreaEffectIncreased;

	public float fireRateMult = 0.9f;
	public int dmgIncrease = 2;

	private bool airCollision = false;
	private bool isGrounded = true;
	public float jumpForce = 55.0f;
	public float bonusGravity = 60.0f;

	void Start()
	{
		this.name = playerName;
		CmdSendNameToServer(playerName);
		gameObject.transform.Find("Name Canvas ").transform.Find("Text").GetComponent<Text>().text = playerName;

		chatObject = GameObject.Find("Chat");

		SetUpSkillTree();

		// adds first skill into quickslot at start
		skillMenu.StartSkill();

		map = GameObject.FindGameObjectWithTag("Map");

		ClientScene.RegisterPrefab(speedEffekt);
		ClientScene.RegisterPrefab(portEffektPrefab);
		ClientScene.RegisterPrefab(explosion);
		ClientScene.RegisterPrefab(slowbullet);
		ClientScene.RegisterPrefab(slowfield);
		ClientScene.RegisterPrefab(supportBullet);
		ClientScene.RegisterPrefab(slowFieldAreaIncreased);
		ClientScene.RegisterPrefab(slowFieldEffectIncreased);
		ClientScene.RegisterPrefab(slowFieldAreaEffectIncreased);
		ClientScene.RegisterPrefab(WarpZone);

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		speedTimeCounter = 0;

		speed = classSpeed;
		fireRate = classFireRate;
		canCrit = false;
		healingMode = false;
		numberPierces = 0;
	}

	public void CmdCloseMenu()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		menu.SetActive(false);
	}

	/// <summary>
	/// updates airCollision and isGrounded variable
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag.Equals("Plane"))
		{
			isGrounded = true;
		}
		airCollision = !isGrounded;
	}
	/// <summary>
	/// updates isGrounded variable
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag.Equals("Plane"))
		{
			isGrounded = false;
		}
	}

	public void FixedUpdate()
	{
		if (isServer)
		{
			if (health.IsZero() && !isBurnedOut)
			{
				//prof is burned out remove him as target
				speed = 0;
				gameObject.GetComponent<Renderer>().material = normal;
				NPCManager.Instance.RemoveTarget(gameObject.transform);
				GameOverManager.Instance.ProfIsBurnedOut(this);
				isBurnedOut = true;
			}
			if (!health.IsZero() && isBurnedOut)
			{
				//prof is well, register him as target
				isBurnedOut = false;
				NPCManager.Instance.AddTarget(this.transform);
				GameOverManager.Instance.ProfIsFine(this);
			}
		}

		if (!isLocalPlayer)
		{
			return;
		}

		if (isBurnedOut)
		{
			return;
		}

		// player movement
		float z;
		float x;

		horizontal += Input.GetAxis("Mouse X") * rotateSpeed;
		vertical -= Input.GetAxis("Mouse Y") * rotateSpeed;
		vertical = Mathf.Clamp(vertical, -12, 60);

		z = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;
		x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;

		// jump by applying force
		if (Input.GetKey(KeyCode.Space) && isGrounded && GetComponent<Rigidbody>().velocity.y < 5.0f ) 
		{
			GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		}
		// additional gravity for better jump
		if (!isGrounded)
		{
			Vector3 vel = GetComponent<Rigidbody>().velocity;
			vel.y -= bonusGravity * Time.deltaTime;
			GetComponent<Rigidbody>().velocity = vel;

			// bounce player off wall when in air
			if (airCollision)
			{
				z = -0.5f * z;
				x = -0.5f * x;
			}
		}


		target.transform.rotation = Quaternion.Euler(vertical, horizontal, 0);
		transform.rotation = Quaternion.Euler(0, horizontal, 0);
		GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(0, 0, z));
		GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(x, 0, 0));

		if (speed > 6 && speedTimeCounter >= 15)
		{
			speedTimeCounter = 0;
			CmdCreatespeedEffekt();
		}

		if (!chatObject.GetComponent<Multiplayer.Lobby.ChatController>().chatBox.isFocused)
		{
			if (x == 0 || z == 0)
			{
				transform.Translate(x, 0, z);
			}
			else
			{
				transform.Translate(x * 0.65f, 0, z * 0.65f);
			}
		}
	}

	///<summary>
	/// The Update function processes all the inputs the player does while playing.
	/// The player uses 'W', 'A', 'S', 'D' to move, 'I' to open inventory, left mouse button to shoot, escape for menu, F1 for tutorial and number keys to use items.
	/// </summary>
	public void Update()
	{

		if (isServer)
		{
			if (health.IsZero() && !isBurnedOut)
			{
				//prof is burned out remove him as target
				speed = 0;
				gameObject.GetComponent<Renderer>().material = normal;
				NPCManager.Instance.RemoveTarget(gameObject.transform);
				GameOverManager.Instance.ProfIsBurnedOut(this);
				isBurnedOut = true;
			}
			if (!health.IsZero() && isBurnedOut)
			{
				//prof is well, register him as target
				isBurnedOut = false;
				NPCManager.Instance.AddTarget(this.transform);
				GameOverManager.Instance.ProfIsFine(this);
			}
		}

		if (!isLocalPlayer)
		{
			return;
		}

		if (isBurnedOut)
		{
			return;
		}

		chatObject.GetComponent<Multiplayer.Lobby.ChatController>().HandleChatSelection();

		//player movement transfered to FixedUpdate


		// for oneHit item and for WrathOfTheDean Skill
		if (Input.GetMouseButtonDown(0))
		{
			if (gameObject.transform.GetChild(5).transform.GetChild(3).gameObject.active == false)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit))
				{
					// Wrath of the Dean
					if (wrath)
					{
						if (Cursor.visible)
						{
							float xPos = hit.point.x;
							float zPos = hit.point.z;
							gameObject.transform.GetComponent<AudioManager>().PlaySound(transform.position, 11);
							CmdSpawnBulletsWrath(xPos, zPos);
							wrath = false;
							Cursor.visible = false;
							Cursor.lockState = CursorLockMode.Locked;
						}
					}

					// one hit uitem
					if (hit.collider.tag == "Enemy" & !inventory.activeSelf)
					{
						if (Cursor.visible)
						{
							CmdDestroyNPC(hit.collider.gameObject.GetComponent<NetworkIdentity>().netId);
							gameObject.GetComponent<AudioManager>().PlaySound(transform.position, 7);
							Cursor.visible = false;
							Cursor.lockState = CursorLockMode.Locked;
						}

					}
				}
			}
		}

		// for shooting
		if (Input.GetKey(KeyCode.Mouse0) && Time.time > nextShot && !wrath)

		{
			if (!Cursor.visible && !inventory.activeSelf)
			{
				nextShot = Time.time + fireRate;
				CmdFire();
				gameObject.transform.GetComponent<AudioManager>().PlaySound(transform.position, 0);
			}
		}

		// open Skills Menu
		if (Input.GetKeyDown(KeyCode.Tab) && !chatObject.GetComponent<Multiplayer.Lobby.ChatController>().chatBox.isFocused)
		{
			if (skillMenu.menu.activeSelf == true)
			{
				map.SetActive(true);
				chatObject.SetActive(true);
				quickAccessBar.SetActive(true);
				skillSlots.SetActive(true);
				skillMenu.menu.SetActive(false);
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			else
			{
				CmdCloseMenu();
				skillMenu.DeactivatePopUps();
				map.SetActive(false);
				chatObject.SetActive(false);
				inventory.SetActive(false);
				quickAccessBar.SetActive(false);
				skillSlots.SetActive(false);
				skillMenu.menu.SetActive(true);
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		// open menu
		if (Input.GetKeyDown(KeyCode.Escape) && !chatObject.GetComponent<Multiplayer.Lobby.ChatController>().chatBox.isFocused && !gameOverMenu.activeSelf)
		{
			if (menu.activeSelf == false)
			{

				skillMenu.menu.SetActive(false);
				map.SetActive(true);
				chatObject.SetActive(true);
				menu.SetActive(true);
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			else
			{
				CmdCloseMenu();
			}
		}

		// open tutorial
		if (Input.GetKeyDown(KeyCode.F1) && !chatObject.GetComponent<Multiplayer.Lobby.ChatController>().chatBox.isFocused && !gameOverMenu.activeSelf && !menu.activeSelf && !skillMenu.menu.activeSelf)
		{
			if (!tutorialMenu.activeSelf)
			{
				tutorialMenu.SetActive(true);
				quickAccessBar.SetActive(false);
				skillSlots.SetActive(false);
				map.SetActive(false);
				chatObject.SetActive(false);
				helpButton.SetActive(false);
			}	
			else
			{
				tutorialMenu.SetActive(false);
				quickAccessBar.SetActive(true);
				skillSlots.SetActive(true);
				map.SetActive(true);
				chatObject.SetActive(true);
				helpButton.SetActive(true);
			}
		}

		// use items
		for (int i = 1; i <= 5; i++) // to address slots 1-5
		{
			if (Input.GetKeyDown("" + i) && !chatObject.GetComponent<Multiplayer.Lobby.ChatController>().chatBox.isFocused)
			{
				Transform slot = gameObject.GetComponentInChildren<QuickAccessBar>().transform.GetChild(i - 1);
				if (slot.childCount == 2)
				{
					slot.GetChild(1).GetComponent<InventoryItem>().item.Ability(this); // call item ability
					Destroy(slot.GetChild(1).gameObject);
				}
			}
		}

		// open/close inventory
		if (Input.GetKeyDown(KeyCode.I) && !chatObject.GetComponent<Multiplayer.Lobby.ChatController>().chatBox.isFocused)
		{
			if (inventory.activeSelf == false)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				inventory.SetActive(true);
			}
			else
			{
				inventory.SetActive(false);
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
		}

		// Used to activate Skills: skills used with right Klick, q and e

		// primary Skill used with Right Klick
		if (Input.GetMouseButtonDown(1) && !chatObject.GetComponent<Multiplayer.Lobby.ChatController>().chatBox.isFocused)
		{
			Transform slot = gameObject.GetComponentInChildren<SkillSlots>().transform.GetChild(0);
			Skill skill;
			if (slot.childCount == 2)
			{
				skill = slot.GetChild(1).GetComponent<SkillBarItem>().skill;
				skill.Ability(this); //call skill ability
			}

		}



		// skill in slot Q
		if (Input.GetKeyDown(KeyCode.Q) && !chatObject.GetComponent<Multiplayer.Lobby.ChatController>().chatBox.isFocused)
		{
			Transform slot = gameObject.GetComponentInChildren<SkillSlots>().transform.GetChild(1);
			Skill skill;
			if (slot.childCount == 2)
			{
				skill = slot.GetChild(1).GetComponent<SkillBarItem>().skill;
				skill.Ability(this); // call skill ability
			}

		}

		// skill in slot E
		if (Input.GetKeyDown(KeyCode.E) && !chatObject.GetComponent<Multiplayer.Lobby.ChatController>().chatBox.isFocused)
		{
			Transform slot = gameObject.GetComponentInChildren<SkillSlots>().transform.GetChild(2);
			Skill skill;
			if (slot.childCount == 2)
			{
				skill = slot.GetChild(1).GetComponent<SkillBarItem>().skill;
				skill.Ability(this); // call skill ability
			}

		}

	}

	public override void OnStartLocalPlayer()
	{
		StartCoroutine(UpdateCameraTarget(this));
		quickAccessBar.SetActive(true);
		skillSlots.SetActive(true);
		inventory.SetActive(false);
	}


	public override void OnStartServer()
	{
		StartCoroutine(RegisterAtManagers(this));
		base.OnStartServer();
	}

	private static IEnumerator RegisterAtManagers(PlayerController player)
	{
		yield return null;
		NPCManager.Instance.AddTarget(player.transform);
		GameOverManager.Instance.AddProf(player);
	}


	public static IEnumerator UpdateCameraTarget(PlayerController player)
	{
		yield return null;
		//keep main camera for later specation options
		player.mainCamera = GameObject.FindGameObjectWithTag("MapCamera");
		player.mainCamera.GetComponent<CameraFollow>().player = player.transform;
		player.mainCamera.GetComponent<CameraFollow>().target = player.target.transform;
		player.mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		player.mainCamera.GetComponent<CameraFollow>().player = player.transform;
		player.mainCamera.GetComponent<CameraFollow>().target = player.target.transform;
	}

	public void Recover()
	{
		health.Recover();
	}

	///<summary>
	/// This method is called after a player loses all their health. It gives the player the option to spectate other players
	///</summary>
	private void OnBurnout(bool isBurnedOut)
	{

		this.isBurnedOut = isBurnedOut;
		if (isLocalPlayer && mainCamera != null)
		{
			if (isBurnedOut)
			{

				//deactivate quick access bar and inventory
				quickAccessBar.SetActive(false);
				skillSlots.SetActive(false);
				inventory.SetActive(false);
				skillMenu.menu.SetActive(false);

				//activate cursor for spectating
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;

				mainCamera.GetComponent<SpectateOptions>().StartSpecating();
			}
			else
			{
				mainCamera.GetComponent<SpectateOptions>().StopSpecating();
			}
		}
	}

	private void OnDestroy()
	{
		if (isServer)
		{
			NPCManager.Instance.RemoveTarget(this.transform);
			GameOverManager.Instance.RemoveProf(this);
		}
	}


	///<summary>
	/// This method enables the player to collect items and adds them to the player quick access bar or inventory.
	///</summary>
	///<param name="collider">The gameObject that collided with the playerc
	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.GetComponent<Item>() != null)
		{
			collectitems = collider;
			CmdCollectItems(true);
			if (!quickAccessBar.transform.GetComponent<QuickAccessBar>().IsFull())
			{
				//  if the quick access bar is not full, collect ssitem in quickaccessbar
				quickAccessBar.transform.GetComponent<QuickAccessBar>().AddItem(collider.GetComponent<Item>());

			}
			else if (!inventory.transform.GetChild(0).GetComponent<Inventory>().IsFull())
			{
				// collect item in inventory
				inventory.transform.GetChild(0).GetComponent<Inventory>().AddItem(collider.GetComponent<Item>());
			}
			else
			{
				// inventory and quick access bar are full
				// item is not collected
				CmdCollectItems(false);
				collider.gameObject.SetActive(true);
				collider.enabled = true;
			}
		}

	}

	private void CmdCollectItems(bool isActive)
	{
		if (isActive == true)
		{
			collectitems.enabled = false;
			collectitems.gameObject.SetActive(false);
		}
		else
		{
			collectitems.gameObject.SetActive(true);
			collectitems.enabled = true;
		}

	}


	///<summary>
	/// The CmdFire method is called when the player released the left mouse button to fire.
	/// It will instantiate a bullet. The bullet will either be normal or a damageBoost bullet, which can be unlocked temporarily by using the damageBoost item (exam).
	/// The bullet will fly slightly straight
	/// The damage dealt to the NPC is handled in the seperate bullet script.
	/// The bullets range and speed are determined by the Class of the player
	///</summary>
	[Command]
	private void CmdFire()
	{
		RpcClientFire();
	}

	[ClientRpc]
	private void RpcClientFire()
	{
		//Force bulletSize to be >= 0.3f and <=0.8f
		//float bulletSize = Mathf.Clamp(charge, 0.3f, 0.8f);
		//charge = Mathf.Clamp(charge, 0, 1.8f);

		// Create the bullet from the bullet prefab

		//healer class only
		if (healingMode)
		{
			bullet = Instantiate(healingBullet, bulletSpawn.position, arm.transform.rotation * Quaternion.Euler(0, 90f, 0));
			bullet.GetComponent<Rigidbody>().velocity = (mainCamera.transform.forward + new Vector3(0, 0.25f, 0)) * 3 * bulletSpeed;
		}
		else if (isBulletBoost)
		{
			// if damageBoost item is used
			bullet = Instantiate(bulletBoost, bulletSpawn.position, arm.transform.rotation * Quaternion.Euler(0, 90f, 0));
			bullet.GetComponent<Bullet>().basedamage = baseDmg * 2; // sets dmg of bullet to class dmg * 2
			matKey = 1; // 1 = damageboostItemmat
			bullet.GetComponent<Rigidbody>().velocity = (mainCamera.transform.forward + new Vector3(0, 0.25f, 0)) * 3 * bulletSpeed;
			bullet.GetComponent<Bullet>().mul *= multiplier;
			bullet.GetComponent<Bullet>().SetSourcePlayer(this);
		}
		else
		{
			// standard bullet
			bullet = Instantiate(bulletPrefab, bulletSpawn.position, arm.transform.rotation * Quaternion.Euler(0f, 90f, 0f));
			bullet.GetComponent<Bullet>().basedamage = baseDmg; // sets dmg of bullet to class dmg
			matKey = 0; // 0 = standardBulletMat

			//support class only
			if (freezeShot && (bullet.GetComponent<Bullet>().freezeShot == false))
			{
				bullet.GetComponent<Bullet>().freezeShot = true;
			}
			bullet.GetComponent<Rigidbody>().velocity = (mainCamera.transform.forward + new Vector3(0,0.25f,0)) * 3 * bulletSpeed;
			bullet.GetComponent<Bullet>().mul *= multiplier;
			bullet.GetComponent<Bullet>().SetSourcePlayer(this);
		}

		//LSE only
		if (buffedAttack)
		{
			RpcBuffedAttack();
		}

		// Add velocity to the bullet, depending on charge
		//bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 3 * Mathf.Sqrt(charge * 50);


		//damage dealer class only
		if (applyDots)
		{
			bullet.GetComponent<Bullet>().dot_Effect = true;
			bullet.GetComponent<Bullet>().dot_dmg *= dotMultiplier;
			bullet.GetComponent<Bullet>().dotSkill = activeDotSkill;
			bullet.GetComponent<Bullet>().pierces = numberPierces;
		}


		if (canCrit)
		{
			int crit = Random.Range(0, 100);
			if (crit <= critchance)
			{
				bullet.GetComponent<Bullet>().basedamage *= 2; // doubles Basedmg if Crit
			}
		}



		if (mni_skill1IsActive)
		{
			matKey = 2; // 2 = MNI_skill1mat;
		}
		else if (activeDotSkill)
		{
			matKey = 3; //3 = MNI_skill3mat;
		}

		if (!healingMode)
		{
			if (isServer)
			{
				RpcChangeBulletMat(matKey);
			}
			else
			{
				ChangeBulletMat(matKey);
			}
		}



		//make Crane look upwards :)
		bullet.transform.Rotate(0, 0, -60f);

		// Add size to the bullet, depending on charge
		bullet.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);

		// Destroy the bullet after 2 seconds
		Destroy(bullet, bulletRange);
	}

	[ClientRpc]
	private void RpcChangeBulletMat(int matKey)
	{
		ChangeBulletMat(matKey);
	}

	private void ChangeBulletMat(int matKey)
	{
		if (bullet != null)
		{
			switch (matKey)
			{
				case 0:
					bullet.GetComponent<Renderer>().material = standartBulletMat;
					break;
				case 1:
					bullet.GetComponent<Renderer>().material = dmgBoostItemMat;
					break;
				case 2:
					bullet.GetComponent<Renderer>().material = MNI_skill1mat;
					break;
				case 3:
					bullet.GetComponent<Renderer>().material = MNI_skill3mat;
					break;
				default:
					bullet.GetComponent<Renderer>().material = standartBulletMat;
					break;
			}
		}
	}

	[ClientRpc]
	private void RpcBuffedAttack()
	{
		float sideBulletSize = 0.2f;
		int sideBulletDamage = baseDmg - 5;

		GameObject bulletLeft = Instantiate(bulletPrefab, bulletSpawn.position, arm.transform.rotation * Quaternion.Euler(-45f, 90f, 0f));
		GameObject bulletRight = Instantiate(bulletPrefab, bulletSpawn.position, arm.transform.rotation * Quaternion.Euler(45f, 90f, 0f));
		bulletLeft.GetComponent<Bullet>().basedamage = sideBulletDamage; // sets dmg of bullet to class dmg
		bulletRight.GetComponent<Bullet>().basedamage = sideBulletDamage; // sets dmg of bullet to class dmg

		bulletLeft.GetComponent<Rigidbody>().velocity = bulletLeft.transform.up * 3 * bulletSpeed;
		bulletLeft.GetComponent<Bullet>().mul *= multiplier;
		bulletLeft.transform.Rotate(0, 0, -60f);

		bulletRight.GetComponent<Rigidbody>().velocity = bulletRight.transform.up * 3 * bulletSpeed;
		bulletRight.GetComponent<Bullet>().mul *= multiplier;
		bulletRight.transform.Rotate(0, 0, -60f);

		bulletLeft.transform.localScale = new Vector3(sideBulletSize, sideBulletSize, sideBulletSize);
		bulletRight.transform.localScale = new Vector3(sideBulletSize, sideBulletSize, sideBulletSize);

		// Destroy the bullet after 2 seconds
		Destroy(bulletLeft, bulletRange);
		Destroy(bulletRight, bulletRange);
	}


	[Command]
	public void CmdSendNameToServer(string nameToSend)
	{
		RpcSetPlayerName(nameToSend);
	}

	[ClientRpc]
	void RpcSetPlayerName(string name)
	{
		gameObject.transform.Find("Name Canvas ").transform.Find("Text").GetComponent<Text>().text = name;
	}

	[Command]
	private void CmdDestroyNPC(NetworkInstanceId id)
	{
		GameObject g = NetworkServer.FindLocalObject(id);

		if (g != null)
		{
			NetworkServer.Destroy(g);
		}
	}

	///<summary>
	/// This Method is called from class "OrientationLoss.cs"
	/// if isActive= true, all NPCs get each other as a target and start "dancing" around, but dealing not damage
	/// if isActive = false, NPCs get players as target
	///</summary>
	///<param name="isActive">if OrientationLoss ability is activated </param>
	[Command]
	public void CmdParty(bool isActive)
	{

		if (isActive)
		{
			stunParty = "party";
			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
			foreach (GameObject go in players)
			{
				NPC.NPCManager.Instance.RemoveTarget(go.transform);
			}
			GameObject[] npcs = GameObject.FindGameObjectsWithTag("Enemy");
			foreach (GameObject go in npcs)
			{
				NPC.NPCManager.Instance.AddTarget(go.transform);
			}
		}
		else
		{
			if (stunParty == "party")
			{
				CmdNpcAgainMove();
			}
		}
	}

	///<summary>
	/// This method is called from the Detonator script by the items Grenade or "Bomb.
	/// An explosion is instantiated which causes damage to all NPCs that fall into its radius.
	/// </summary>
	///<param name="throwable">false for local explosions, true for distant explosions</param>
	[Command]
	public void CmdCreateDetonator(bool throwable)
	{
		float bulletSize = Mathf.Clamp(0.5f, 0.3f, 0.8f);
		float charge = Mathf.Clamp(0.5f, 0, 1.8f);
		var bullet = Instantiate(explosion, bulletSpawn.position, arm.transform.rotation * Quaternion.Euler(0, 90f, 0));

		if (throwable)
		{
			// for distant explosions
			bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 3 * Mathf.Sqrt(charge * 50);
			Multiplayer.Explosion e = bullet.GetComponent<Explosion>();
			if (e != null)
			{
				e.maxDamage = 60f;
			}

		}
		//make Crane look upwards :)
		bullet.transform.Rotate(0, 0, -60f);
		NetworkServer.Spawn(bullet);

		bullet.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);
		//	Destroy(bullet, 12.0f);

	}

	/// <summary>
	/// Creates a bullet to throw a slowfield on the ground.
	/// </summary>
	[Command]
	public void CmdCreateSlowBullet()
	{
		float bulletSize = Mathf.Clamp(0.4f, 0.3f, 0.7f);
		float charge = Mathf.Clamp(0.5f, 0, 1.8f);
		var bullet = Instantiate(slowbullet, bulletSpawn.position, arm.transform.rotation * Quaternion.Euler(0, 90f, 0));
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 3 * Mathf.Sqrt(charge * 15);
		//make Crane look upwards :)
		bullet.transform.Rotate(0, 0, -60f);
		NetworkServer.Spawn(bullet);
		bullet.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);
	}



	/// <summary>
	/// Creates the effects for all items which increase the player's speed.
	/// </summary>

	[Command]
	private void CmdCreatespeedEffekt()
	{
		speedTimeCounter = 0;
		speedEffektClone = (GameObject)Instantiate(speedEffekt, gameObject.transform.position, transform.rotation);
		speedEffektClone.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.85f, gameObject.transform.position.z);
		NetworkServer.Spawn(speedEffektClone);
		Destroy(speedEffektClone, 1.5f);
	}


	/// <summary>
	/// All profs which are not burned out gain back mental health.
	/// </summary>
	///<param name="heal">amount of health</param>
	[Command]
	public void CmdHealAll(int heal)
	{
		gameObject.transform.GetComponent<AudioManager>().PlaySound(transform.position, 5);
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject go in players)
		{
			if (!go.GetComponent<Health>().IsZero() && !go.GetComponent<PlayerController>().isBurnedOut)
			{
				go.GetComponent<Health>().Heal(heal);
			}
		}
	}


	/// <summary>
	/// This method instantiates a "copy" of the player prefab to distract the students.
	/// If a second dummy is requested, the first dummy get's destroyed.
	/// </summary>
	///<param name="isActive">if Distraction ability is activated</param>
	[Command]
	public void CmdDummy(bool isActive)
	{
		if (dummy != null)
		{
			// remove dummy if second dummy is called or if the distraction ability has ended
			NPCManager.Instance.AddTarget(gameObject.transform);
			NPCManager.Instance.RemoveTarget(dummy.transform);
			GameOverManager.Instance.RemoveProf(dummy.GetComponent<PlayerController>());
			CmdNpcAgainMove();
			NetworkServer.Destroy(dummy);
			dummy = null;
		}
		if (isActive)
		{
			dummy = Instantiate(transform.gameObject, new Vector3(gameObject.transform.position.x + 1.1f, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
			dummy.GetComponent<PlayerController>().enabled = false;
			dummy.GetComponent<Health>().SetHealth();
			dummy.GetComponent<Health>().enabled = false;
			dummy.GetComponent<Health>().SetVulnerability(false);
			dummy.name = "Dummy_" + gameObject.name;
			dummy.tag = "Dummy";

			NPCManager.Instance.RemoveTarget(gameObject.transform);
			NetworkServer.Spawn(dummy);

			DummyAccessBar(dummy.gameObject.GetComponent<NetworkIdentity>().netId);
		}
	}


	/// <summary>
	/// Remove the Access Bar of the distraction dummy.
	/// </summary>
	///<param name="id">NetworkID from dummy</param>
	private void DummyAccessBar(NetworkInstanceId id)
	{
		GameObject g = NetworkServer.FindLocalObject(id);
		if (g != null)
		{
			GameObject dummyAccessBar = g.transform.GetChild(5).transform.GetChild(0).gameObject;
			dummyAccessBar.SetActive(false);
		}

	}


	///<summary>
	/// This method will freeze all NPCs to their position for a certain amount of time.
	/// It is also called to unfreeze the NPCs again.
	/// Item: GlobalStun (smartphone)
	///</summary>
	///<param name="isActive">if GlobalStun ability is activated</param>
	[Command]
	public void CmdCreateGlobalStun(bool isActive)
	{
		if (isActive)
		{
			stunParty = "stun";
			CmdNpcAgainMove();
			npcs = GameObject.FindGameObjectsWithTag("Enemy");
			foreach (GameObject go in npcs)
			{
				go.GetComponent<NPC.NPC>().move.agent.speed = 0f;
			}


			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
			foreach (GameObject go in players)
			{
				NPC.NPCManager.Instance.RemoveTarget(go.transform);
			}
		}
		else
		{
			if (stunParty == "stun")
			{
				CmdNpcAgainMove();
			}
		}
	}


	/// <summary>
	/// This Method called from the methods "CmdParty()" , "CmdCreateGlobalStun()"・ｽand "CmdDummy()"
	/// every NPC becomming agan speed 6.0f and again Initialise
	/// </summary>
	[Command]
	private void CmdNpcAgainMove()
	{


		GameObject[] npcs = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject go in npcs)
		{
			go.GetComponent<NPC.NPC>().move.agent.speed = 7.0f;
			go.GetComponent<NPC.NPC>().againLive();
			NPC.NPCManager.Instance.RemoveTarget(go.transform);
		}
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject go in players)
		{
			NPC.NPCManager.Instance.AddTarget(go.transform);
		}

	}

	/// <summary>
	/// Starts teleport effect at start position.
	/// </summary>
	[Command]
	public void CmdTeleportEffectStart()
	{
		portEffektCloneStart = (GameObject)Instantiate(portEffektPrefab, gameObject.transform.position, transform.rotation);
		portEffektCloneStart.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1.0f, gameObject.transform.position.z);
		NetworkServer.Spawn(portEffektCloneStart);
		Destroy(portEffektCloneStart, 3.0f);

	}

	/// <summary>
	/// Starts teleport effect at target position.
	/// </summary>
	[Command]
	public void CmdTeleportEffectEnd(Vector3 pos)
	{
		portEffektCloneEnd = (GameObject)Instantiate(portEffektPrefab, pos, gameObject.transform.rotation);
		portEffektCloneEnd.transform.position = new Vector3(pos.x, pos.y - 1.0f, pos.z);
		NetworkServer.Spawn(portEffektCloneEnd);
		Destroy(portEffektCloneEnd, 3.0f);

	}

	///<summary>
	/// This method is called after a player activated the item DamageBoost.
	/// It changes the isBulletBoost flag of the PlayerController.
	///</summary>
	///<param name="isActive">if DamageBoost ability is activated</param>
	[Command]
	internal void CmdCreateBoostBullet(bool isAktive)
	{
		if (isAktive)
		{
			isBulletBoost = true;
		}
		else
		{
			isBulletBoost = false;
		}
	}


	/// <summary>
	/// The player becomes invisible to all mobs and changes its material to a more invisible shade,
	/// if the Invisibility item is activated. For the inactive state, the player's material is set
	/// back to normal, if the invisible shade is still the current material.
	/// </summary>
	///<param name="isActive">if Invisibility is activated</param>
	[Command]
	public void CmdInvisibility(bool isActive)
	{
		if (isActive)
		{
			material = "invisible";
			RpcSetPlayerColorInvisible();
			NPCManager.Instance.RemoveTarget(gameObject.transform);
		}
		else
		{
			if (material.Equals("invisible"))
			{
				RpcSetPlayerColorNormal();
			}
			NPCManager.Instance.AddTarget(gameObject.transform);
		}
	}


	/// <summary>
	/// The player can no longer take damage if the Invulnerability item is activated.
	/// In the invulnerable state the player changes its material to rainbow.
	/// For the inactive state, the player's material is set back to normal, if rainbow is still the current material.
	/// </summary>
	///<param name="isActive">if Invulnerability is activated</param>
	[Command]
	public void CmdInvulnerability(bool isActive)
	{

		if (isActive)
		{
			material = "rainbow";
			RpcSetPlayerColorInvulnerability();
			health.SetVulnerability(false);
		}
		else
		{
			if (material.Equals("rainbow"))
			{
				RpcSetPlayerColorNormal();
			}
			health.SetVulnerability(true);
		}

	}

	///<summary>
	/// The player gets the standard material.
	/// </summary>
	[ClientRpc]
	void RpcSetPlayerColorNormal()
	{
		gameObject.GetComponent<Renderer>().material = normal;
	}


	///<summary>
	/// The player chenges its material to a more invisible shade.
	/// </summary>
	[ClientRpc]
	void RpcSetPlayerColorInvisible()
	{
		gameObject.GetComponent<Renderer>().material = invisibleColor;
	}


	///<summary>
	/// The player changes its material to rainbow.
	/// </summary>
	[ClientRpc]
	void RpcSetPlayerColorInvulnerability()
	{
		gameObject.GetComponent<Renderer>().material = rainbow;
	}

	[ClientRpc]
	public void RpcIncreaseSkillPoints(int points)
	{
		skillMenu.SetSkillPoints(points);
	}

	//////////////////////////////////////////////////////////////////////////////////////
	// --- The next set of methods is used to implement the skill tree and the skills --- //
	//////////////////////////////////////////////////////////////////////////////////////


	[Command]
	public void CmdToggleMniMainSkillVar()
	{
		if (isServer && isLocalPlayer)
			return;
		RpcToggleMniMainSKillVar();
	}
	[ClientRpc]
	public void RpcToggleMniMainSKillVar()
	{
		this.mni_skill1IsActive = !this.mni_skill1IsActive;
	}
	/// <summary>
	/// Sets up SkillTree
	/// </summary>
	public void SetUpSkillTree()
	{
		// Initialize empty Skill Tree for all roles where no Skill Tree is implemented to prevent errors

		// make sure that all skills are locked at start
		for (int i = 0; i < NUM_SKILLS; i++)
		{
			if (skills[i] != null)
				skills[i].unlocked = false;
		}

		skillMenu.player = this;
		skillMenu.skillTree.SetUpSkillTree(skills);

	}

	/// <summary>
	/// adds a Skill onto the skill slots
	/// </summary>
	/// <param name="skill"> skill to be added</param>
	public void AddActiveSkill(Skill skill)
	{
		if (!skillSlots.transform.GetComponent<SkillSlots>().IsFull())
		{
			skillSlots.transform.GetComponent<SkillSlots>().AddSkill(skill);
		}
	}

	// --- Following Methods are for the MNI(Damage Dealer (DD)) Skills --- //

	/// <summary>
	/// activates the boost dmg
	/// </summary>
	/// <param name="isActive"> bool if skill is activated or deactivated</param>
	public void CmdChangeDmgMultiplier(bool isActive)
	{
		if (isActive)
		{
			gameObject.transform.GetComponent<AudioManager>().PlaySound(transform.position, 8);
			multiplier = boostDmg;
			bulletSize = 0.4f;
			mni_skill1IsActive = true;
			if (dotEffectActive)
			{
				applyDots = true;
			}
		}
		else
		{
			multiplier = 1.0f;
			bulletSize = 0.3f;
			applyDots = false;
			mni_skill1IsActive = false;
		}
	}

	/// <summary>
	/// Changes the Number of Pierce throughs for this Player
	/// </summary>
	/// <param name="isActive"> bool if skill is activated or deactivatedparam>
	/// <param name="number"> number of pierc throughs to be added</param>
	public void CmdChangePierces(bool isActive, int number)
	{
		if (isActive)
		{
			gameObject.transform.GetComponent<AudioManager>().PlaySound(transform.position, 9);
			numberPierces += number;
			applyDots = true;
		}
		else
		{
			numberPierces -= number;
		}
	}

	/// <summary>
	/// Uses the Wrath of the Dean abilitie.
	/// activates the mouse Curser to klick a position.
	/// Sets the var wrath to true so the Input nows to use the ability
	/// </summary>
	/// <param name="duration"> duration the Ability is active</param>
	/// <param name="radius"> radius of the Ability </param>
	public void CmdActivateWrathOfDean(float duration, float radius)
	{
		wrath = true;
		wrathRadius = radius;
		wrathDuration = duration;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	[Command]
	private void CmdSpawnBulletsWrath(float xPos, float zPos)
	{
		RpcSpawnBulletsWrath(xPos, zPos);
	}

	[ClientRpc]
	private void RpcSpawnBulletsWrath(float xPos, float zPos)
	{
		StartCoroutine(SpawnBulletsDuration(xPos, zPos));
	}

	/// <summary>
	/// Spawns a Wave of Bullets every 0.2f seconds for the wrathDuration
	/// </summary>
	/// <param name="xPos"> the center x spawn Position</param>
	/// <param name="zPos"> the center z spawn Position </param>
	/// <returns></returns>
	private IEnumerator SpawnBulletsDuration(float xPos, float zPos)
	{
		GameObject wrath;
		Vector3 spawnPoss = new Vector3(xPos, -0.89f, zPos);
		wrath = Instantiate(wrathIndicator, spawnPoss, Quaternion.Euler(90, 0, 0));
		NetworkServer.Spawn(wrath);
		Color wrathColor = wrath.GetComponent<SpriteRenderer>().color;

		int fadeInCounter = 0;
		int fadeInDuration = 20;

		while (fadeInCounter < fadeInDuration)
		{
			yield return new WaitForSecondsRealtime(0.1f);
			wrathColor.a *= 2;
			wrath.GetComponent<SpriteRenderer>().color = wrathColor;
			fadeInCounter++;
		}

		float i = 0.0f;

		while (i < wrathDuration)
		{

			wrathColor.a *= 0.8f;
			wrath.GetComponent<SpriteRenderer>().color = wrathColor;
			CmdSpawnBullets(Random.Range(4, 9), Random.Range(-10, -5), wrathRadius, xPos, zPos);
			i += 0.2f;
			yield return new WaitForSecondsRealtime(0.2f);
		}

		Destroy(wrath);
	}


	/// <summary>
	/// This Method spawns a wave of Bullets.
	/// </summary>
	/// <param name="number"> The number of Bullets a Wave contains </param>
	/// <param name="speed"> The speed of the Bullets of this wave </param>
	/// <param name="radius"> The Radius in which the bullets are spawnt </param>
	/// <param name="XPos"> the center x spawn Position </param>
	/// <param name="zPos"> the center z spawn Position </param>
	[Command]
	private void CmdSpawnBullets(int number, float speed, float radius, float xPos, float zPos)
	{
		RpcClientSpawnBullets(Random.Range(4, 9), Random.Range(-10, -5), wrathRadius, xPos, zPos);
	}

	[ClientRpc]
	private void RpcClientSpawnBullets(int number, float speed, float radius, float xPos, float zPos)
	{
		for (int i = 0; i < number; i++)
		{
			GameObject bullet;
			float size = Random.Range(0.3f, 0.6f);
			float xDirec = Random.Range(-3f, 3f);
			float zDirec = Random.Range(-3f, 3f);

			Vector3 spawnPoss = new Vector3(xPos + Random.Range(-radius, radius), 20f, zPos + Random.Range(-radius, radius));

			bullet = Instantiate(bulletPrefab, spawnPoss, Quaternion.Euler(0, 0, 0));
			bullet.GetComponent<Rigidbody>().velocity = new Vector3(xDirec, speed, zDirec);
			bullet.transform.Rotate(xDirec, zDirec, -90f);
			bullet.transform.localScale = new Vector3(size, size, size);

			bullet.GetComponent<SphereCollider>().center =
				new Vector3(bullet.GetComponent<SphereCollider>().center.x, (bullet.GetComponent<SphereCollider>().center.y + 3.5f), bullet.GetComponent<SphereCollider>().center.z);
			bullet.GetComponent<SphereCollider>().radius *= 8;

			bullet.GetComponent<Bullet>().basedamage = 50;
			bullet.GetComponent<Bullet>().wrath = true;

			//NetworkServer.Spawn(bullet);

			Destroy(bullet, 2.5f); // Destroy Bullet after 2.5 seconds
		}
	}

	/// <summary>
	/// Creates a Bullet that deals the given amount of dmg with a special Color
	/// </summary>
	/// <param name="dmg">(Base) dmg the Bullet deals</param>
	[Command]
	public void CmdStrongShot(int dmg)
	{
		RpcClientStrongShot(dmg);
	}

	[ClientRpc]
	private void RpcClientStrongShot(int dmg)
	{
		GameObject bullet;
		gameObject.transform.GetComponent<AudioManager>().PlaySound(transform.position, 10);
		bullet = Instantiate(bulletBoost, bulletSpawn.position, arm.transform.rotation * Quaternion.Euler(0, 90f, 0));
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 3 * 10;
		bullet.GetComponent<Bullet>().basedamage = dmg;
		bullet.transform.Rotate(0, 0, -60f);
		bullet.GetComponent<Renderer>().material = strongMaterial;
		bullet.transform.localScale = new Vector3(0.5f, 0.5f, bulletSize);
		Destroy(bullet, bulletRange);
	}


	// --- Following Methods are for the Gesundheit(Healer) Skills --- //

	/// <summary>
	/// This Function Toggles the bool for healing or damaging mode of the healer.
	/// Used to activate/deactivate the healing mode on all clients to keep the variable in sync.
	/// </summary>
	[Command]
	public void CmdToggleGesMainSkillVar()
	{
		if (isServer && isLocalPlayer)
			return;
		RpcToggleGesMainSKillVar();
	}
	/// <summary>
	/// This Function Toggles the bool for healing or damaging mode of the healer.
	/// Used to activate/deactivate the healing mode on all clients to keep the variable in sync.
	/// </summary>
	[ClientRpc]
	public void RpcToggleGesMainSKillVar()
	{
		this.healingMode = !this.healingMode;
	}

	/// <summary>
	/// Triggers the healingMode boolean to switch state
	/// If its true, the Healer will shoot Healing Bullets
	/// </summary>
	public void CmdToggleHealingMode(int slot)
	{
		gameObject.transform.GetComponent<AudioManager>().PlaySound(transform.position, 8);
		healingMode = !healingMode;
		if (permaHealingMode)
		{
			SkillCooldownController.Instance.healBulletOverlay(slot, healingMode);
		}
	}

	[Command]
	public void CmdStrongHealingBullet(int heal)
	{
		RpcClientStrongHealingBullet(heal);
	}
	/// <summary>
	/// This function instantiates, modifies, shoots and destroys the PiercingHealBullet Skill
	/// </summary>
	/// <param name="heal">The amount of healing done per hit</param>
	[ClientRpc]
	private void RpcClientStrongHealingBullet(int heal)
	{
		GameObject bullet;
		gameObject.transform.GetComponent<AudioManager>().PlaySound(transform.position, 10);
		bullet = Instantiate(healingBullet, bulletSpawn.position, arm.transform.rotation * Quaternion.Euler(0, 90f, 0));
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 3 * 2;
		bullet.GetComponent<HealingBullet>().baseHeal = heal;
		bullet.GetComponent<HealingBullet>().pierces = 1;
		bullet.GetComponent<HealingBullet>().superBullet = true;
		bullet.transform.Rotate(0, 0, -60f);
		bullet.GetComponent<Renderer>().material = strongHealingBulletMaterial;
		bullet.transform.localScale = new Vector3(0.5f, 0.5f, bulletSize + 1);
		Destroy(bullet, bulletRange + 2);
	}

	/// <summary>
	/// This function reduces the cooldown of the PiercingHealBullet on Hit with a normal(damaging) Bullet
	/// </summary>
	/// <param name="cdrVal">The amount of cooldown reduction per hit in seconds</param>
	public void PiercingHealBulletReduceCooldown(int cdrVal)
	{
		cdPiercingHealBullet -= cdrVal; //reduce actual cd
		SkillCooldownController.Instance.ReduceCooldown(9, cdrVal); //reduce the cd in the UI-overlay
	}


	[Command]
	public void CmdDropShieldItem(int shieldVal)
	{
		RpcClientDropShieldItem(shieldVal);
	}
	/// <summary>
	/// This function instantiates the shieldItem from the Shield Drop skill of the healer
	/// </summary>
	/// <param name="shieldVal">The amount of shield granted</param>
	[ClientRpc]
	private void RpcClientDropShieldItem(int shieldVal)
	{
		GameObject shieldItemObj;
		gameObject.transform.GetComponent<AudioManager>().PlaySound(transform.position, 9);
		shieldItemObj = Instantiate(shieldItem, bulletSpawn.position, arm.transform.rotation * Quaternion.Euler(0, 90f, 0));
		shieldItemObj.GetComponent<Rigidbody>().velocity = shieldItemObj.transform.up * 3 * 2;
		//bullet.GetComponent<HealingBullet>().baseHeal = heal;
		shieldItemObj.GetComponent<Rigidbody>().velocity = shieldItemObj.transform.up * 3 * Mathf.Sqrt(charge * 50);
		//bullet.GetComponent<HealingBullet>().pierces = 1;
		//bullet.GetComponent<HealingBullet>().superBullet = true;
		shieldItemObj.transform.Rotate(0, 0, -60f);
		//bullet.GetComponent<Renderer>().material = strongHealingBulletMaterial;
		shieldItemObj.transform.localScale = new Vector3(0.5f, 0.5f, bulletSize);
		shieldItemObj.GetComponent<ShieldItem>().applyHots = applyHotsOnShieldItemPickup;
		Destroy(shieldItemObj, shieldItemDropDuration);
	}

	/// <summary>
	/// This function reduces the cooldown of the ShieldDrop on Hit with a normal(damaging) Bullet
	/// </summary>
	/// <param name="cdrVal">The amount of cooldown reduction per hit in seconds</param>
	public void ShieldItemDropReduceCooldown(int cdrVal)
	{
		cdShieldItemDrop -= cdrVal;
		SkillCooldownController.Instance.ReduceCooldown(2, cdrVal); //reduce the cd in the UI-overlay
	}



	[Command]
	public void CmdCreateCoffeeMachine(int duration, float radius)
	{
		RpcClientCreateCoffeeMachine(duration, radius);
	}
	/// <summary>
	/// This function instantiates the Coffeemachine, the ultimate skill of the healer
	/// </summary>
	/// <param name="duration">The duration the coffeemachine lasts in seconds</param>
	/// <param name="radius">The radius of effect</param>
	[ClientRpc]
	private void RpcClientCreateCoffeeMachine(int duration, float radius)
	{
		GameObject coffeeMachine;
		gameObject.transform.GetComponent<AudioManager>().PlaySound(transform.position, 11);
		coffeeMachine = Instantiate(coffeeMachineObject, gameObject.transform.position, gameObject.transform.rotation * Quaternion.Euler(90f, 45f, -40f));
		coffeeMachine.transform.eulerAngles = new Vector3(-180f, 60f, 180f);
		coffeeMachine.transform.position = new Vector3(coffeeMachine.transform.position.x, -1, coffeeMachine.transform.position.z);
		StartCoroutine(HealingWaves(coffeeMachine, duration, radius));
	}
	/// <summary>
	/// This Coroutine represents the Healing Waves that emit from the Coffeemachine skill.
	/// </summary>
	/// <param name="coffeeMachine">The coffeemachine GameObject</param>
	/// <param name="duration">The duration of the coffemachine in seconds</param>
	/// <param name="radius">The radius around the coffemachine allies get healed in</param>
	/// <returns></returns>
	private IEnumerator HealingWaves(GameObject coffeeMachine, int duration, float radius)
	{
		GameObject coffeemachineHealingCircle = Instantiate(coffeeMachineIndicator, coffeeMachine.transform.position + new Vector3(0, 0.02f, 0), Quaternion.Euler(90, 0, 0));
		Color tmp = coffeemachineHealingCircle.GetComponent<SpriteRenderer>().color;
		tmp = Color.green;
		tmp.a = 0.1f;
		coffeemachineHealingCircle.GetComponent<SpriteRenderer>().color = tmp;
		coffeemachineHealingCircle.GetComponent<Transform>().localScale = new Vector3(radius / 2 + 0.46f, radius / 2 + 0.46f, 0);
		Collider[] colliders;
		for (int i = 0; i < duration * 2; i++) //Duration in sekunden -> 20s, 2 ticks pro sekunde -> duration = 40
		{
			colliders = Physics.OverlapSphere(coffeeMachine.transform.position, radius);
			for (int j = 0; j < colliders.Length; j++)
			{
				var target = colliders[j].gameObject;
				if (target.tag == "Player")
				{
					var health = target.GetComponent<Health>();
					health.Heal(coffeeMachineHealPerTick);
				}
			}
			yield return new WaitForSecondsRealtime(0.5f);
		}
		Destroy(coffeeMachine);
		Destroy(coffeemachineHealingCircle);
	}


	//Tank Skills

	/// <summary>
	/// This method is called when the tank activates his "taunt" skill.
	/// Also reduces the damage taken by the player if advanced taunting is activated.
	/// </summary>
	/// <param name="activated">
	/// if true, all enemies will attack the taunting player;
	/// if false, the enemies will attack all players again;
	/// </param>
	[Command]
	public void CmdTaunt(bool activated)
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		if (activated)
		{
			if (tauntDmgReduce)
			{
				GetComponent<Health>().tauntDmgReduce = true;
			}
			foreach (GameObject go in players)
			{
				if (go != gameObject)
				{
					NPCManager.Instance.RemoveTarget(go.transform);
				}
			}
			//NPCManager.Instance.AddTarget(gameObject.transform);
		}
		else
		{
			if (tauntDmgReduce)
			{
				GetComponent<Health>().tauntDmgReduce = false;
			}
			foreach (GameObject go in players)
				if (go != gameObject)
				{
					{
						NPCManager.Instance.AddTarget(go.transform);
					}
				}
		}
		RpcClientTaunt(activated);
	}

	/// <summary>
	/// This method is called when the tank activates his taunt skill.
	/// It changes the material of the tank and plays the taunt sound effect.
	/// </summary>
	/// <param name="activated">
	/// If true, material will be changed to the taunt material and sound effect will be played
	/// If false, material will be changed to the standard tank material
	/// </param>
	[ClientRpc]
	public void RpcClientTaunt(bool activated)
	{
		if (activated)
		{
			gameObject.GetComponent<Renderer>().material = tauntMat;
			transform.GetComponent<AudioManager>().PlaySound(transform.position, 10);

		}
		else
		{
			gameObject.GetComponent<Renderer>().material = tankMat;
		}
	}

	/// <summary>
	/// This method is called when the tank activates his stun skill.
	/// It spawns a stun cone gameobject and positions it properly.
	/// Also starts a coroutine for the stun effect.
	/// </summary>
	/// <param name="duration">
	/// The duration of the stun effect.
	/// </param>
	[Command]
	public void CmdStun(int duration)
	{
		RpcStun(duration);
	}

	[ClientRpc]
	public void RpcStun(int duration)
	{
		GameObject stunCone;
		gameObject.transform.GetComponent<AudioManager>().PlaySound(transform.position, 9);
		stunCone = Instantiate(stunConeObject, gameObject.transform.position + (transform.forward * 7), gameObject.transform.rotation);
		stunCone.transform.eulerAngles += new Vector3(90f, 0f, 180f);
		stunCone.transform.position = new Vector3(stunCone.transform.position.x, 0, stunCone.transform.position.z);
		StartCoroutine(Stun(stunCone, duration));
	}

	/// <summary>
	/// Waits for duration in seconds to pass and then destroys the stun cone gameobject that was spawned in <see cref="CmdStun(int)"/>
	/// </summary>
	/// <param name="stunCone">stun cone gameobject that was spawned in <see cref="CmdStun(int)"/></param>
	/// <param name="duration">duration of stun effect in seconds</param>
	private IEnumerator Stun(GameObject stunCone, int duration)
	{
		for (int i = 0; i < duration; i++)
		{
			yield return new WaitForSecondsRealtime(1);
		}
		Destroy(stunCone);
	}

	/// <summary>
	/// This method is called when the tank activates his invincibility skill.
	/// Sets the player <see cref="isInvincible"/> field and plays the invincibility sound effect
	/// </summary>
	/// <param name="activated">
	/// If true, player becomes invincible and sound effect is played
	/// If false, player becomes vulnerable again
	/// </param>
	[Command]
	public void CmdInvc(bool activated)
	{
		if (activated)
		{
			RpcClientInvc(activated);
			isInvincible = true;
			transform.GetComponent<AudioManager>().PlaySound(transform.position, 11);
		}
		else
		{
			RpcClientInvc(activated);
			isInvincible = false;
		}
	}

	/// <summary>
	/// This method is called when the tank activates his invincibility skill.
	/// It changes the material of the tank and plays the invincibility sound effect.
	/// </summary>
	/// <param name="activated">
	/// If true, material will be changed to the invincibility material and sound effect will be played
	/// If false, material will be changed to the standard tank material
	/// </param>
	[ClientRpc]
	public void RpcClientInvc(bool activated)
	{
		if (activated)
		{
			gameObject.GetComponent<Renderer>().material = invincibleMat;
		}
		else
		{
			gameObject.GetComponent<Renderer>().material = tankMat;
		}
	}

	/// <summary>
	/// This method is called when the shield of the tank breaks if he activated the explosing shield skill.
	/// It plays the sound effect of the breaking shield and deals damage to all nearby enemies.
	/// </summary>
	[Command]
	public void CmdShieldExplode()
	{
		transform.GetComponent<AudioManager>().PlaySound(transform.position, 13);
		Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
		for (int j = 0; j < colliders.Length; j++)
		{
			var target = colliders[j].gameObject;
			if (target.tag == "Enemy")
			{
				var health = target.GetComponent<Health>();
				health.TakeDamage(explosiveShieldDamage);
			}
		}
	}

	/// <summary>
	/// This method is called when the shield of the tank breaks if he activated the pushing shield skill.
	/// It pushes all nearby enemies backwards.
	/// </summary>
	[Command]
	public void CmdShieldPush()
	{
		//transform.GetComponent<AudioManager>().PlaySound(transform.position, 13);
		Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
		for (int j = 0; j < colliders.Length; j++)
		{
			var target = colliders[j].gameObject;
			if (target.tag == "Enemy")
			{
				target.transform.position -= (target.transform.forward * 4);
			}
		}
	}

	/// <summary>
	/// This method is called when the buffed tank skill is activated. 
	/// </summary>
	/// <param name="healthIncrease"></param>
	[Command]
	public void CmdBuffTank(int healthIncrease)
	{
		RpcBuffTank(healthIncrease);
	}

	/// <summary>
	/// This method is called when the buffed tank skill is activated.
	/// It increases the players health and activates the buffed attack.
	/// </summary>
	/// <param name="healthIncrease">Amount of health points to be increased</param>
	[ClientRpc]
	public void RpcBuffTank(int healthIncrease)
	{
		Multiplayer.Health health = gameObject.GetComponent<Multiplayer.Health>();
		health.MAX_HEALTH += healthIncrease;
		health.CmdHeal(healthIncrease);
		health.UpdateHealthbarSize();
		buffedAttack = true;
	}

	/// <summary>
	/// This method is called when the shield of the tank breaks if he activated the buffed shield skill.
	/// Increases the base damage and the fire rate of the player.
	/// The <see cref="buffedTank"/> boolean field prevents stacking of the buffed effect.
	/// </summary>
	/// <param name="activated">
	/// If true, buffed perks will come into effect
	/// If false, the perks will be reversed
	/// </param>
	[Command]
	public void CmdShieldBuff(bool activated)
	{
		if (activated && !buffedTank)
		{
			baseDmg += baseDamageIncrease;
			fireRate -= fireRateIncrease;
			buffedTank = true;
		}
		else if (!activated && buffedTank)
		{
			baseDmg -= baseDamageIncrease;
			fireRate += fireRateIncrease;
			buffedTank = false;
		}
	}

	/// <summary>
	/// This method is called when the supporter uses his slow field skill.
	/// The command calls the rpc method RpcSuppotSlowField().
	/// </summary>
	[Command]
	public void CmdSupportSlowField()
	{
		RpcSupportSlowField();
	}

	/// <summary>
	/// Dependending on active skills, this method chooses a slow field to spawn, then calls the according Command.
	/// </summary>
	[ClientRpc]
	public void RpcSupportSlowField()
	{
		if (!isSlowAreaIncreased && !isSlowEffectIncreased)
		{
			CmdCreateSlowBullet();
		}
		else
		{
			if (isSlowAreaIncreased && !isSlowEffectIncreased)
			{
				CmdCreateSlowBulletArea();
			}
			if (isSlowEffectIncreased && !isSlowAreaIncreased)
			{
				CmdCreateSlowBulletEffect();
			}

			if (isSlowEffectIncreased && isSlowAreaIncreased)
			{
				CmdCreateSlowBulletAreaEffect();
			}
		}
	}

	/// <summary>
	/// This method is called when the supporter uses his main skill.
	/// It shoots a slow bullet and spawns a random item.
	/// </summary>
	[Command]
	public void CmdSupportSpawnItem()
	{
		transform.GetComponent<AudioManager>().PlaySound(transform.position, 8);
		float bulletSize = Mathf.Clamp(0.4f, 0.3f, 0.7f);
		float charge = Mathf.Clamp(0.5f, 0, 1.8f);
		var randItem = Instantiate(supportBullet, bulletSpawn.position, arm.transform.rotation * Quaternion.Euler(0, 90f, 0));
		randItem.GetComponent<Rigidbody>().velocity = randItem.transform.up * 3 * Mathf.Sqrt(charge * 15);
		randItem.GetComponent<SupportBullet>().bulletType = "randomItemBullet";
		randItem.transform.Rotate(0, 0, -60f);
		NetworkServer.Spawn(randItem);
		randItem.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);
	}

	/// <summary>
	/// This method is called when the supporter uses his damage boost item skill.
	/// Creates a slow bullet and spawns a damage boost item.
	/// </summary>
	[Command]
	public void CmdSupportDamageBoost()
	{
		transform.GetComponent<AudioManager>().PlaySound(transform.position, 9);
		float bulletSize = Mathf.Clamp(0.4f, 0.3f, 0.7f);
		float charge = Mathf.Clamp(0.5f, 0, 1.8f);
		var damageBoost = Instantiate(supportBullet, bulletSpawn.position, arm.transform.rotation * Quaternion.Euler(0, 90f, 0));
		damageBoost.GetComponent<Rigidbody>().velocity = damageBoost.transform.up * 3 * Mathf.Sqrt(charge * 15);
		damageBoost.GetComponent<SupportBullet>().bulletType = "damageBoostBullet";
		damageBoost.transform.Rotate(0, 0, -60f);
		NetworkServer.Spawn(damageBoost);
		damageBoost.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);
	}

	/// <summary>
	/// Creates slow bullet and spawns a slow field with an inreased area.
	/// </summary>
	[Command]
	public void CmdCreateSlowBulletArea()
	{
		float bulletSize = Mathf.Clamp(0.4f, 0.3f, 0.7f);
		float charge = Mathf.Clamp(0.5f, 0, 1.8f);
		var bullet = Instantiate(bulletArea, bulletSpawn.position, arm.transform.rotation * Quaternion.Euler(0, 90f, 0));
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 3 * Mathf.Sqrt(charge * 15);
		bullet.transform.Rotate(0, 0, -60f);
		NetworkServer.Spawn(bullet);
		bullet.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);
	}

	/// <summary>
	/// Creates a slow bullet and spawns a slow field with an increased effect.
	/// </summary>
	[Command]
	public void CmdCreateSlowBulletEffect()
	{
		float bulletSize = Mathf.Clamp(0.4f, 0.3f, 0.7f);
		float charge = Mathf.Clamp(0.5f, 0, 1.8f);
		var bullet = Instantiate(bulletEffect, bulletSpawn.position, arm.transform.rotation * Quaternion.Euler(0, 90f, 0));
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 3 * Mathf.Sqrt(charge * 15);
		bullet.transform.Rotate(0, 0, -60f);
		NetworkServer.Spawn(bullet);
		bullet.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);
	}

	/// <summary>
	/// Creates a slow bullet and spawnes an increased slow field area, with an increased effect
	/// </summary>
	[Command]
	public void CmdCreateSlowBulletAreaEffect()
	{
		float bulletSize = Mathf.Clamp(0.4f, 0.3f, 0.7f);
		float charge = Mathf.Clamp(0.5f, 0, 1.8f);
		var bullet = Instantiate(bulletAreaEffect, bulletSpawn.position, arm.transform.rotation * Quaternion.Euler(0, 90f, 0));
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 3 * Mathf.Sqrt(charge * 15);
		bullet.transform.Rotate(0, 0, -60f);
		NetworkServer.Spawn(bullet);
		bullet.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);
	}


	/// <summary>
	/// This method is called when the supporter uses his ultimate skill.
	/// The command calls the rpc method RpcSupportWarpZone().
	/// </summary>
	[Command]
	public void CmdSupportWarpZone()
	{
		RpcSupportWarpZone();
	}

	/// <summary>
	/// Instantiates the Warpzone of the supporter
	/// </summary>
	[ClientRpc]
	public void RpcSupportWarpZone()
	{
		transform.GetComponent<AudioManager>().PlaySound(transform.position, 11);
		var warpZone = Instantiate(WarpZone, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
	}

	/// <summary>
	/// Commmand calls rpc method RpcSetAura
	/// </summary>
	/// <param name="skill">Type of aura to be activated</param>
	[Command]
	public void CmdSetAura(int skill)
	{
		RpcSetAura(skill);
	}

	/// <summary>
	/// Command calls rpc method RpcFreeze
	/// </summary>
	/// <param name="enemy">Enemy that will be frozen</param>
	[Command]
	public void CmdFreeze(GameObject enemy)
	{
		RpcFreeze(enemy);
	}


	/// <summary>
	/// This method freezes enemies by setting their speed to 0
	/// </summary>
	/// <param name="enemy">Enemy that will be frozen</param>
	[ClientRpc]
	public void RpcFreeze(GameObject enemy)
	{
		if (enemy != null)
		{
			if (enemy.GetComponent<NPC.NPC>() != null)
			{
				if (enemy.GetComponent<NPC.NPC>().move != null)
				{
					if (enemy.GetComponent<NPC.NPC>().move.agent.speed != null)
					{
						enemy.GetComponent<NPC.NPC>().move.agent.speed = 0;
					}
				}
			}
		}
	}

	/// <summary>
	/// Command calls rpc method RpcFreezeReset
	/// </summary>
	/// <param name="enemy">Enemy that will be unfrozen</param>
	[Command]
	public void CmdFreezeReset(GameObject enemy)
	{
		RpcFreezeReset(enemy);
	}

	/// <summary>
	/// Method to reset NPC speed to standard speed after a certain time
	/// </summary>
	/// <param name="enemy">Enemy that will be unfrozen</param>
	[ClientRpc]
	public void RpcFreezeReset(GameObject enemy)
	{
		StartCoroutine(NPCSpeedHandler.ResetSpeed(enemy));
	}

	/// <summary>
	/// This method calls rpc method RpcAuraBoost
	/// </summary>
	/// <param name="aura">Support Aura</param>
	/// <param name="sourcePlayer">Source Player(support)</param>
	/// <param name="targetPlayer">Target Player</param>
	[Command]
	public void CmdAuraBoost(GameObject aura, GameObject sourcePlayer, GameObject targetPlayer)
	{
		float radius = 15f;
		RpcAuraBoost(aura, radius, sourcePlayer, targetPlayer);
	}

	/// <summary>
	/// Starts Coroutine "auraBoost"
	/// </summary>
	/// <param name="aura">Support Aura</param>
	/// <param name="radius">Radius of Aura</param>
	/// <param name="sourcePlayer">Source Player(support)</param>
	/// <param name="targetPlayer">Target Player</param>
	[ClientRpc]
	public void RpcAuraBoost(GameObject aura, float radius, GameObject sourcePlayer, GameObject targetPlayer)
	{
		StartCoroutine(auraBoost(aura, radius, sourcePlayer, targetPlayer));
	}

	/// <summary>
	/// This methods boosts base dmg and / or attack speed depending on which aura the supporter has activated
	/// </summary>
	/// <param name="aura">Support Aura</param>
	/// <param name="radius">Radius of Aura</param>
	/// <param name="sourcePlayer">Source Player(support)</param>
	/// <param name="targetPlayer">Target Player</param>
	private IEnumerator auraBoost(GameObject aura, float radius, GameObject sourcePlayer, GameObject targetPlayer)
	{
		PlayerController sourcePlayerController = sourcePlayer.GetComponent<PlayerController>();
		PlayerController targetPlayerController = targetPlayer.GetComponent<PlayerController>();

		if (!targetPlayerController.attackSpeedSet && sourcePlayerController.isAtckSpeedAuraActive && !sourcePlayerController.isDmgAuraActve)
		{
			targetPlayerController.fireRate *= targetPlayerController.fireRateMult;
			targetPlayerController.attackSpeedSet = true;
		}
		if (!targetPlayerController.dmgSet && !sourcePlayerController.isAtckSpeedAuraActive && sourcePlayerController.isDmgAuraActve)
		{
			targetPlayerController.baseDmg += dmgIncrease;
			targetPlayerController.dmgSet = true;
		}
		if (!targetPlayerController.dmgSet && !targetPlayerController.attackSpeedSet && sourcePlayerController.isAtckSpeedAuraActive && sourcePlayerController.isDmgAuraActve)
		{
			targetPlayerController.fireRate *= targetPlayerController.fireRateMult;
			targetPlayerController.attackSpeedSet = true;

			targetPlayerController.baseDmg += targetPlayerController.dmgIncrease;
			targetPlayerController.dmgSet = true;
		}
		StartCoroutine(resetAuraBoost(targetPlayer));

		yield return null;	
	}

	/// <summary>
	/// This method resets boosted base dmg and attack speed from the aura
	/// </summary>
	/// <param name="target">Target Player</param>
	private IEnumerator resetAuraBoost(GameObject target)
	{
		yield return new WaitForSeconds(2f);

		PlayerController targetPlayer = target.GetComponent<PlayerController>();

		if (targetPlayer.dmgSet == true && targetPlayer.attackSpeedSet == false)
		{
			targetPlayer.baseDmg -= dmgIncrease;
			targetPlayer.dmgSet = false;
		}

		if (targetPlayer.attackSpeedSet == true && targetPlayer.dmgSet == false)
		{
			targetPlayer.fireRate /= targetPlayer.fireRateMult;
			targetPlayer.attackSpeedSet = false;
		}

		if (targetPlayer.attackSpeedSet == true && targetPlayer.attackSpeedSet == true)
		{
			targetPlayer.fireRate /= targetPlayer.fireRateMult;
			targetPlayer.attackSpeedSet = false;

			targetPlayer.baseDmg -= targetPlayer.dmgIncrease;
			targetPlayer.dmgSet = false;
		}
	}



	/// <summary>
	/// Method to activate the correct aura, depending on activated passive skills
	/// </summary>
	/// <param name="skill">activates the correct aura (0 = dmg aura; 1 = attack speed aura)</param>
	[ClientRpc]
	public void RpcSetAura(int skill)
	{
		float radius = 15.0f;
		auraIndicator.SetActive(true);
		AuraTest.GetComponent<AuraBehav>().playerSupport = gameObject;

		if (skill == 0)
		{
			isDmgAuraActve = true;
		}

		if (skill == 1)
		{
			isAtckSpeedAuraActive = true;
		}

		if (isDmgAuraActve && !isAtckSpeedAuraActive)
		{
			Color tmp = AuraTest.GetComponent<SpriteRenderer>().color;
			tmp = Color.red;
			tmp.a = 0.1f;
			AuraTest.GetComponent<SpriteRenderer>().color = tmp;
			AuraTest.GetComponent<Transform>().localScale = new Vector3(radius / 2 + 0.46f, radius / 2 + 0.46f, 1);
		}

		if (isAtckSpeedAuraActive && !isDmgAuraActve)
		{
			Color tmp = AuraTest.GetComponent<SpriteRenderer>().color;
			tmp = Color.yellow;
			tmp.a = 0.1f;
			AuraTest.GetComponent<SpriteRenderer>().color = tmp;
			AuraTest.GetComponent<Transform>().localScale = new Vector3(radius / 2 + 0.46f, radius / 2 + 0.46f, 1);
		}

		if (isAtckSpeedAuraActive && isDmgAuraActve)
		{
			Color tmp = AuraTest.GetComponent<SpriteRenderer>().color;
			tmp = Color.blue;
			tmp.a = 0.1f;
			AuraTest.GetComponent<SpriteRenderer>().color = tmp;
			AuraTest.GetComponent<Transform>().localScale = new Vector3(radius / 2 + 0.46f, radius / 2 + 0.46f, 1);
		}
	}
}
