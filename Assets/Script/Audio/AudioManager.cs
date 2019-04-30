using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;



public class AudioManager : MonoBehaviour
{
    public AudioMixerSnapshot paused;
    public AudioMixerSnapshot unpaused;
    public float pauseTransitionTime = 0.01f;

    public bool isDevMode = true;
    // sometimes we will have more than one sfx clip playing at the same time
    // so we need to have multiple audio sources, each of which primarily handles one sfx

    public AudioSource[] sfxSources;

    public AudioSource[] musicSources;
    int curMusicSourceID = 0;

    public float musicFadeOutTime = 1f;
    public float musicFadeInTime = 1f;

    public static AudioManager instance = null;     //Allows other scripts to call functions from AudioManager.             
    public float lowPitchRange = .9f;              //The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.1f;            //The highest a sound effect will be randomly pitched.
    float randomPitch;

    // this includes backup sources, this is used by ChangeSFXVolume()
    AudioSource[] allSFXSources;

    public static int landingSfxID = 1;

    // Music clip List
    public AudioClip[] musicList;

    // SFX clip List
    public AudioClip[] sfxList;

    // specifies which sfx clip (name) will be played primarily on which sfx source (id in sfxSources list)
    public Dictionary<string, int> sfxSourceMap;

    // specifies which sfx clip (name) will be played primarily on which bgm source (id in bgmSources list)
    public Dictionary<string, int> musicSourceMap;


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

        allSFXSources = sfxSources[0].transform.parent.GetComponentsInChildren<AudioSource>();
        //print("sfxSourcesAndBackupSources: " + sfxAndBackupSources.Length);

        if (PlayerPrefs.HasKey("musicVolume"))
            ChangeBGMVolume(PlayerPrefs.GetFloat("musicVolume"));
        if (PlayerPrefs.HasKey("soundVolume"))
            ChangeSFXVolume(PlayerPrefs.GetFloat("soundVolume"));

        // register sfx clips to sfx sources
        sfxSourceMap = new Dictionary<string, int>();
        foreach (AudioClip clip in sfxList)
        {
            string n = clip.name;

            if (n.Contains("NextLevel"))
                sfxSourceMap.Add(n, 0);
            else if (n.Contains("Harp Land"))
                sfxSourceMap.Add(n, 1);
            else if (n.Contains("Harp Charge"))
                sfxSourceMap.Add(n, 2);
            else if (n == "being hit")
                sfxSourceMap.Add(n, 3);
            else if (n == "Die")
                sfxSourceMap.Add(n, 4);
            else if (n.Contains("BadgeEmerge"))
                sfxSourceMap.Add(n, 5);
            else if (n == "BadgeLand")
                sfxSourceMap.Add(n, 6);
            else if (n == "Unlocking")
                sfxSourceMap.Add(n, 7);
            else if (n == "Star")
                sfxSourceMap.Add(n, 8);
        }
        
