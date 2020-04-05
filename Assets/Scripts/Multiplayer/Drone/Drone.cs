/* edited by: SWT-P_WS_2018_Holo */
using NPC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// This class represents the drone, a flying object which is controlled by one Player.
/// The drone can be played on a normal Monitor or on the Holoboard.
/// Playing on the holoboard is indicated by the usesHoloboard boolean flag and
/// requires the existance of the holoboard camera in the scene.
///
/// The flying behaviour can be altered by changing the variables, which are set by the Unity Inspector.
///		tiltangle, speed, min- and max Altitude.
///
/// The movement is simulated by the physics engine, which requires an RigidBody Component.
/// Note, that the RigidBody-Constrains are set by this script, to maintain the hover effect and the flightboundary.
///
/// See also the IAircraft Interface for the flight commands.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Drone : NetworkBehaviour, IAircraft
{
	// this variables are to alter the drone movement behaviour
	[SyncVar]
	public bool usesHoloboard = false;
	public GameObject holoboardCamera;
	public float tiltangle;
	public float speed;
	public float minAltitude;
	public float maxAltitude;
	// this variables are temporary variables, which will be altered by the flight calculation
	private float pitch;
	private float yaw;
	private float roll;
	private Vector3 angularspeed;
	private Rigidbody rb;
	// this variable is used to reset the constraints after flight command
	private RigidbodyConstraints rbc;
	[SerializeField]

	//these variables are for managing the inventory
	public GameObject droneAccessBar;
	public Sprite MNISprite;
	public Sprite LSESprite;
	public Sprite GESSprite;
	public Sprite WIRSprite;
	public List<GameObject> itemPrefabs = new List<GameObject>();
	private GameObject spawnItem;


	/// <summary>
	/// Riegister Drone as Player
	/// </summary>
	void Start()
	{
		/*
		SkillMenu skillMenu = GameObject.FindWithTag("SkillMenu").GetComponent<SkillMenu>();
		skillMenu.menu.SetActive(false);
		*/

		for (int i = 0; i < itemPrefabs.Count; i++) {
			ClientScene.RegisterPrefab(itemPrefabs[i]);
		}
		rb = GetComponent<Rigidbody>();
		rbc = rb.constraints;

		if (!isLocalPlayer)
		{
            //unfreeze vertical movement for every other player,
			//otherwise the drone won't lift on other players,
			//because unity won't sync RigidBody.Constraints on network
            GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;
		}
	}


	void FixedUpdate()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		//throttle velocity from added force
		rb.velocity = Vector3.ClampMagnitude(rb.velocity, Mathf.Lerp(rb.velocity.magnitude, 10f, Time.deltaTime * 5f));

		//tilt drone
		rb.MoveRotation(Quaternion.Euler(pitch, yaw, -roll));
	}

	public override void OnStartServer()
	{
		base.OnStartServer();
	}


	///<summary>
	/// Activate droneAccesBar and deactivate Map
	/// </summary>
	public override void OnStartLocalPlayer()
	{
		droneAccessBar.SetActive(true);
		StartCoroutine(UpdateCameraTarget(usesHoloboard, holoboardCamera, transform));
		GameObject.FindGameObjectWithTag("Map").SetActive(false);
	}

	/// <summary>
	/// This is a coroutine, which will register the drone as target for the appropiate camera. 
	/// </summary>
	/// <param name="usesHoloboard">Indicates if the holoboard camera shall be used</param>
	/// <param name="holoboardCamera">The holoboard camera</param>
	/// <param name="target">The transform of the drone as camera target</param>
	/// <returns></returns>
	private static IEnumerator UpdateCameraTarget(bool usesHoloboard, GameObject holoboardCamera, Transform target)
	{
		yield return null;

		if (usesHoloboard)
		{
			//when using the holoboad camera perspective
			//disable main camera and then activate the holoboard camera which is disabled by default.
			GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
			holoboardCamera.SetActive(true);
		}
		else
		{
			//register Drone as main camera target
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<DroneCameraFollow>().target = target;
		}
	}

	void IAircraft.Pitch(float pitch)
	{
		if (!isLocalPlayer)
		{
			return;
		}

		if(usesHoloboard)
		{
			//inverting pitch value when on holoboard, otherwise a forward movement
			//would result in a visible backward movement in front of the leap motion.
			pitch = pitch * -1;
		}

		if (!pitch.Equals(0))
		{
			rb.AddRelativeForce(Vector3.forward * pitch * speed);
			//calculate new x rotation after this maneuver
			this.pitch = Mathf.SmoothDamp(this.pitch, pitch * 20, ref angularspeed.x, 0.1f, tiltangle);
		}
        else
        {
	        //revert slowly to original initial pitch
            this.pitch = Mathf.Lerp(this.pitch, 0, Time.deltaTime * 3.0f);
        }
	}

	void IAircraft.Yaw(float yaw)
	{
		if (!isLocalPlayer)
		{
			return;
		}
        if (usesHoloboard)
        {
            //inverting yaw value when on holoboard, otherwise a clockwise rotation
            //would result in a visible counter clockwise rotation in front of the leap motion.
            yaw = yaw * -1;
        }
        //calculate new y rotation after this maneuver
        this.yaw = Mathf.SmoothDamp(this.yaw, this.yaw + yaw, ref angularspeed.y, 0.025f);
	}

	void IAircraft.Roll(float roll)
	{
		if (!isLocalPlayer)
		{
			return;
		}

		if (!roll.Equals(0))
		{
            rb.AddRelativeForce(Vector3.right * roll * speed);
			//calculate new z rotation after this maneuver
			this.roll = Mathf.SmoothDamp(this.roll, roll * 20, ref angularspeed.z, 0.1f, tiltangle);
		}
        else
		{
	        //revert slowly to original initial pitch
            this.roll = Mathf.Lerp(this.roll, 0, Time.deltaTime * 3.0f);
        }
	}

	void IAircraft.Lift(float lift)
	{
		if (!isLocalPlayer)
		{
			return;
		}

		//freeze lift until lift is commanded and lift does not increase distance to allowed flightzone
        if (lift.Equals(0) || (rb.position.y > maxAltitude & lift > 0) || (rb.position.y < minAltitude & lift < 0))
        {
            //restore initial constraint state e.g. freezed Y position
            //prevents sinking of drone while moving horizontal
            //and moving out of specified flightzone
            rb.constraints = rbc;
        }
        else
        {
            //unfreeze vertical movement, while lift is applied
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            rb.AddForce(Vector3.up * lift * speed);
        }
	}
	
	void IAircraft.Drop(){
		DroneAccessBar dab = droneAccessBar.GetComponent<DroneAccessBar>();
		dab.Drop();
	}



	///<summary>
	/// Considered two options:
	/// 1) The drone flies to the Dockingstation. Then an item is created.
	/// 2) An NPC enters the Collider drone.This NPC will get the StartTime and Flag trigered.
	/// </summary>
	/// <param name="collider">The collider that collided with the drone</param>
	private void OnTriggerEnter(Collider collider)
	{
		// if the drone docked at the docking station
		if (collider.tag == "DockingStation")
		{
			// craft the students to a new item
			GameObject craftItem = collider.GetComponent<DockingStation>().Craft(droneAccessBar.GetComponent<DroneAccessBar>().students);
			 // clear the students from the inventory to give the possibility to collect new students
			droneAccessBar.transform.GetComponent<DroneAccessBar>().ClearStudentList();
			if (craftItem != null)
			{
				droneAccessBar.transform.GetComponent<DroneAccessBar>().CollectCraftedItem(craftItem);
			}
		}

		// if the collider is from an NPC
		if (collider.tag == "Enemy")
		{
			// only for standard enemies
			if (collider.GetComponent<NPC.NPC>().type < 4){
				collider.gameObject.GetComponent<Renderer>().material = collider.GetComponent<NPC.NPC>().trans; 			
				collider.GetComponent<NPC.NPC>().SetTriggered(true);
				collider.GetComponent<NPC.NPC>().SetTime();
			}
		}
	}

	///<summary>
	/// If the NPC is trigered and the time is more than 3 seconds, it moves to the DroneAccesbar.
	/// </summary>
	/// <param name="collider">The collider that collided with the drone</param>
	void OnTriggerStay(Collider collider)
	{
		if (collider.tag == "Enemy")
		{
			if (collider.GetComponent<NPC.NPC>().GetTrigered())
			{
				if (collider.GetComponent<NPC.NPC>().GetTime() > 2)
				{
					if (!droneAccessBar.GetComponent<DroneAccessBar>().IsFull())
					{
						Sprite sprite = collider.GetComponent<NPC.NPC>().sprite;
						short type = collider.GetComponent<NPC.NPC>().type;
						int posNumber = droneAccessBar.transform.GetComponent<DroneAccessBar>().AddStudent(type, sprite);
						Destroy(collider.gameObject); // remove the student from map
						CmdSendToServer(posNumber, type);
					}
				}
			}
		}
	}


	///<summary>
	/// if the NPC is no longer in the focus of the drone, material and trigger are set back to normal.
	/// </summary>
	/// <param name="collider">The collider that collided with the drone</param>
	void OnTriggerExit(Collider collider)
	{
		if (collider.tag == "Enemy"){
			if (collider.GetComponent<NPC.NPC>().type < 4){
				collider.gameObject.GetComponent<Renderer>().material = collider.GetComponent<NPC.NPC>().normal; ;
				collider.GetComponent<NPC.NPC>().SetTriggered(false);
				collider.GetComponent<NPC.NPC>().SetTime();
			}
		}
	}


	///<summary>
	/// Changes the state of the DroneAccesBar if the drone player is not the host.
	/// Adds the sprite of the collected NPC to the DroneAccessbar.
	/// </summary>
	/// <param name="posNumber">Slot number</param>
	/// <param name="type">Type of the student</param>
	[ClientRpc]
	void RpcSetStudent(int posNumber, int type)
	{
		Transform slot = droneAccessBar.transform.GetChild(posNumber);
		if (slot.childCount < 2)
		{
			GameObject itemImage = Instantiate(droneAccessBar.transform.GetComponent<DroneAccessBar>().GetItemImagePrefab());
			itemImage.transform.SetParent(slot, false);

			switch (type) {
				case 0: slot.transform.GetChild(1).transform.GetChild(1).transform.GetComponent<Image>().sprite = MNISprite;
					break;
				case 1: slot.transform.GetChild(1).transform.GetChild(1).transform.GetComponent<Image>().sprite = LSESprite;
					break;
				case 2: slot.transform.GetChild(1).transform.GetChild(1).transform.GetComponent<Image>().sprite = WIRSprite;
					break;
				case 3: slot.transform.GetChild(1).transform.GetChild(1).transform.GetComponent<Image>().sprite = GESSprite;
					break;
			}
		}
		droneAccessBar.transform.GetComponent<DroneAccessBar>().students[posNumber] = type;
	}


	///<summary>
	/// Changes the state of DroneAccesBar on the server.
	/// Adds the sprite of the collected NPC to the DroneAccessbar.
	/// </summary>
	/// <param name="posNumber">Slot number</param>
	/// <param name="type">Type of the student</param>
	[Command]
	public void CmdSendToServer(int posNumber, int type)
	{
		Transform slot = droneAccessBar.transform.GetChild(posNumber);
		if (slot.childCount < 2){
		GameObject itemImage = Instantiate(droneAccessBar.transform.GetComponent<DroneAccessBar>().GetItemImagePrefab());
		itemImage.transform.SetParent(slot, false);
			switch (type)
			{
				case 0:
					slot.transform.GetChild(1).transform.GetChild(1).transform.GetComponent<Image>().sprite = MNISprite;
					break;
				case 1:
					slot.transform.GetChild(1).transform.GetChild(1).transform.GetComponent<Image>().sprite = LSESprite;
					break;
				case 2:
					slot.transform.GetChild(1).transform.GetChild(1).transform.GetComponent<Image>().sprite = WIRSprite;
					break;
				case 3:
					slot.transform.GetChild(1).transform.GetChild(1).transform.GetComponent<Image>().sprite = GESSprite;
					break;
			}

		}
		droneAccessBar.transform.GetComponent<DroneAccessBar>().students[posNumber] = type;
		RpcSetStudent(posNumber, type);
	}


	///<summary>
	/// The created Item is dropped on the map.
	/// </summary>
	[Command]
	public void CmdDrop()
	{
		GameObject dockingStation = GameObject.FindGameObjectWithTag("DockingStation");
		Vector3 spawn = gameObject.transform.position + Vector3.down;
		switch (dockingStation.GetComponent<DockingStation>().GetLastCraftNumber()) { 
			case "0":
					// distraction
					spawnItem = Instantiate(itemPrefabs[0], spawn, Quaternion.identity);
					break;

			case "1":
				// globalStun
				spawnItem = Instantiate(itemPrefabs[1], spawn, Quaternion.identity);
				break;


			case "2":
					// healAll
					spawnItem = Instantiate(itemPrefabs[2], spawn, Quaternion.identity);
					break;


			case "3":
					// oneHit
					spawnItem = Instantiate(itemPrefabs[3], spawn, Quaternion.identity);
					break;


			case "4":
					// orientationLoss
					spawnItem = Instantiate(itemPrefabs[4], spawn, Quaternion.identity);	
					break;


			case "5":
					// speedBoost
					spawnItem = Instantiate(itemPrefabs[5], spawn, Quaternion.identity);
					break;
		}
		dockingStation.GetComponent<DockingStation>().SetLastCraftNumber(); // reset
		NetworkServer.Spawn(spawnItem);
	}
}
