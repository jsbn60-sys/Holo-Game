using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSyncPosition : MonoBehaviour
{
    public AudioClip clip;
    private AudioSource audiospeaker;
    void Start()
    {   
        audiospeaker = gameObject.GetComponent<AudioSource>();
        InvokeRepeating("PlaySoundAtPosition", 0.0f, 1.1f);
    }

	void PlaySoundAtPosition(){
		AudioSource.PlayClipAtPoint(clip,transform.position);
	}

}
