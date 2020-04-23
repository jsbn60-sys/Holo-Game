using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Parent class for all playerClasses. Implements combined functionalities
/// which are UI, movement and Input.
/// </summary>
public abstract class Player : Unit
{
	[SyncVar]
	public string name;
	[SerializeField]
	private GameObject itemQuickAccess;
	[SerializeField]
	private GameObject skillQuickAccess;
	[SerializeField]
	private GameObject map;

	private GameObject chat;

	[SerializeField]
	private GameObject skillMenu;
	[SerializeField]
	private GameObject inGameMenu;
	[SerializeField]
	private GameObject gameOverMenu;
	[SerializeField]
	private GameObject tutorialMenu;
	[SerializeField]
	private GameObject inventory;

	[SerializeField]
	private GameObject bulletSpawn;

	[SerializeField]
	private GameObject gun;

	// Camera related
	[SerializeField]
	private GameObject playerCamTarget;
	private GameObject playerCam;
	private float horizontal;
	private float vertical;

	// movement related
	private bool isGrounded;
	private bool isCollidingInAir;
	private float bonusGravity;

	// temp fix
	protected int role;


	// called at start
    protected void Start()
	{
		// UI
		chat = GameObject.Find("Chat");
		skillMenu.GetComponent<SkillMenu>().setSkillQuickAccess(skillQuickAccess.GetComponent<SkillQuickAccess>());
		setForGameplay();

		// variables
		attackRate = 0.2f;
		jumpForce = 55.0f;
		speed = 12f;
		bonusGravity = 60.0f;
		isGrounded = true;

		base.Start();
	}

	protected void Update()
	{
		//input
		if (!isDead())
		{
			moveInput();
			menuInput();
			itemInput();
			skillInput();
			shootInput();
		}

		base.Update();
	}

	private void updateTimers()
	{
		
	}

	/// <summary>
	/// Handles input related to movement and camera.
	/// </summary>
	private void moveInput()
	{
		horizontal += Input.GetAxis("Mouse X");
		vertical -= Input.GetAxis("Mouse Y");
		vertical = Mathf.Clamp(vertical, 0, 60);
		float z = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;
		float x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;

		// jump by applying force
		if (Input.GetKey(KeyCode.Space) && isGrounded && GetComponent<Rigidbody>().velocity.y < 5.0f)
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
			if (isCollidingInAir)
			{
				z *= -0.5f;
				x *= -0.5f;
			}
		}

