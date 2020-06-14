/* edited by: SWT-P_SS_2019_Holo */

using System;
using UnityEngine;
using Random = UnityEngine.Random;

///<summary>
/// This class Handles the Main Camera that follows the Player and the Map Camera that is Responsible for the MiniMap
/// The Camera uses the rotation and Position of either the Player for the Map Camera, or a target object that is a child of the Player Object
/// for the Main Camera.
/// It also handles the shaking the camera.
///</summary>
public class CameraController : MonoBehaviour
{
	[Header("Camera : Follow")]
	[SerializeField] private bool isMain;
	[SerializeField] private float distance;
	[SerializeField] private float height;
	[SerializeField] private float heightDamping;
	[SerializeField] private float rotationDamping;

	[Header("Camera : Shake")]
	private float shakeAmount;
	private Transform target, player;
	private Vector3 originalPos;
	private float shakeTimer;

	/// <summary>
	/// Start is called before the first frame update.
	/// </summary>
	private void Start()
	{
		shakeTimer = 0f;
	}

	/// <summary>
	/// Uses late update to be correctly synced to playerPos.
	/// </summary>
	void LateUpdate()
	{
		if (!target) return;

		if (!isMain)
		{
			Camera(player);
		}
		else if (isMain)
		{
			Camera(target);
		}
	}

	/// <summary>
	/// Sets up cameras for localPlayer.
	/// </summary>
	/// <param name="target">Target of player to follow</param>
	/// <param name="player">Player to follow</param>
	public void setupCamera(Transform target, Transform player)
	{
		this.target = target;
		this.player = player;
	}

	/// <summary>
	/// Updates the camera so it follows the player or target.
	/// Also updates the shake timer.
	/// </summary>
	/// <param name="target">Target to follow</param>
	void Camera(Transform target)
	{
		float wantedRotationAngle_y = target.eulerAngles.y;
		float wantedHeight = target.position.y + height;

		float currentRotationAngle_y = transform.eulerAngles.y;
		float currentHeight = transform.position.y;

		float currentRotationAngle_x = 0.0f;

		// x Rotation only needed for Main Camera
		if (isMain)
		{
			float wantedRotationAngle_x = target.eulerAngles.x;
			currentRotationAngle_x = transform.eulerAngles.x;
			currentRotationAngle_x = Mathf.LerpAngle(currentRotationAngle_x, wantedRotationAngle_x,
				rotationDamping * Time.deltaTime);
		}

		currentRotationAngle_y = Mathf.LerpAngle(currentRotationAngle_y, wantedRotationAngle_y,
			rotationDamping * Time.deltaTime);

		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

		var currentRotation = Quaternion.Euler(currentRotationAngle_x, currentRotationAngle_y, 0);

		transform.position = target.position;

		if (shakeTimer > 0f)
		{
			transform.position += Random.insideUnitSphere * shakeAmount;

			shakeTimer -= Time.deltaTime;

			if (shakeTimer <= 0f)
			{
				shakeAmount = 0f;
			}
		}

		transform.position -= currentRotation * new Vector3(0, 0, 1) * distance;

		// only needed for map Camera
		if (!isMain)
		{
			transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
		}

		transform.LookAt(target);
	}

	/// <summary>
	/// Causes a shake effect.
	/// Shake amount and shake duration are added onto previous values,
	/// if there happen to be multiple shakes at one time.
	/// </summary>
	/// <param name="shakeAmount">Amount to shake</param>
	/// <param name="shakeDuration">Duration of shake</param>
	public void Shake(float shakeAmount, float shakeDuration)
	{
		shakeTimer += shakeDuration;
		this.shakeAmount += shakeAmount;
	}
}
