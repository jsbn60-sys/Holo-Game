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
public class KeyboardDroneController: MonoBehaviour
{
	IAircraft aircraft;

	private void Start()
	{
		aircraft = GameObject.FindGameObjectWithTag("Drone")
			.GetComponent<Drone>();
	}

	private void FixedUpdate()
	{
        aircraft.Lift(Input.GetAxis("AircraftLift"));
		aircraft.Pitch(Input.GetAxis("AircraftPitch"));
		aircraft.Yaw(Input.GetAxis("AircraftYaw"));
		aircraft.Roll(Input.GetAxis("AircraftRoll"));
		
		// checking if space was hit to drop an item
		if (Input.GetKeyDown("space")) aircraft.Drop(); 
	}
}
