using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlinkingImage : MonoBehaviour
{

	Image image;

	void Start()
	{
		image = GetComponent<Image>();
	}

	public IEnumerator blinkOnce()
	{
		image.color = new Color(image.color.r, image.color.g, image.color.b, 0.7f);
		yield return new WaitForSeconds(0.2f);
		image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
	}
}
