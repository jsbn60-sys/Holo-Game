using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class represents a door that will close itself after a given time.
/// </summary>
public class Door : MonoBehaviour
{

	[SerializeField] private TextMesh countdownText;
	[SerializeField] private Transform doorClosePos;

	protected bool isActivated;


	/// <summary>
	/// Start is called before the first frame update.
	/// </summary>
	private void Start()
	{
		isActivated = false;
		countdownText.text = "";
	}

	/// <summary>
	/// Update is called once per frame.
	/// </summary>
	private void Update()
	{
		if (!isActivated) return;

		countdownText.text = "";
		this.transform.position = Vector3.Lerp(this.transform.position, doorClosePos.transform.position, 0.3f * Time.deltaTime);

		if (Vector3.Distance(this.transform.position,doorClosePos.position) < 0.2f)
		{
			this.enabled = false;
		}
	}
	/// <summary>
	/// Updates the countdownText.
	/// </summary>
	/// <param name="countdown"></param>
	public void updateDoor(string doorText)
	{
		countdownText.text = doorText;
	}

	public void activateDoor()
	{
		isActivated = true;
	}
}
