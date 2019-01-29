using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    
    public AudioSource sfxSource;                   //Drag a reference to the audio source which will play the sound effects.
    public AudioSource sfxEnergyChargeSource;                   //Drag a reference to the audio source which will play the sound effects.

    public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
    public static AudioManager instance = null;     //Allows other scripts to call functions from SoundManager.             
    public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.

    public static int sfxNormalLandID = 1;
    
    // Music List
    public List<AudioClip> musicList;

    // SFX List
    public List<AudioClip> sfxList;

    // Sington pattern
    void Awake()
    {
        //Check if there is already an instance of AudioManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of AudioManager.
            Destroy(gameObject);

        //Set AudioManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);


        PlayMusic("bgmStart");

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		

	}
  

    //Used to play a bgm music clip by its name
    public void PlayMusic(string name, bool isLoop = true, float fadeOutTime = 0f, float fadeInTime = 0f)
    {
        if(fadeOutTime != 0)
        {
            // Fade out current music

        }
        

        // find the clip from the musicList by name
        foreach (AudioClip clip in musicList)        
            if (clip.name == name)            
                musicSource.clip = clip; 

        //print("musicSource.clip: " + musicSource.clip);

        // set loop option
        musicSource.loop = isLoop;

        //Play the clip.
        musicSource.Play();
    }

    //Used to play a sound clip
    public void PlaySFX(string name, bool isRandomPitch = false)
    {
        if(isRandomPitch)
        {
            //Choose a random pitch to play back our clip at between our high and low pitch ranges.
            float randomPitch = Random.Range(lowPitchRange, highPitchRange);

            //Set the pitch of the audio source to the randomly chosen pitch.
            sfxSource.pitch = randomPitch;
        }
        else
        {
            sfxEnergyChargeSource.pitch = 1.0f;
        }

        // find the clip from the musicList by name
        foreach (AudioClip clip in sfxList)        
            if (clip.name == name)           
                sfxSource.clip = clip;
        
        //Play the clip.
        sfxSource.Play();
    }
    //Used to play a sound clip
    public void PlaySFXEnergyCharge(string name, bool isRandomPitch = false)
    {
        if (isRandomPitch)
        {
            //Choose a random pitch to play back our clip at between our high and low pitch ranges.
            float randomPitch = Random.Range(lowPitchRange, highPitchRange);

            //Set the pitch of the audio source to the randomly chosen pitch.
            sfxEnergyChargeSource.pitch = randomPitch;
        }
        else
        {
            sfxEnergyChargeSource.pitch = 1.0f;
        }

        // find the clip from the musicList by name
        foreach (AudioClip clip in sfxList)
            if (clip.name == name)
                sfxEnergyChargeSource.clip = clip;

        //Play the clip.
        sfxEnergyChargeSource.Play();
    }

    //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
    public void RandomizeSfx(params AudioClip[] clips)
    {
        //Generate a random number between 0 and the length of our array of clips passed in.
        int randomIndex = Random.Range(0, clips.Length);

        //Choose a random pitch to play back our clip at between our high and low pitch ranges.
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        //Set the pitch of the audio source to the randomly chosen pitch.
        sfxSource.pitch = randomPitch;

        //Set the clip to the clip at our randomly chosen index.
        sfxSource.clip = clips[randomIndex];

        //Play the clip.
        sfxSource.Play();
    }

}