        //foreach (string key in sfxSourceMap.Keys)
        //{
        //    float val = sfxSourceMap[key];
        //    print(key + " = " + val);
        //}
        musicSourceMap = new Dictionary<string, int>();
        foreach (AudioClip clip in musicList)
        {
            string n = clip.name;

            if (n == "bgm0a" || n == "bgm0b")
                musicSourceMap.Add(n, 0);
            else if (n == "bgm1")
                musicSourceMap.Add(n, 1);
            else if (n == "bgm2")
                musicSourceMap.Add(n, 2);
        }

        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (buildIndex < 3)
        {
            int unlockedLevel = -2;
            if (PlayerPrefs.HasKey(Constants.curLevelKey))
                unlockedLevel = PlayerPrefs.GetInt(Constants.curLevelKey);                
             
            if (unlockedLevel <= 10)
                PlayMusic("bgm0a");
            else if (unlockedLevel > 10 && unlockedLevel < 21)
                PlayMusic("bgm0b");
        }
        else if (buildIndex < 14 || buildIndex == 26) // end Stage
            PlayMusic("bgm1");
        else 
            PlayMusic("bgm2");
               
    }

    // Use this for initialization
    void Start()
    {


    }

    // get the current playback time of an audio clip
    public float GetMusicPlaybackTime(string n)
    {
        // search in the dictionary for the audio source
        int si = 0;
        if(musicSourceMap.TryGetValue(n, out si))
        {
            AudioSource a = musicSources[si];
            if (a.isPlaying && a.clip.name == n)
                return a.time;
            else
            {
                print("Music clip is not playing");
                return -1;
            }
        }
        print("Music clip not registered to an audio source");
        return -1;
    }


    //Called by Colliions -> OnTriggerEnter2D
    public void PlayLevelFinishSFX(string curLevelName)
    {
        // entering a new big level or from startpage
        if (curLevelName == "start page" || curLevelName == "-1" || curLevelName == "2-start")
            //PlaySFX("NextLevel2");
            PlaySFX("NextLevel_1");
        // entering the end sub-level of the current big level
        //else if (curLevelName == "9" || curLevelName == "17" || curLevelName == "-2")
            //PlaySFX("NextLevel1");

        else
            PlaySFX("NextLevel_0");
        
    }
           
    private void OnLevelWasLoaded(int level)
    {
        string curScene = SceneManager.GetActiveScene().name;
        int curLevel;

        if (int.TryParse(curScene, out curLevel))
        {
            if (curLevel > 0 && curLevel <= 10)
                PlayMusic("bgm1");
            else if (curLevel > 10 && curLevel <= 20)
                PlayMusic("bgm2");
        }

        if (curScene == "start page")
        {
            // find out what is next level (the locked big level)
            int nextLevel = GameStates.instance.getProgress();
            if (nextLevel > 0 && nextLevel <= 10)
                PlayMusic("bgm0a");
            else if (nextLevel > 10 && nextLevel <= 21)
                PlayMusic("bgm0b");
            else
                PlayMusic("bgm0a");
        }
        else if (curScene == "2-start")
        {
            PlayMusic("bgm1");
        }

        if (level == 26)
        {
            PlayMusic("bgm1");
        }
    }

    //Used to play a bgm music clip by its name
    public void PlayMusic(string n, bool isLoop = true)
    {
        //print("PlayMusic " + SceneManager.GetActiveScene().name + "," + n);

        // is playing the same, and music is in loop mode
        if (isLoop && musicSources[curMusicSourceID].isPlaying && musicSources[curMusicSourceID].clip.name == n)
        {
            return;
        }
            

        int si = 0;
        int ci = 0;
        AudioSource src;

        if (musicSourceMap.TryGetValue(n, out si))
        {
            src = musicSources[si];

            // find the clip id from the sfxList by name
            for (int i = 0; i < musicList.Length; i++)
            {
                if (musicList[i].name == n)
                {
                    ci = i;
                    break;
                }
            }

            //Load the clip to the right music source
            src.clip = musicList[ci];

            // To-do cross fade two sources

            float v = PlayerPrefs.HasKey("musicVolume") ? PlayerPrefs.GetFloat("musicVolume") : 1;

            StartCoroutine(FadeIn(src, musicFadeInTime, v));

            if (musicSources[curMusicSourceID].isPlaying && curMusicSourceID != si)
            {
                StartCoroutine(FadeOut(musicSources[curMusicSourceID], musicFadeOutTime));
            }
                

            curMusicSourceID = si;
        }
        
    }


    public void PlaySFX(params string[] ns)
    {
        int si = 0;
        int ci = 0;
        AudioSource src;
        //print("playing " + ns);
        string n = ns[Random.Range(0, ns.Length)];

        // find the primary audioSource registered for playing this clip
        if (sfxSourceMap.TryGetValue(n, out si))
        {
            // Some sfx like "being hit" will be called frequently, 
            // Need two or more audio sources to avoid noise between two plays
            // reference: https://forum.unity.com/threads/problem-with-audio-crackling.482086/

            // if the primary audioSource is busy playing the same clip,
            // find an idle backup source (in its children) to play it
            if (sfxSources[si].isPlaying)
            {
                AudioSource[] backupSources = sfxSources[si].gameObject.GetComponentsInChildren<AudioSource>();
                src = backupSources[0]; // get the first one by default
                for (int i = 0; i < backupSources.Length; i++)
                {
                    if (!backupSources[i].isPlaying)
                    {
                        src = backupSources[i];
                        if(isDevMode)
                            print("backupSource: " + i);
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
                if (sfxList[i].name == n)
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
                if (s == n)
                {
                    RandomizePitch();

                    //Set the pitch of the audio source to the randomly chosen pitch.                     
                    src.pitch = randomPitch;

                    //print("src.pitch: " + src.pitch);

                    break;
                }
            }

            src.Play();

            if (isDevMode)
                print("Playing:" + src.clip.name);
        }
        else
        {
            print("please specify a sfx source to play this sfx clip in AudioManager.Start()");
        }

    }


    //Used to play a sound clip by its name, with delay in secs, if has more inputs, randomly choose one
    public void PlaySFX(float delay = 0, params string[] ns)
    {
        int si = 0;
        int ci = 0;
        AudioSource src;
        //print("playing " + ns);
        string n = ns[Random.Range(0, ns.Length)];

        // find the primary audioSource registered for playing this clip
        if (sfxSourceMap.TryGetValue(n, out si))
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
                if (sfxList[i].name == n)
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
                if (s == n)
                {
                    RandomizePitch();

                    //Set the pitch of the audio source to the randomly chosen pitch.                     
                    src.pitch = randomPitch;

                    print("src.pitch: " + src.pitch);

                    break;
                }
            }
            
            src.PlayDelayed(delay);
   

        }
        else
        {
            print("please specify a sfx source to play this sfx clip in AudioManager.Start()");
        }

    }

    // find if a clip is being played
    public bool IsPlaying(string n)
    {
        int si; // audiosource id
        if (sfxSourceMap.TryGetValue(n, out si))        
            foreach (AudioSource a in sfxSources[si].gameObject.GetComponentsInChildren<AudioSource>())            
                if (a.isPlaying && a.clip.name == n)
                    return true;

        if (musicSourceMap.TryGetValue(n, out si))
            foreach (AudioSource a in musicSources[si].gameObject.GetComponentsInChildren<AudioSource>())
                if (a.isPlaying && a.clip.name == n)
                    return true;

        return false;
    }

    public void PauseTransition()
    {
        if(Time.timeScale == 0)
            paused.TransitionTo(pauseTransitionTime);
        else
            unpaused.TransitionTo(pauseTransitionTime);
    }
    
    public void ChangeMasterVolume(float v)
    {
        ChangeBGMVolume(v);
        ChangeSFXVolume(v);
    }

    public void ChangeBGMVolume(float v)
    {
        foreach (AudioSource a in musicSources)
            a.volume = v;
    }

    public void ChangeSFXVolume(float v)
    {
        foreach (AudioSource a in allSFXSources)
            a.volume = v;
           
    }

    private void Update()
    {
        //print(musicSources[curMusicSourceID].time);
    }

    IEnumerator FadeOut(AudioSource src, float duration)
    {
        while (src.volume > 0.01f)
        {
            src.volume -= Time.unscaledDeltaTime / duration; // volume needs to go down by this much very frame in order to fade out during "duration" sec
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
            src.volume += Time.unscaledDeltaTime / duration; // volume needs to go up by this much very frame in order to fade in during "duration" sec
            yield return null;
        }
        src.volume = maxVolume;
    }

    void RandomizePitch()
    {
        randomPitch = Random.Range(lowPitchRange, highPitchRange);
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



}
