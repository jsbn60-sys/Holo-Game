using System.Collections;
using System.Collections.Generic;
using Multiplayer.Lobby;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

/// <summary>
/// Parent class for all playerClasses. Implements combined functionalities
/// which are UI, movement and Input.
/// </summary>
public class Player : Unit
{

	[Header("Player : Attribute")]
	[SyncVar] public string name;
	[SerializeField] private float throwSpeed;

	[Header("Player : UI")]
	[SerializeField] private GameObject itemQuickAccess;
	[SerializeField] private GameObject skillQuickAccess;
	[SerializeField] private GameObject map;
	[SerializeField] private GameObject nameText;
	[SerializeField] private GameObject chat;
	[SerializeField] private GameObject helpIcon;
	[SerializeField] private GameObject skillMenu;
	[SerializeField] private GameObject inGameMenu;
	[SerializeField] private GameObject gameOverMenu;
	[SerializeField] private GameObject tutorialMenu;
	[SerializeField] private GameObject inventory;
	[SerializeField] private GameObject deathScreen;

	[Header("Player : Attachments")]
	[SerializeField] private GameObject bulletSpawn;
	[SerializeField] private GameObject gun;
	[SerializeField] private GameObject playerCamTarget;
	private GameObject playerCam;
	private float horizontal;
	private float vertical;
	private int currPlayerToWatchIdx;

	// movement related
	private bool isGrounded;
	private bool isCollidingInAir;
	private float bonusGravity;

	private bool canAttack;

	// called at start
    protected void Start()
    {
	    //chat = LobbyManager.Instance.Chat;
		// UI
		skillMenu.GetComponent<SkillMenu>().setSkillQuickAccess(skillQuickAccess.GetComponent<SkillQuickAccess>());
		nameText.GetComponent<Text>().text = name;

		// variables
		bonusGravity = 60.0f;
		isGrounded = true;

		if (!isLocalPlayer)
		{
			deactiveAllUI();
		}
		else
		{
			setForGameplay();
		}
		base.Start();
	}

    protected void Update()
	{
		if (isLocalPlayer)
		{
			if (!isDead())
			{
				if(!isChatSelected())
				{
					moveInput();
					menuInput();
					itemInput();
					skillInput();
					shootInput();
				}
				chatInput();
			}
			else
			{
				cameraSwitchInput();
			}
		}

		base.Update();
	}

    /// <summary>
    /// Handles input related to the chat.
    /// </summary>
    private void chatInput()
    {
	    if (Input.GetKeyDown(KeyCode.Return) || (isChatSelected() && Input.GetKeyDown(KeyCode.Escape)))
	    {
		    chat.GetComponent<Chat>().toggleInput();
	    }
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
	    bool jumped = Input.GetKey(KeyCode.Space) && isGrounded && GetComponent<Rigidbody>().velocity.y < 5.0f;

	    CmdPlayerMovement(x,z,horizontal,jumped,this.transform.position,this.transform.rotation);
    }

    /// <summary>
    /// Executes the player movement on all clients.
    /// </summary>
    /// <param name="x">x movement</param>
    /// <param name="z">z movement</param>
    /// <param name="horizontal">horizontal movement</param>
    /// <param name="jumped">has the player jumped</param>
    /// <param name="actualPos">accurate position of the player</param>
    /// <param name="actualRot">accurate rotation of the player</param>
    [Command]
    private void CmdPlayerMovement(float x, float z, float horizontal, bool jumped, Vector3 actualPos, Quaternion actualRot)
    {
	    RpcPlayerMovement(x,z,horizontal,jumped,actualPos,actualRot);
    }

    /// <summary>
    /// Executes the player movement on client.
    /// If is local player updates camera, if is not local player checks network position.
    /// </summary>
    /// <param name="x">x movement</param>
    /// <param name="z">z movement</param>
    /// <param name="horizontal">horizontal movement</param>
    /// <param name="jumped">has the player jumped</param>
    /// <param name="actualPos">accurate position of the player</param>
    /// <param name="actualRot">accurate rotation of the player</param>
    [ClientRpc]
    private void RpcPlayerMovement(float x, float z, float horizontal, bool jumped, Vector3 actualPos,Quaternion actualRot)
    {
	    // rotate camera on local player
	    if (isLocalPlayer)
	    {
		    playerCamTarget.transform.rotation = Quaternion.Euler(vertical, horizontal, 0);
	    }
	    else
	    {
		    checkNetworkPosition(actualPos, actualRot);
	    }

	    // jump
	    if (jumped)
	    {
		    GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
	    }

	    // additional gravity for better jump
	    checkForAdditionalGravity(ref z, ref x);

	    // rotation and position update
	    transform.rotation = Quaternion.Euler(0, horizontal, 0);
	    transform.Translate(x, 0, z);
    }

