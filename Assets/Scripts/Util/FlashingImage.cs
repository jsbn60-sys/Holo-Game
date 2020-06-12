using System;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class represents an interface for flashing an image.
/// The image will slowly fade away over a certain duration.
/// </summary>
public class FlashingImage : MonoBehaviour
{
	[SerializeField] private Image border;

	private float flashTimer;
	private float originalAlpha;
	private float fadeValue;

	/// <summary>
	/// Start is called before the first frame update.
	/// </summary>
	private void Start()
	{
		originalAlpha = border.color.a;
		flashTimer = 0f;
	}

	/// <summary>
	/// Update is called once per frame.
	/// </summary>
    void Update()
    {
	    if (flashTimer > 0f)
	    {
		    flashTimer -= Time.deltaTime;
		    border.color = border.color.WithAlpha(border.color.a - fadeValue);
		    if (flashTimer <= 0f)
		    {
			    border.enabled = false;
		    }
	    }
    }

	/// <summary>
	/// Flashes the image.
	/// </summary>
	/// <param name="duration">Duration of the flash</param>
    public void Flash(float duration)
    {
	    border.enabled = true;
	    border.color = border.color.WithAlpha(originalAlpha);
	    flashTimer = duration;
	    fadeValue = border.color.a / duration;
    }
}
