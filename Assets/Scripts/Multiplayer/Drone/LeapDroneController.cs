/* edited by: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

/// <summary>
/// This class will allow the Drone to be moved by the
/// Leap Motion.
/// It is required, that the Drone object is assigned the
/// the Drone-Component and tagged with "Drone".
/// </summary>
public class LeapDroneController : MonoBehaviour{

    Controller controller;
    IAircraft aircraft;


    // Use this for initialization
    void Start () {
        controller = new Controller();
        aircraft = GameObject.FindGameObjectWithTag("Drone")
            .GetComponent<Drone>();
    }
	
    private void FixedUpdate()
    {
        Frame frame = controller.Frame();

		// Check if hands are detected by the leap motion
        if (frame.Hands.Count >= 1)
        {

            List<Hand> hands = frame.Hands;
            Hand firstHand = hands[0];

			// fetch values from leap motion and apply low pass filter
            float mapedLift = MapInterval(firstHand.PalmPosition.y, 60, 260, -20F, 20F);
			
			// check if the hand is a fist
			IsDropping(firstHand.GrabAngle);

			// apply drone commands
            aircraft.Lift(mapedLift);
            aircraft.Pitch(-firstHand.Direction.Pitch * 0.6F);
            aircraft.Yaw(firstHand.Direction.Yaw * 1.5F);
            aircraft.Roll(-firstHand.PalmNormal.Roll);

        }
        else
        {
			//TODO: display hand symbol after ca. 3 seconds
            //Debug.Log("[!] No hands recognized");
        }
    }

	/// <summary>
	/// A Low-Pass-Filter to map the ranges from the Leap Motion to the required -1 to 1 range for the Drone.
	/// </summary>
	/// <param name="val"></param>
	/// <param name="srcMin"></param>
	/// <param name="srcMax"></param>
	/// <param name="dstMin"></param>
	/// <param name="dstMax"></param>
	/// <returns></returns>
    float MapInterval(float val, float srcMin, float srcMax, float dstMin, float dstMax)
    {
        if (val >= srcMax) return dstMax;
        if (val <= srcMin) return dstMin;
        return dstMin + (val - srcMin) / (srcMax - srcMin) * (dstMax - dstMin);
    }
	
	
	/// <summary>
	/// if the angle between the fingers and the hand is >= 90 degrees, the gesture to drop an item is detected
	/// </summary>
	/// <param name="grabAngle"></param>
	/// <returns></returns>
	void IsDropping(float grabAngle){
		if(grabAngle >= 2.9f){
			aircraft.Drop();
		}
	}		

}
