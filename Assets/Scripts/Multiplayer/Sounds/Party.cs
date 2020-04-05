using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class Party : NetworkBehaviour
{
    // Start is called before the first frame update
    public GameObject party;
    void Start()
    {
        
    }

    // Update is called once per frame
	[Command]
	public void CmdParty() {
        var partyy = Instantiate(party);
        NetworkServer.Spawn(partyy);
	}
}
