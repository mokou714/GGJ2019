using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // sometimes we will have more than one sfx clip playing at the same time
    // so we need to have multiple audio sources, each of which primarily handles one sfx

    public AudioSource[] sfxSources;
    public GameObject sfxSourcesObj;

    public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
    public static AudioManager instance = null;     //Allows other scripts to call functions from AudioManager.             
    public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.
    float randomPitch;


    AudioSource[] sfxAndBackupSources;

    public static int sfxNormalLandID = 1;

    // Music clip List
    public AudioClip[] musicList;

    // SFX clip List
    public AudioClip[] sfxList;

    // specifies which sfx clip (name) will be played primarily on which sfx source (id in sfxSources list)
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

        sfxAndBackupSources = sfxSourcesObj.GetComponentsInChildren<AudioSource>();
        //print("sfxSourcesAndBackupSources: " + sfxAndBackupSources.Length);

        if (PlayerPrefs.HasKey("musicVolume"))
            ChangeBGMVolume(PlayerPrefs.GetFloat("musicVolume"));
        if (PlayerPrefs.HasKey("soundVolume"))
            ChangeSFXVolume(PlayerPrefs.GetFloat("soundVolume"));

        // play the bgm
        PlayMusic("soundtrack 2in1");

    }

    // Use this for initialization
    void Start()
    {

        // register sfx clips to sfx sources
        sfxSourceMap = new Dictionary<string, int>();
        foreach (AudioClip clip in sfxList)
        {
            string name = clip.name;

            if (name == "Next Level")
                sfxSourceMap.Add(name, 0);
            else if (name.Contains("Harp Land"))
                sfxSourceMap.Add(name, 1);
            else if (name.Contains("Harp Charge"))
                sfxSourceMap.Add(name, 2);
            else if (name == "being hit")
                sfxSourceMap.Add(name, 3);
            else if(name == "badge")
                sfxSourceMap.Add(name, 2);

        }

        //foreach (string key in sfxSourceMap.Keys)
        //{
        //    float val = sfxSourceMap[key];
        //    print(key + " = " + val);
        //}

        
    }

    // Update is called once per frame
    void Update()
    {


    }

    //Used to play a bgm music clip by its name
    public void PlayMusic(string name, bool isLoop = true)
    {       
        // find the clip from the musicList by name
        foreach (AudioClip clip in musicList)
            if (clip.name == name)
                musicSource.clip = clip;
        
        // set loop option
        musicSource.loop = isLoop;

        // play the clip.
        musicSource.Play();
    }


    //Used to play a sound clip
    public void PlaySFX(string name)
    {
        int si = 0;
        int ci = 0;
        AudioSource src;

        // find the primary audioSource registered for playing this clip
        if (sfxSourceMap.TryGetValue(name, out si))
        {
            // Some sfx like "being hit" will be called frequently, 
            // Need two or more audio sources to avoid noise in between two "being hit" plays
            // reference: https://forum.unity.com/threads/problem-with-audio-crackling.482086/

            // if the primary audioSource is busy playing the same clip,
            // find an idle backup source (in its children) to play it
            if (sfxSources[si].isPlaying)
            {
                AudioSource[] backupSources = sfxSources[si].gameObject.GetComponentsInChildren<AudioSource>();
                src = backupSources[backupSources.Length - 1]; // get the last one by default
                for (int i = 0; i < backupSources.Length; i++)
                {
                    if (!backupSources[i].isPlaying)
                    {
                        src = backupSources[i];
                        //print("backupSource: " + i);
                        break;
                    }
                    
                }

            }
            else
            {
                // play the clip with the primary audioSource
                src = sfxSources[si];
            }

            // find the clip id from the sfxList by name
            for (int i = 0; i < sfxList.Length; i++)
            {
                if (sfxList[i].name == name)
                {
                    ci = i;
                    break;
                }
            }


            //Load the clip
            src.clip = sfxList[ci];

            // varies its pitch if pitchVariation is applied to this clip in the inspector
            foreach (string s in pitchVariationSFX)
            {
                if (s == name)
                {
                    RandomizePitch();

                    //Set the pitch of the audio source to the randomly chosen pitch.                     
                    src.pitch = randomPitch;

                    //print("src.pitch: " + src.pitch);

                    break;
                }
            }
            
            src.Play();
            
        }
        else
        {
            print("please specify which sfx source to play this sfx clip in AudioManager.sfxSourceMap");
        }

    }

    public void ChangeMasterVolume(float v)
    {
        ChangeBGMVolume(v);
        ChangeSFXVolume(v);
    }

    public void ChangeBGMVolume(float v)
    {
        musicSource.volume = v;
    }
    public void ChangeSFXVolume(float v)
    {
        foreach (AudioSource a in sfxAndBackupSources)
            a.volume = v;
           
    }
    //Used to play a sound clip with the options to fade out and fade in
    //public void PlaySFX(string name, float fadeOutDuration = 0.3f, float fadeInDuration = 0.2f)
    //{
    //    int si = 0;
    //    int ci = 0;
    //    AudioSource src;
    //    Debug.Log("Sound being played");

    //    // find the primary audioSource registered for playing this clip
    //    if (sfxSourceMap.TryGetValue(name, out si))
    //    {
    //        // if the primary audioSource is busy playing the same clip,
    //        // find an idle backup source to play it
    //        if (sfxSources[si].isPlaying)
    //        {
    //            // fade out it
    //            StartCoroutine(FadeOut(sfxSources[si], fadeOutDuration));

    //            src = sfxBackupSources[sfxBackupSources.Count-1]; // get the last one by default
    //            for (int i=0; i<sfxBackupSources.Count; i++)
    //            {
    //                if(!sfxBackupSources[i].isPlaying)
    //                {

    //                    src = sfxBackupSources[i];
    //                    //print("backupSource: " + i);
    //                    break;
    //                }
    //                else
    //                {
    //                    // fade out the one currently playing 
    //                    StartCoroutine(FadeOut(sfxBackupSources[i], fadeOutDuration));
    //                }
    //            }

    //        }
    //        else
    //        {
    //            // play the clip with the primary audioSource
    //            src = sfxSources[si];

    //            // if a backup is playing the same clip at the same time, fade it out
    //            for (int i = 0; i < sfxBackupSources.Count; i++)
    //            {
    //                if (sfxBackupSources[i].isPlaying && sfxBackupSources[i].clip.name == name)
    //                {
    //                    StartCoroutine(FadeOut(sfxBackupSources[i], fadeOutDuration));

    //                    //print("fade out backupSource: " + i);
    //                    break;
    //                }

    //            }
    //        }

    //        // find the clip id from the sfxList by name
    //        for (int i = 0; i < sfxList.Count; i++)
    //        {
    //            if (sfxList[i].name == name)
    //            {
    //                ci = i;
    //                break;
    //            }
    //        }


    //        // before switching clips, if the audioSource is playing something, fade it out

    //        if (fadeOutDuration != 0)
    //        {
    //            if (src.isPlaying)
    //            {
    //                StartCoroutine(FadeOut(src, fadeOutDuration));
    //            }
    //        }

    //        //Load the clip
    //        src.clip = sfxList[ci];

    //        // varies its pitch if pitchVariation is applied to this clip in the inspector
    //        foreach (string s in pitchVariationSFX)
    //        {
    //            if (s == name)
    //            {
    //                RandomizePitch();

    //                //Set the pitch of the audio source to the randomly chosen pitch.                     
    //                src.pitch = randomPitch;

    //                //print("src.pitch: " + src.pitch);
    //                break;
    //            }
    //        }

    //        //Play or Fade in.
    //        if (fadeInDuration != 0)
    //            StartCoroutine(FadeIn(src, fadeInDuration, 1f));
    //        else
    //            src.Play();


    //    }
    //    else
    //    {
    //        print("please specify which sfx source to play this sfx clip in AudioManager.sfxSourceMap");
    //    }

    //}



    // Used by PlayMusic() and PlaySFX() to cross fade betweem audio clips
    // reference: https://www.youtube.com/watch?v=OrJXjnNcyE0
    IEnumerator FadeOut(AudioSource src, float duration)
    {
        while (src.volume > 0.01f)
        {
            src.volume -= Time.deltaTime / duration; // volume needs to go down by this much very frame in order to fade out during "duration" sec
            yield return null;
        }
        src.volume = 0;
        src.Stop();
        src.volume = 1;

    }
    IEnumerator FadeIn(AudioSource src, float duration, float maxVolume)
    {
        src.volume = 0;
        src.Play();
        while (src.volume < maxVolume)
        {
            src.volume += Time.deltaTime / duration; // volume needs to go up by this much very frame in order to fade in during "duration" sec
            yield return null;
        }
        src.volume = maxVolume;
    }

    void RandomizePitch()
    {
        randomPitch = Random.Range(lowPitchRange, highPitchRange);
    }

    private void OnApplicationQuit()
    {
        StartCoroutine(FadeOut(musicSource, 0.5f));
    }

}
