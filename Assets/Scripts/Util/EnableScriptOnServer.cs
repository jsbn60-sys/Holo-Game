using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class EnableScriptOnServer : NetworkBehaviour {

	[SerializeField]
	List<MonoBehaviour> scripts = new List<MonoBehaviour>();
	
	override public void OnStartServer() 
	{
		foreach (var script in scripts)
		{
			script.enabled = true;
		}
	}

	
}
