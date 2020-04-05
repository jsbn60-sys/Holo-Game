using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicManager : MonoBehaviour
{

	private AudioSource source;

	private void Awake()
	{
		source = GetComponent<AudioSource>();
	}


	void Update()
    {
        if(source.volume < 0.6)
		{
			source.volume += 0.01f * Time.deltaTime;
		}
    }
}
