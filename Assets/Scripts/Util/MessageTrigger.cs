using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a trigger that activates a text that is displayed to the player.
/// It can be used for giving the player hints in a new area.
/// </summary>
public class MessageTrigger : MonoBehaviour
{
	private bool isTriggered;

	[TextArea(3, 10)] [SerializeField] private string messageToShow;

	public bool IsTriggered => isTriggered;

	/// <summary>
	/// Start is called before the first frame update
	/// </summary>
    void Start()
    {
	    isTriggered = false;
    }

	/// <summary>
	/// Enables trigger and shows text.
	/// </summary>
	/// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
	    if (!isTriggered && other.tag.Equals("Player"))
	    {
		    isTriggered = true;
		    PlayerHint.Instance.ShowText(messageToShow,5f);
	    }
    }
}
