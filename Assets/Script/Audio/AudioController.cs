using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {


    public AudioClip[] audioClips;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        playBGM(0, true);
    }

	// Use this for initialization
	void Start () {


       
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    public void playBGM(int i, bool isLoop)
    {
        audioSource.clip = audioClips[i];
                
        audioSource.loop = isLoop;
        
        audioSource.Play();
    }
}
