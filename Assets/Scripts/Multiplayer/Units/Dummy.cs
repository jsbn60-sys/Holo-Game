using System.Collections;
using System.Collections.Generic;
using Multiplayer.Lobby;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Simple dummy unit, that will be targeted by npcs in range.
/// </summary>
public class Dummy : Unit
{
	/// <summary>
	/// Destroys itself on death.
	/// </summary>
	protected override void onDeath()
    {
	    Destroy(gameObject);
    }

	/// <summary>
	/// Dummies can't push anyone.
	/// </summary>
	/// <param name="target">Target that collided</param>
	/// <returns>Always false</returns>
	protected override bool canPushTarget(Unit target)
	{
		return false;
	}

	/// <summary>
	/// To-Do
	/// </summary>
	protected override void hitEffects()
	{

	}

	/// <summary>
	/// Dummies can't attack.
	/// </summary>
	protected override void execAttack()
	{

	}

	/// <summary>
	/// Dummies can only be hit on the server.
	/// </summary>
	/// <returns>Is the localPlayer the server</returns>
	protected override bool canBeHit()
	{
		return LobbyManager.Instance.LocalPlayerObject.GetComponent<NetworkBehaviour>().isServer;
	}
}
