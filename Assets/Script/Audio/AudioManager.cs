using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    // sometimes we will have more than one sfx clip playing at the same time
    // so we need a secondary or a third audio source to handle that
    public List<AudioSource> sfxSources;
       
    public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
    public static AudioManager instance = null;     //Allows other scripts to call functions from SoundManager.             
    public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.

    public static int sfxNormalLandID = 1;
    
    // Music clip List
    public List<AudioClip> musicList;

    // SFX clip List
    public List<AudioClip> sfxList;

    // specifies which sfx clip (name) will be played on which sfx source (id in sfxSources list)
    public Dictionary<string, int> sfxSourceMap;

    public string[] pitchVariationSFX;


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


        PlayMusic("soundtrack 2in1");

    }

    // Use this for initialization
    void Start () {

        // register sfx clips to sfx sources
        sfxSourceMap = new Dictionary<string, int>();
        foreach (AudioClip clip in sfxList)
        {
            string name = clip.name;

            if (name == "Next Level")
                sfxSourceMap.Add(name, 0);
            else if (name.Contains("Harp Land"))
                sfxSourceMap.Add(name, 1);
            else if(name.Contains("Harp Charge"))
                sfxSourceMap.Add(name, 2);
            else if (name == "being hit")
                sfxSourceMap.Add(name, 3);

        }

        foreach (string key in sfxSourceMap.Keys)
        {
            float val = sfxSourceMap[key];
            print(key + " = " + val);
        }

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


        // set loop option
        musicSource.loop = isLoop;

        //Play the clip.
        musicSource.Play();
    }

    //Used to play a sound clip
    public void PlaySFX(string name)
    {

        int si = 0;
        if(sfxSourceMap.TryGetValue(name, out si))
        {
            // find the clip from the musicList by name
            foreach (AudioClip clip in sfxList)
                if (clip.name == name)
                {
                    sfxSources[si].clip = clip;
                    break;
                }                    


            // varies its pitch if applied in the inspector
            foreach (string s in pitchVariationSFX)
            {
                if(s == name)
                {
                    //Choose a random pitch to play back our clip at between our high and low pitch ranges.
                    float randomPitch = Random.Range(lowPitchRange, highPitchRange);

                    //Set the pitch of the audio source to the randomly chosen pitch.                     
                    sfxSources[si].pitch = randomPitch;

                    print("sfxSources[si].pitch: " + sfxSources[si].pitch);

                    break;
                }
            }
                        

            //Play the clip.
            sfxSources[si].Play();
        }
        else
        {
            print("please specify which sfx source to play this sfx clip in AudioManager.sfxSourceMap");
        }
        
    }
        

}
