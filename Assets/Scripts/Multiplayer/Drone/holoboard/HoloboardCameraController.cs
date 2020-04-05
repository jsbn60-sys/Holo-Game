using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloboardCameraController : MonoBehaviour {
    public Camera Left;
    public Camera Right;
    public Camera Front;
    public Camera Back;

    /// <summary>
    /// The radius of the viewport for all camera combined. 
    /// Should roughly equal the width of the pyramide.
    /// </summary>
    public float Radius;

    /// <summary>
    /// The Height of the pyramide.
    /// </summary>
    public float Height;

	// Use this for initialization
	void Start () {
        // Move to the outer ring depending on the radius.
        Left.transform.localPosition = new Vector3(
            Left.transform.localPosition.x,
            Left.transform.localPosition.y,
            Radius
        );
        Right.transform.localPosition = new Vector3(
            Right.transform.localPosition.x,
            Right.transform.localPosition.y,
            -Radius
        );
        Front.transform.localPosition = new Vector3(
            Radius,
            Front.transform.localPosition.y,
            Front.transform.localPosition.z
        );
        Back.transform.localPosition = new Vector3(
            -Radius,
            Back.transform.localPosition.y,
            Back.transform.localPosition.z
        );

        // Calculate rotation.
        Left.transform.localEulerAngles = new Vector3(
            180.0f - Height,
            0.0f,
            45.0f
        );
        Right.transform.localEulerAngles = new Vector3(
            0.0f + Height,
            0.0f,
            45.0f
        );
        Front.transform.localEulerAngles = new Vector3(
            180.0f - Height,
            90.0f,
            -45.0f
        );
        Back.transform.localEulerAngles = new Vector3(
            0.0f + Height,
            90.0f,
            -45.0f
        );
	}
}
