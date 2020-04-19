/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Multiplayer;

/// <summary>
/// This class represents the Minimap. It shows the sorrounding of the enemy from a bird perspective.
/// </summary>
public class Map : MonoBehaviour
{
	public ScrollRect map;
	public GameObject player;
	private float x;
	private float z;
    void Start()
    {
		map.vertical = false;
		map.horizontal = false;
		map.verticalNormalizedPosition = 0.65f;
		map.horizontalNormalizedPosition = 0.38f;
	}

    // Update is called once per frame
    void Update()
    {
		if (player == null)
		{
			player = GameObject.FindGameObjectWithTag("Player");
			x = player.transform.position.x;
			z = player.transform.position.z;
		}
		else
		{
			map.verticalNormalizedPosition = (player.transform.position.z - z) * 0.0055f;
			map.horizontalNormalizedPosition = (player.transform.position.x - x) * 0.0043f;
		}
	}
}
