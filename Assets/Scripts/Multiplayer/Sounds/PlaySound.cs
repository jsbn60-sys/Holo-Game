/* edited by: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///this script simply plays a AudioClip at the gameobjects transform.position
///</summary>
[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip clip;
    void Start()
    {
        AudioSource.PlayClipAtPoint(clip,transform.position);
    }

}
