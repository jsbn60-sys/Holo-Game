using UnityEngine;

/// <summary>
/// This class will control the movement of the main camera when using the drone.
/// The Camera will maintain a 90 degree viewing angle resulting in a "god view".
/// </summary>
public class DroneCameraFollow : MonoBehaviour
{
	// The target of the camera, which might be set in the inspector.
	// In this special case the drone.
	public Transform target;

	[SerializeField] private Vector3 positionOffset;
	[SerializeField] private Quaternion rotationOffset;

	void LateUpdate()
	{
		if (!target)
		{
			return;
		}

		//set position and rotation of the camera to the target, while maintaining the "god view"
		transform.position = target.position + positionOffset;
		transform.rotation =
			Quaternion.Inverse(new Quaternion(target.rotation.x, -target.rotation.y, target.rotation.z, target.rotation.w)) *
			Quaternion.Euler(rotationOffset.x, rotationOffset.y, rotationOffset.z);
	}
}
