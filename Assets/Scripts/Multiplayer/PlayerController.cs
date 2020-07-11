using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
//test
/// <summary>
/// This class represents a controller that provides access to all playerObjects.
/// It is implemented as a singleton.
/// </summary>
public class PlayerController : NetworkBehaviour
{
	public static PlayerController Instance;

	private void OnEnable()
	{
		Instance = this;
	}

	/// <summary>
	/// Gets all player objects.
	/// </summary>
	/// <returns>All player objects</returns>
	public GameObject[] getPlayerObjects()
	{
		return GameObject.FindGameObjectsWithTag("Player");
	}

	/// <summary>
	/// Checks if all player objects are dead.
	/// </summary>
	/// <returns>Are all players dead</returns>
	public bool areAllPlayersDead()
	{
		return getAlivePlayerObjects().Length == 0;
	}

	/// <summary>
	/// Gets amount of alive players in game.
	/// </summary>
	/// <returns>Amount of alive players</returns>
	public int getPlayerCount()
	{
		return getAlivePlayerObjects().Length;
	}

	/// <summary>
	/// Returns all player objects that are still alive.
	/// </summary>
	/// <returns>Players that are alive</returns>
	public GameObject[] getAlivePlayerObjects()
	{
		List<GameObject> alivePlayerObjects = new List<GameObject>();

		foreach (GameObject playerObject in getPlayerObjects())
		{
			if (!playerObject.GetComponent<Player>().isDead())
			{
				alivePlayerObjects.Add(playerObject);
			}
		}

		return alivePlayerObjects.ToArray();
	}
}
