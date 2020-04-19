/* edited by: SWT-P_SS_2019_Holo */
using UnityEngine;

///<summary>
/// This class Handles the Main Camera that follows the Player and the Map Camera that is Responsible for the MiniMap
/// The Camera uses the rotation and Position of either the Player for the Map Camera, or a target object that is a child of the Player Object
/// for the Main Camera
///</summary>
public class CameraFollow : MonoBehaviour
{
	public Transform target, player;
	public float distance = 10.0f;
	public float height = 5.0f;
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;

	[SerializeField]
	private bool isMain;

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

	public void setupCamera(Transform target, Transform player)
	{
		this.target = target;
		this.player = player;
	}

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
			currentRotationAngle_x = Mathf.LerpAngle(currentRotationAngle_x, wantedRotationAngle_x, rotationDamping * Time.deltaTime);
		}

		currentRotationAngle_y = Mathf.LerpAngle(currentRotationAngle_y, wantedRotationAngle_y, rotationDamping * Time.deltaTime);

		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

		var currentRotation = Quaternion.Euler(currentRotationAngle_x, currentRotationAngle_y, 0);

		transform.position = target.position;
		transform.position -= currentRotation * new Vector3(0, 0, 1) * distance;

		// only needed for map Camera
		if (!isMain)
		{
			transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
		}

		transform.LookAt(target);
	}
}