		playerCamTarget.transform.rotation = Quaternion.Euler(vertical, horizontal, 0);
		transform.rotation = Quaternion.Euler(0, horizontal, 0);
		transform.Translate(x, 0, z);
	}

	/// <summary>
	/// Handles input related to toggling menus ingame.
	/// </summary>
	private void menuInput()
	{
		// open/close skillMenu
		if (Input.GetKeyDown(KeyCode.Tab) && !isChatSelected() && !gameOverMenu.activeSelf && !inGameMenu.activeSelf & !tutorialMenu.activeSelf && !inventory.activeSelf)
		{
			if (!skillMenu.activeSelf)
			{
				activateSkillMenu();
			}
			else
			{
				setForGameplay();
			}
		}

		// open/close inGameMenu
		else if (Input.GetKeyDown(KeyCode.Escape) && !isChatSelected() && !gameOverMenu.activeSelf && !skillMenu.activeSelf & !tutorialMenu.activeSelf && !inventory.activeSelf)
		{
			if (!inGameMenu.activeSelf)
			{
				activateInGameMenu();
			}
			else
			{
				setForGameplay();
			}
		}

		// open/close tutorialMenu
		else if (Input.GetKeyDown(KeyCode.F1) && !isChatSelected() && !gameOverMenu.activeSelf && !inGameMenu.activeSelf & !skillMenu.activeSelf && !inventory.activeSelf)
		{
			if (!tutorialMenu.activeSelf)
			{
				activateTutorialMenu();
			}
			else
			{
				setForGameplay();
			}
		}

		// open/close inventory
		else if (Input.GetKeyDown(KeyCode.I) && !isChatSelected() && !gameOverMenu.activeSelf && !inGameMenu.activeSelf & !skillMenu.activeSelf && !tutorialMenu.activeSelf)
		{
			if (!inventory.activeSelf)
			{
				activateInventory();
			}
			else
			{
				setForGameplay();
			}
		}
	}

	/// <summary>
	/// Handles input for the itemQuickAccess.
	/// </summary>
	private void itemInput()
	{
		for (int i = 1; i <= 5; i++) // to address slots 1-5
		{
			if (Input.GetKeyDown("" + i) && !isChatSelected())
			{
				itemQuickAccess.GetComponent<ItemQuickAccess>().useContent(i - 1, this);
			}
		}
	}

	/// <summary>
	/// Handles input for the skillQuickAccess.
	/// </summary>
	private void skillInput()
	{
		// primary Skill used with Right Klick
		if (Input.GetMouseButtonDown(1) && !isChatSelected())
		{
			//skillBar.GetComponent<SkillSlots>().useSkill(0, this);
		}


		// skill in slot Q
		if (Input.GetKeyDown(KeyCode.Q) && !isChatSelected())
		{
			//skillBar.GetComponent<SkillSlots>().useSkill(1, this);

		}

		// skill in slot E
		if (Input.GetKeyDown(KeyCode.E) && !isChatSelected())
		{
			//skillBar.GetComponent<SkillSlots>().useSkill(2, this);
		}
	}

	/// <summary>
	/// Handles input for shooting.
	/// </summary>
	private void shootInput()
	{
		if (Input.GetKey(KeyCode.Mouse0) && canAttack())
		{
			shootProjectile(attack.GetComponent<Projectile>());
			base.useAttack();
		}
	}


	/// <summary>
	/// Shoots a projectile from the player.
	/// This function is public, because it is used by some effects.
	/// </summary>
	/// <param name="projectile">Projectile to shoot</param>
	public void shootProjectile(Projectile projectile)
	{
		GameObject bullet = Instantiate(projectile.gameObject, bulletSpawn.transform.position, gun.transform.rotation * Quaternion.Euler(0f, 90f, 0f));
		bullet.GetComponent<Projectile>().setupProjectile(playerCam.transform.forward);
	}

	/// <summary>
	/// Removes player as NPC target and tells the GameOverManager that the player is dead.
	/// </summary>
	protected override void onDeath()
	{
		NPC.NPCManager.Instance.RemoveTarget(this.transform);
		GameOverManager.Instance.ProfIsBurnedOut(this);
	}


	/// <summary>
	/// Sets the UI for gameplay.
	/// </summary>
	private void setForGameplay()
	{
		deactivateUI(inGameMenu);
		deactivateUI(skillMenu);
		deactivateUI(gameOverMenu);
		deactivateUI(tutorialMenu);
		deactivateUI(inventory);

		activateUI(itemQuickAccess);
		activateUI(skillQuickAccess);
		activateUI(map);
		activateUI(chat);

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	/// <summary>
	/// Sets the UI for the SkillMenu.
	/// </summary>
	private void activateSkillMenu()
	{
		deactivateUI(itemQuickAccess);
		deactivateUI(skillQuickAccess);
		deactivateUI(map);
		deactivateUI(chat);
		deactivateUI(inGameMenu);
		deactivateUI(gameOverMenu);
		deactivateUI(tutorialMenu);
		deactivateUI(inventory);

		activateUI(skillMenu);

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	/// <summary>
	/// Sets the UI for the InGameMenu.
	/// </summary>
	private void activateInGameMenu()
	{
		deactivateUI(itemQuickAccess);
		deactivateUI(skillQuickAccess);
		deactivateUI(map);
		deactivateUI(chat);
		deactivateUI(skillMenu);
		deactivateUI(gameOverMenu);
		deactivateUI(tutorialMenu);
		deactivateUI(inventory);

		activateUI(inGameMenu);

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	/// <summary>
	/// Sets the UI for the TutorialMenu.
	/// </summary>
	private void activateTutorialMenu()
	{
		deactivateUI(itemQuickAccess);
		deactivateUI(skillQuickAccess);
		deactivateUI(map);
		deactivateUI(chat);
		deactivateUI(skillMenu);
		deactivateUI(gameOverMenu);
		deactivateUI(inGameMenu);
		deactivateUI(inventory);

		activateUI(tutorialMenu);
	}

	/// <summary>
	/// Sets the UI for the Inventory.
	/// </summary>
	private void activateInventory()
	{
		deactivateUI(itemQuickAccess);
		deactivateUI(skillQuickAccess);
		deactivateUI(map);
		deactivateUI(chat);
		deactivateUI(skillMenu);
		deactivateUI(gameOverMenu);
		deactivateUI(inGameMenu);
		deactivateUI(tutorialMenu);

		activateUI(inventory);

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	/// <summary>
	/// Sets the UI for the GameOverScreen.
	/// </summary>
	private void activateGameOverScreen()
	{
		deactivateUI(itemQuickAccess);
		deactivateUI(skillQuickAccess);
		deactivateUI(map);
		deactivateUI(chat);
		deactivateUI(skillMenu);
		deactivateUI(inventory);
		deactivateUI(inGameMenu);
		deactivateUI(tutorialMenu);

		activateUI(gameOverMenu);

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	/// <summary>
	/// deactivates an UIElement.
	/// </summary>
	/// <param name="obj">Object to deactivate</param>
	private void deactivateUI(GameObject obj)
	{
		obj.SetActive(false);
	}

	/// <summary>
	/// activates an UIElement.
	/// </summary>
	/// <param name="obj">Object to activate</param>
	private void activateUI(GameObject obj)
	{
		obj.SetActive(true);
	}

	/// <summary>
	/// Checks if chat is selected.
	/// </summary>
	/// <returns>Is chat selected</returns>
	private bool isChatSelected()
	{
		return chat.GetComponent<Multiplayer.Lobby.ChatController>().chatBox.isFocused;
	}

	/// <summary>
	/// Called when the local player object has been set up
	/// </summary>
	public override void OnStartLocalPlayer()
	{
		setupCameras();
	}

	/// <summary>
	/// Sets up playerCam and mapCam at start.
	/// </summary>
	private void setupCameras()
	{
		CameraFollow playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
		CameraFollow mapCamera = GameObject.FindGameObjectWithTag("MapCamera").GetComponent<CameraFollow>();

		// setup playerCam
		playerCamera.setupCamera(playerCamTarget.transform, this.transform);
		playerCam = playerCamera.gameObject;

		// setup mapCam
		mapCamera.setupCamera(playerCamTarget.transform, this.transform);
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
		isCollidingInAir = !isGrounded;
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

	/// <summary>
	/// Handles itemPickUp.
	/// </summary>
	/// <param name="collider"></param>
	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag.Equals("Item"))
		{
			Item item = collider.GetComponent<Item>();
			if (!itemQuickAccess.GetComponent<ItemQuickAccess>().isFull() && !item.wasPickedUp())
			{
				//  if the quick access bar is not full, collect ssitem in quickaccessbar
				itemQuickAccess.GetComponent<ItemQuickAccess>().addContent(item);
				item.pickUp();
			}
			else if (!inventory.transform.GetChild(0).GetComponent<Inventory>().IsFull())
			{
				// collect item in inventory
				inventory.transform.GetChild(0).GetComponent<Inventory>().AddItem(item);
			}
			else
			{
				// inventory and quick access bar are full
				// item is not collected
				collider.gameObject.SetActive(true);
				collider.enabled = true;
			}
		}

	}

	public void addSkillPoints(int amount)
	{
		skillMenu.GetComponent<SkillMenu>().addSkillPoints(amount);
	}

	// temp fix for gameOverManager
	public void gameOver()
	{
		activateGameOverScreen();
	}

	// temp fix for resumeScript
	public void CmdCloseMenu()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		deactivateUI(inGameMenu);
	}

	// temp fix
	public void setRole(int role)
	{
		this.role = role;
	}

	public override void OnStartServer()
	{
		RegisterAtManagers();
		base.OnStartServer();
	}

	/// <summary>
	/// Registers the player at the NPCManager and GameOverManager.
	/// </summary>
	private void RegisterAtManagers()
	{
		NPC.NPCManager.Instance.AddTarget(this.transform);
		GameOverManager.Instance.AddProf(this);
	}

	/// <summary>
	/// Toggles player visibility and NPCRegister.
	/// </summary>
	/// <param name="turnOn">Should invisibility be turned on</param>
	public void changeInvisibility(bool turnOn)
	{
		if (turnOn)
		{
			GetComponent<MeshRenderer>().enabled = false;
			NPC.NPCManager.Instance.RemoveTarget(this.transform);
		}
		else
		{
			GetComponent<MeshRenderer>().enabled = true;
			NPC.NPCManager.Instance.AddTarget(this.transform);
		}
	}

}