using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjectEffect : SingleUseEffect
{
	[SerializeField] private GameObject objectToPlace;
	[SerializeField] private bool placeInfrontOfPlayer;
	protected override void execEffect()
    {
	    if (placeInfrontOfPlayer)
	    {
		    target.GetComponent<Player>().placeObjectInfront(objectToPlace);
	    }
	    else
	    {
		    target.placeObjectOnTop(objectToPlace);
	    }
    }
}
