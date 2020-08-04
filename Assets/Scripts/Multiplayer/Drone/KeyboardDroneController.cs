/* edited by: SWT-P_WS_2018_Holo */

using UnityEngine;

/// <summary>
/// This class will allow the Drone to be moved by a
/// normal desktop keyboard.
/// It is required, that the Drone object is assigned the
/// the Drone-Component and tagged with "Drone".
///
/// The default Key-Layout can be alterted in Unitys Input handler.
/// </summary>
public class KeyboardDroneController : MonoBehaviour
{
	private Drone drone;

	/// <summary>
	/// Start is called before the first frame update.
	/// </summary>
	private void Start()
	{
		drone = GetComponent<Drone>();
	}

	/// <summary>
	/// Updates Input.
	/// </summary>
	private void FixedUpdate()
	{
		drone.GetComponent<IAircraft>().Lift(Input.GetAxis("AircraftLift"));
		drone.GetComponent<IAircraft>().Pitch(Input.GetAxis("AircraftPitch"));
		drone.GetComponent<IAircraft>().Yaw(Input.GetAxis("AircraftYaw"));
		drone.GetComponent<IAircraft>().Roll(Input.GetAxis("AircraftRoll"));

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			drone.DroneSkillMenu.gameObject.SetActive(!drone.DroneSkillMenu.gameObject.activeSelf);

			if (drone.DroneSkillMenu.gameObject.activeSelf)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			else
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}


		// for slot 0
		if (Input.GetKeyDown(KeyCode.Space))
		{
			drone.SkillQuickAccess.useContent(0, drone);
		}

		// for slot 1 - 4
		for (int i = 1; i <= 4; i++)
		{
			if (Input.GetKeyDown("" + i))
			{
				drone.SkillQuickAccess.useContent(i, drone);
			}
		}
	}
}
