using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
///<summary>
///This Script handels the Sounds from the player and syncronizes the Sounds in the multiplayer
///</summary>
[RequireComponent(typeof(AudioSource))]
public class AudioManager : NetworkBehaviour
{
	//this variable saves the AudioClips(Need to be in the right order see player prefab)
	public AudioClip[] clips;
	//this method Plays the sound "id" on the given Vector3 "pos" in the world 
	public void PlaySound(Vector3 pos,int id)
	{
		CmdPlaySound(pos,id);
	}

	[Command]
	public void CmdPlaySound(Vector3 pos,int id)
	{
		//AudioSource.PlayClipAtPoint(clips[0],pos);
		RpcClient_Sound(pos,id);
	}

	[ClientRpc]
	public void RpcClient_Sound(Vector3 pos,int id)
	{
		AudioSource.PlayClipAtPoint(clips[id], pos);
	}

}
