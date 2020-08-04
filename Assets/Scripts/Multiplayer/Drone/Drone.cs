/* edited by: SWT-P_WS_2018_Holo */

using System;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
using Multiplayer.Lobby;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// This class represents the drone, a flying object which is controlled by one Player.
/// The drone can be played on a normal Monitor or on the Holoboard.
/// The drone inherits from the Unit, so it can be integrated into the effect system,
/// even though it does not use many functionalities of the unit class.
///
/// The drone can not be played alone, atleast 2 players are required.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Drone : Unit, IAircraft
{
	// this variables are to alter the drone movement behaviour
	[SyncVar] public bool usesHoloboard = false;
	public GameObject holoboardCamera;
	public float tiltangle;
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

	[SerializeField] private Vector3 attackSpawnOffset;
	[SerializeField] private float dropSpeed;
	[SerializeField] private SkillQuickAccess skillQuickAccess;

	private GameObject spawnItem;

	[SerializeField] private DroneSkillMenu droneSkillMenu;

	public DroneSkillMenu DroneSkillMenu => droneSkillMenu;

	public SkillQuickAccess SkillQuickAccess => skillQuickAccess;

	/// <summary>
	/// Register Drone as Player
	/// </summary>
	void Start()
	{
		/*
		SkillMenu skillMenu = GameObject.FindWithTag("SkillMenu").GetComponent<SkillMenu>();
		skillMenu.menu.SetActive(false);
		*/
		rb = GetComponent<Rigidbody>();
		rbc = rb.constraints;

		if (!isLocalPlayer)
		{
			//unfreeze vertical movement for every other player,
			//otherwise the drone won't lift on other players,
			//because unity won't sync RigidBody.Constraints on network
			GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;
		}

		DroneSkillMenu.setSkillQuickAccess(skillQuickAccess);
	}

	/// <summary>
	/// The drone has no usual basic attack.
	/// </summary>
	protected override void execAttack() { }

	/// <summary>
	/// The drone can not die.
	/// If all other player's die the game ends.
	/// </summary>
	protected override void onDeath() { }

	/// <summary>
	/// The drone cannot push anything.
	/// </summary>
	/// <param name="target">Unit that collided</param>
	/// <returns>False</returns>
	protected override bool canPushTarget(Unit target)
	{
		return false;
	}

	/// <summary>
	/// The drone does not apply any effects on hit.
	/// </summary>
	protected override void hitEffects() { }

	/// <summary>
	/// Effects are only applied on the drone, if it is the local player.
	/// This is needed for the drone skills.
	/// </summary>
	/// <returns>Is this player the local player</returns>
	protected override bool canBeHit()
	{
		return isLocalPlayer;
	}

	/// <summary>
	/// Updates movement.
	/// </summary>
	void FixedUpdate()
	{
		if (!isLocalPlayer) return;


		//throttle velocity from added force
		rb.velocity = Vector3.ClampMagnitude(rb.velocity, Mathf.Lerp(rb.velocity.magnitude, 10f, Time.deltaTime * 5f));

		//tilt drone
		rb.MoveRotation(Quaternion.Euler(pitch, yaw, -roll));
	}

	///<summary>
	/// Activate droneAccesBar and deactivate Map
	/// </summary>
	public override void OnStartLocalPlayer()
	{
		LobbyManager.Instance.LocalPlayerObject = this.gameObject;
		GameObject.FindGameObjectWithTag("Map").SetActive(false);

		if (usesHoloboard)
		{
			// when using the holoboad camera perspective
			// disable main camera and then activate the holoboard camera which is disabled by default.
			Camera.main.gameObject.SetActive(false);
			holoboardCamera.SetActive(true);
		}
		else
		{
			//register Drone as main camera target
			Camera.main.gameObject.GetComponent<DroneCameraFollow>().target = transform;
		}
	}


	/// <summary>
	/// Controls the pitch (the leaning forward and backwards) of the drone.
	/// </summary>
	/// <param name="pitch">Input</param>
	void IAircraft.Pitch(float pitch)
	{
		if (!isLocalPlayer)
		{
			return;
		}

		if (usesHoloboard)
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

	/// <summary>
	/// Controls the yaw (the rotation around its y-axis) of the drone.
	/// </summary>
	/// <param name="yaw">Input</param>
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

	/// <summary>
	/// Controls the roll (the leaning left and right) of the drone.
	/// </summary>
	/// <param name="roll">Input</param>
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

	/// <summary>
	/// Controls the lift (going up and down) of the drone.
	/// </summary>
	/// <param name="lift">Input</param>
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

	/// <summary>
	/// Not used anymore.
	/// </summary>
	void IAircraft.Drop() { }

	/// <summary>
	/// Drops an object below the drone.
	/// </summary>
	/// <param name="prefabIdx">Idx of the prefab</param>
	[Command]
	public void CmdDrop(int prefabIdx)
	{
		RpcDrop(prefabIdx);
	}

	/// <summary>
	/// Drops an object below the drone.
	/// </summary>
	/// <param name="prefabIdx">Idx of the prefab</param>
	[ClientRpc]
	public void RpcDrop(int prefabIdx)
	{
		GameObject copy = Instantiate(LobbyManager.Instance.getPrefabAtIdx(prefabIdx),
			this.transform.position + attackSpawnOffset,
			Quaternion.identity);
	}

	/// <summary>
	/// Shoots an projectile downwards with a given angle.
	/// </summary>
	/// <param name="prefabIdx">Idx of the projectile to shoot</param>
	/// <param name="xDegree">angle on x-axis</param>
	/// <param name="zDegree">angle on z-axis</param>
	[Command]
	public void CmdShoot(int prefabIdx, float xDegree, float zDegree)
	{
		RpcShoot(prefabIdx,xDegree,zDegree);
	}

	/// <summary>
	/// Shoots an projectile downwards with a given angle.
	/// </summary>
	/// <param name="prefabIdx">Idx of the projectile to shoot</param>
	/// <param name="xDegree">angle on x-axis</param>
	/// <param name="zDegree">angle on z-axis</param>
	[ClientRpc]
	private void RpcShoot(int prefabIdx, float xDegree, float zDegree)
	{
		Quaternion quaternion = Quaternion.Euler(xDegree,0f,zDegree);
		Projectile attackCopy =
			Instantiate(LobbyManager.Instance.getPrefabAtIdx(prefabIdx), this.transform.position + attackSpawnOffset,
					Quaternion.identity)
				.GetComponent<Projectile>();
		attackCopy.setupProjectile(Vector3.down.RotatedBy(quaternion), dropSpeed);
	}
}
