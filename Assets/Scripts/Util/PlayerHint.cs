using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class represents a text that is display to a player for a given amount of time.
/// It can be used to give information or hints to the players.
/// It is implemented as a singleton.
/// </summary>
public class PlayerHint : MonoBehaviour
{
	private static PlayerHint instance;

	public static PlayerHint Instance => instance;

	[SerializeField] private Text hintText;

	[SerializeField] private float fadeInDuration;

	private float fadeTimer;

	private float textShowTimer;

	private bool isActive;

	private bool isFadingOut;

	/// <summary>
	/// Initialize singleton instance.
	/// </summary>
	private void Awake()
	{
		instance = this;
	}

	/// <summary>
	/// Sets up and displays the text for a given duration.
	/// </summary>
	/// <param name="text">Text to display</param>
	/// <param name="duration">How long to display the text for</param>
	public void ShowText(string text, float duration)
	{
		hintText.text = text;
		textShowTimer = duration;
		fadeTimer = fadeInDuration;
		isFadingOut = false;
		isActive = true;
	}

	/// <summary>
	/// Update is called once per frame.
	/// Handles the fading in and out of the text.
	/// </summary>
	private void Update()
	{
		if (isActive)
		{
			if (fadeTimer > 0f)
			{
				fadeTimer -= Time.deltaTime;

				Color fadedColor = hintText.color;

				if (isFadingOut)
				{
					fadedColor.a =  (fadeTimer / fadeInDuration);
				}
				else
				{
					fadedColor.a = 1 - (fadeTimer / fadeInDuration);
				}

				hintText.color = fadedColor;

				if (isFadingOut && fadeTimer <= 0f)
				{
					isActive = false;
				}
			}
			else if(textShowTimer > 0f)
			{
				textShowTimer -= Time.deltaTime;

				if (textShowTimer <= 0f)
				{
					isFadingOut = true;
					fadeTimer = fadeInDuration;
				}
			}
		}
	}
}
