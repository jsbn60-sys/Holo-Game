using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This Component will manage the spectator mode.
/// To use it attach it to one GameObject in the Scene.
/// When the Spectation mode is started, all GameObjects with the Spectateable-Component will be queried.
/// Via the SpectateCollegue it is possible spectate every queried Spectateable-GameObject,
/// except the Spectator itself, if he is spectateable too.
///
/// NOTE: ShowSpectationOptions and HideSpectationOptions will be invoked on the respective Methode Calls.
/// Use might want to use them to display the GUI of the specation mode.
///
/// NOTE: It is required, that the Main Camera has the "MainCamera"-Tag assigned.
/// </summary>
public class SpectateOptions : MonoBehaviour {
	
	private GameObject mainCamera;
	// the camera target, befor it was changed to some collegue
	private Transform mainTarget;
	// queryied spectateable objects
	private IEnumerable<Spectateable> spectateables;
	// iterator over spectateables
	private IEnumerator<Spectateable> colleagues;
	private bool isSpectating = false;

	[SerializeField]
	private EventTrigger.TriggerEvent ShowSpectationOptions;
	[SerializeField]
	private EventTrigger.TriggerEvent HideSpectationOptions;

	/// <summary>
	/// Starts the Spectation Mode.
	/// Will invoke ShowSpectationOptions and gather all spectateable Objects.
	/// </summary>
	public void StartSpecating()
	{
		if(!isSpectating)
		{
			isSpectating = true;
			mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			mainTarget = mainCamera.GetComponent<CameraFollow>().target;
			spectateables = FindObjectsOfType<Spectateable>();
			colleagues = spectateables.GetEnumerator();
			
			ShowSpectationOptions.Invoke(null);
		}
	}

	/// <summary>
	/// Stops the spectation mode, which will reset the Camera to their pre Spectation mode target
	/// and invokes HideSpectationOptions.
	/// </summary>
	public void StopSpecating()
	{
		if (isSpectating)
		{
			isSpectating = false;
			HideSpectationOptions.Invoke(null);
			ResetSpectating();
		}
	}

	/// <summary>
	/// Changes the spectation to the next spectateable object
	/// </summary>
	public void SpectateColleague()
	{
		mainCamera.GetComponent<CameraFollow>().target = GetColleague();
	}

	/// <summary>
	/// Resets camera target to target before spectation mode.
	/// </summary>
	public void ResetSpectating()
	{
		//reset camera to localplayer
		mainCamera.GetComponent<CameraFollow>().target = mainTarget;
	}

	/// <summary>
	/// This will return the next spectateable object and start over
	/// from the beginning when all objects where spectated ones,
	/// like a circular buffer.
	/// </summary>
	/// <returns></returns>
	private Transform GetColleague()
	{
		//is treated as a circularbuffer
		if (!colleagues.MoveNext())
		{
			//wrap around to beginning of collection and start over
			colleagues.Reset();
			colleagues.MoveNext();
		}


		if (mainTarget.tag != "Drone")
		{
			//skip own player
			if (colleagues.Current.transform.Equals(mainTarget))
				{
				return GetColleague();
				}
			
		}
		else
		{
			colleagues.MoveNext();
		}

		return colleagues.Current.transform;
	}
}
