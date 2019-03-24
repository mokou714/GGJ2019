using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour {

    public Button homeButton;
    public Button audioButton;

    //public Slider audioSlider;
    //public GameObject audioPanel;

    public GameObject musicIcon;
    public GameObject muteIcon;

    bool isAudio = true;

    public static UIManager instance = null;

    private void Awake()
    {
        //Check if there is already an instance 
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance
            Destroy(gameObject);

        //Set AudioManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }


    // Use this for initialization
    public void Start () {

        homeButton.onClick.AddListener(OnHomeButtonClicked);
        audioButton.onClick.AddListener(OnAudioButtonClicked);

        //audioPanel.SetActive(false);

        //audioSlider.maxValue = 100f;
        //audioSlider.minValue = 0f;
        //audioSlider.onValueChanged.AddListener(delegate { OnAudioSliderValueChanged(); });

        isAudio = GameStates.isAudio;

        musicIcon.SetActive(isAudio);
        muteIcon.SetActive(!isAudio);

    }

    private void OnAudioButtonClicked()
    {
        //audioPanel.SetActive(!audioPanel.activeSelf);
        isAudio = !isAudio;
        musicIcon.SetActive(isAudio);
        muteIcon.SetActive(!isAudio);
        GameStates.isAudio = isAudio;
        GameStates.bgmVolume = isAudio? 1.0f : 0f;
        AudioManager.instance.ChangeMasterVolume(GameStates.bgmVolume);
    }

    private void OnHomeButtonClicked()
    {
        SceneManager.LoadScene("start page");
    }

    void OnAudioSliderValueChanged()
    {
        //float vol = audioSlider.value / 100f;

        //GameStates.masterVolume = vol; // for saving system settings

        //AudioManager.instance.ChangeMasterVolume(vol); // change vol

    }

    // Update is called once per frame
    void Update () {
		
	}
}