    /// <summary>
    /// Applies additional gravity on the player movement for a better jump curve.
    /// </summary>
    /// <param name="z">z movement</param>
    /// <param name="x">x movement</param>
    private void checkForAdditionalGravity(ref float z, ref float x)
    {
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
			skillQuickAccess.GetComponent<SkillQuickAccess>().useContent(0,this);
		}


		// skill in slot Q
		if (Input.GetKeyDown(KeyCode.Q) && !isChatSelected())
		{
			skillQuickAccess.GetComponent<SkillQuickAccess>().useContent(1,this);

		}

		// skill in slot E
		if (Input.GetKeyDown(KeyCode.E) && !isChatSelected())
		{
			skillQuickAccess.GetComponent<SkillQuickAccess>().useContent(2,this);

		}

		// skill in slot R
		if (Input.GetKeyDown(KeyCode.R) && !isChatSelected())
		{
			skillQuickAccess.GetComponent<SkillQuickAccess>().useContent(3,this);
		}
	}

	/// <summary>
	/// Handles input for shooting.
	/// </summary>
	private void shootInput()
	{
		if (Input.GetKey(KeyCode.Mouse0) && readyToAttack() && canAttack)
		{
			base.useAttack();
			shoot(LobbyManager.Instance.getIdxOfPrefab(attack.gameObject),0);
		}
	}

	/// <summary>
	/// Runs on server when a player shot a projectile.
	/// This function is public, because it is used by some effects.
	/// The forward direction for the projectile is accessed through the
	/// SyncVar forwardDirection.
	/// </summary>
	/// <param name="prefabIdx">Idx of registeredPrefab that was shot</param>
	[Command]
	private void CmdShoot(int prefabIdx,float degree, Vector3 forwardDirection)
	{
		RpcShoot(prefabIdx, degree, forwardDirection);
	}

	/// <summary>
	/// Shoots projectiles on all clients.
	/// </summary>
	/// <param name="prefabIdx"></param>
	/// <param name="degree"></param>
	/// <param name="forwardDirection"></param>
	[ClientRpc]
	private void RpcShoot(int prefabIdx, float degree, Vector3 forwardDirection)
	{
		Vector3 throwDirection = Quaternion.Euler(0, degree, 0) * forwardDirection;
		Projectile projectile = LobbyManager.Instance.getPrefabAtIdx(prefabIdx).GetComponent<Projectile>();
		GameObject bullet = Instantiate(projectile.gameObject, bulletSpawn.transform.position, gun.transform.rotation * Quaternion.Euler(0f, 90f, 0f));
		bullet.GetComponent<Projectile>().setupProjectile(throwDirection,throwSpeed);
	}

	/// <summary>
	/// Throws a projectile if this player is the local player.
	/// </summary>
	/// <param name="prefabIdx">Idx of registeredPrefab</param>
	/// <param name="degree">degree from forwardDirection, that the projectile should be shoot</param>
	public void shoot(int prefabIdx, float degree)
	{
		if (isLocalPlayer)
		{
			CmdShoot(prefabIdx, degree, playerCam.transform.forward);
		}
	}

	/// <summary>
	/// Handles input related to switching cameras between players after death.
	/// </summary>
	private void cameraSwitchInput()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			currPlayerToWatchIdx++;
			updateDeathCam();
		}
	}

	/// <summary>
	/// Removes player as NPC target and tells the GameOverManager that the player is dead.
	/// </summary>
	protected override void onDeath()
	{
		if (isLocalPlayer)
		{
			updateDeathCam();
			CmdOnDeath();
		}
	}

	/// <summary>
	/// Updates the deathCam of the player after death.
	/// </summary>
	private void updateDeathCam()
	{
		GameObject[] alivePlayers = PlayerController.Instance.getAlivePlayerObjects();
		if (!PlayerController.Instance.areAllPlayersDead())
		{
			currPlayerToWatchIdx %= alivePlayers.Length;
			Player playerToWatch = alivePlayers[currPlayerToWatchIdx].GetComponent<Player>();
			activateDeathScreen();
			deathScreen.GetComponent<DeathScreen>().updatePlayerToWatchText(playerToWatch.name);
			setupCameras(playerToWatch.playerCamTarget.transform,playerToWatch.transform);
		}
		else
		{
			CmdGameOver();
		}
	}

	/// <summary>
	/// Updates all clients about the players death.
	/// </summary>
	[Command]
	private void CmdOnDeath()
	{
		RpcOnDeath();
	}

	/// <summary>
	/// Makes the player invisible.
	/// </summary>
	[ClientRpc]
	private void RpcOnDeath()
	{
		changeInvisibility(true);
	}

	/// <summary>
	/// Updates all clients that the game is over.
	/// </summary>
	[Command]
	private void CmdGameOver()
	{
		RpcGameOver();
	}

	/// <summary>
	/// Updates all clients that the game is over.
	/// </summary>
	[ClientRpc]
	private void RpcGameOver()
	{
		LobbyManager.Instance.LocalPlayerObject.GetComponent<Player>().activateGameOverScreen();
	}

	/// <summary>
	/// Sets the UI for gameplay.
	/// </summary>
	public void setForGameplay()
	{
		deactivateUI(inGameMenu);
		deactivateUI(skillMenu);
		deactivateUI(gameOverMenu);
		deactivateUI(tutorialMenu);
		deactivateUI(inventory);
		deactivateUI(deathScreen);


		activateUI(helpIcon);
		activateUI(itemQuickAccess);
		activateUI(skillQuickAccess);
		activateUI(map);
		activateUI(chat);

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		canAttack = true;
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
		deactivateUI(deathScreen);
		deactivateUI(helpIcon);

		activateUI(skillMenu);

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		canAttack = false;
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
		deactivateUI(deathScreen);
		deactivateUI(helpIcon);

		activateUI(inGameMenu);

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		canAttack = false;
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
		deactivateUI(deathScreen);

		activateUI(tutorialMenu);
		activateUI(helpIcon);

		canAttack = false;
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
		deactivateUI(deathScreen);
		deactivateUI(helpIcon);

		activateUI(inventory);

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		canAttack = false;
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
		deactivateUI(deathScreen);
		deactivateUI(helpIcon);

		activateUI(gameOverMenu);

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		canAttack = false;
	}

	private void activateDeathScreen()
	{
		deactivateUI(itemQuickAccess);
		deactivateUI(skillQuickAccess);
		deactivateUI(map);
		deactivateUI(chat);
		deactivateUI(skillMenu);
		deactivateUI(inventory);
		deactivateUI(inGameMenu);
		deactivateUI(tutorialMenu);
		deactivateUI(gameOverMenu);
		deactivateUI(helpIcon);

		activateUI(deathScreen);


		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		canAttack = false;
	}

	private void deactiveAllUI()
	{
		deactivateUI(itemQuickAccess);
		deactivateUI(skillQuickAccess);
		deactivateUI(map);
		deactivateUI(chat);
		deactivateUI(skillMenu);
		deactivateUI(inventory);
		deactivateUI(inGameMenu);
		deactivateUI(tutorialMenu);
		deactivateUI(gameOverMenu);
		deactivateUI(deathScreen);
		deactivateUI(helpIcon);
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
		return chat.GetComponent<Chat>().ChatIsSelected;
	}

	/// <summary>
	/// Called when the local player object has been set up.
	/// </summary>
	public override void OnStartLocalPlayer()
	{
		setupCameras(playerCamTarget.transform,this.transform);
		nameText.GetComponent<Text>().text = name;
		LobbyManager.Instance.LocalPlayerObject = this.gameObject;
	}

	/// <summary>
	/// Sets up playerCam and mapCam at start.
	/// </summary>
	private void setupCameras(Transform playerTarget, Transform player)
	{
		CameraFollow playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
		CameraFollow mapCamera = GameObject.FindGameObjectWithTag("MapCamera").GetComponent<CameraFollow>();

		// setup playerCam
		playerCamera.setupCamera(playerTarget, player);
		playerCam = playerCamera.gameObject;

		// setup mapCam
		mapCamera.setupCamera(playerTarget,player);
	}


	/// <summary>
	/// Players can only push NPCs.
	/// </summary>
	/// <param name="target">Target that collided</param>
	/// <returns>If the target was an npc</returns>
	protected override bool canPushTarget(Unit target)
	{
		return target.tag.Equals("NPC");
	}

	/// <summary>
	/// Updates airCollision and isGrounded variable.
	/// </summary>
	/// <param name="collision">Any collision</param>
	private void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);
		if (collision.gameObject.tag.Equals("Plane"))
		{
			isGrounded = true;
		}
		isCollidingInAir = !isGrounded;
	}

	/// <summary>
	/// Updates isGrounded variable.
	/// </summary>
	/// <param name="collision">Any collision</param>
	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag.Equals("Plane"))
		{
			isGrounded = false;
		}
	}

	/// <summary>
	/// WIP: Handles itemPickUp.
	/// </summary>
	/// <param name="collider"></param>
	private void OnTriggerEnter(Collider collider)
	{
		if (isLocalPlayer && collider.gameObject.tag.Equals("Item"))
		{
			Item item = collider.GetComponent<Item>();
			if (!itemQuickAccess.GetComponent<ItemQuickAccess>().isFull() && !item.wasPickedUp())
			{
				// workaround: pickUpItem is called early local, so the player doesnt pick the item up twice.
				// if the quick access bar is not full, collect item in quickaccessbar
				itemQuickAccess.GetComponent<ItemQuickAccess>().addContent(item);
				item.pickUpItem();
				CmdPickUpItem(item.gameObject);
			}
			else if (!inventory.transform.GetChild(0).GetComponent<Inventory>().IsFull())
			{
				// collect item in inventory
				inventory.transform.GetChild(0).GetComponent<Inventory>().AddItem(item);
				CmdPickUpItem(item.gameObject);
			}
		}
	}

	/// <summary>
	/// Adds skill points to this player.
	/// </summary>
	/// <param name="amount">Amount of skill points to add</param>
	public void addSkillPoints(int amount)
	{
		skillMenu.GetComponent<SkillMenu>().addSkillPoints(amount);
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
			isInvisible = true;
		}
		else
		{
			GetComponent<MeshRenderer>().enabled = true;
			isInvisible = false;
		}
	}

	/// <summary>
	/// WIP: Used for spawning gameobject infront of player.
	/// Is not spawned with NetworkServer.Spawn(), because
	/// effects are executed locally and this would cause
	/// duplicates.
	/// </summary>
	/// <param name="objectToPlace"></param>
	[Command]
	public void CmdPlaceObjectInfront(int prefabIdx)
	{
		Vector3 spawnPos = transform.position + transform.forward * 3f;
		spawnPos.y = 0f;
		GameObject spawnedObject = Instantiate(LobbyManager.Instance.getPrefabAtIdx(prefabIdx), spawnPos , this.transform.rotation);
		if (spawnedObject.tag.Equals("Dummy"))
		{
			NetworkServer.SpawnWithClientAuthority(spawnedObject, gameObject);
		}
		else
		{
			NetworkServer.Spawn(spawnedObject);
		}
	}

	/// <summary>
	/// Runs on server, when an item was picked up.
	/// </summary>
	/// <param name="item">Item that was picked up</param>
	[Command]
	public void CmdPickUpItem(GameObject item)
	{
		RpcPickUpItem(item);
	}

	/// <summary>
	/// Runs on all clients, when an item was picked up.
	/// </summary>
	/// <param name="item">Item that was picked up</param>
	[ClientRpc]
	public void RpcPickUpItem(GameObject item)
	{
		item.GetComponent<Item>().pickUpItem();
	}

	/// <summary>
	/// Changes the speed at which projectiles are thrown.
	/// </summary>
	/// <param name="factor">Factor by which throw speed is changed</param>
	public void changeThrowSpeed(float factor)
	{
		throwSpeed *= factor;
	}

	/// <summary>
	/// Spawns an object on the network.
	/// </summary>
	/// <param name="prefabIdx">Idx of prefab to spawn</param>
	/// <param name="spawnPos">Position at which to spawn object</param>
	/// <param name="spawnRotation">Rotation of the object</param>
	[Command]
	public void CmdSpawn(int prefabIdx, Vector3 spawnPos, Quaternion spawnRotation)
	{
		GameObject objectPrefab = LobbyManager.Instance.getPrefabAtIdx(prefabIdx);
		GameObject objectCopy = Instantiate(objectPrefab, spawnPos, spawnRotation);
		NetworkServer.Spawn(objectCopy);
	}

	/// <summary>
	/// Sends a text message to all clients.
	/// </summary>
	/// <param name="sender">Player that send the message</param>
	/// <param name="message">Message that was send</param>
	[Command]
	public void CmdSendTextMessage(string sender, string message)
	{
		RpcSendTextMessage(sender,message);
	}

	/// <summary>
	/// Sends a message to all clients.
	/// Updates the chat of the local player.
	/// </summary>
	/// <param name="sender">Player that send the message</param>
	/// <param name="message">Message that was send</param>
	[ClientRpc]
	public void RpcSendTextMessage(string sender, string message)
	{
		LobbyManager.Instance.LocalPlayerObject.GetComponent<Player>().chat.GetComponent<Chat>().postMessage(sender, message);
	}
}
