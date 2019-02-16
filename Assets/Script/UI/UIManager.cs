using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour {

    public Button homeButton;
    public Button audioButton;
    public Slider audioSlider;
    public GameObject audioPanel;

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
    void Start () {

        homeButton.onClick.AddListener(OnHomeButtonClicked);
        audioButton.onClick.AddListener(OnAudioButtonClicked);

        audioPanel.SetActive(false);

        audioSlider.maxValue = 100f;
        audioSlider.minValue = 0f;
        audioSlider.onValueChanged.AddListener(delegate { OnAudioSliderValueChanged(); });


    }

    private void OnAudioButtonClicked()
    {
        audioPanel.SetActive(!audioPanel.activeSelf);
    }

    private void OnHomeButtonClicked()
    {
        SceneManager.LoadScene("start page");
    }

    void OnAudioSliderValueChanged()
    {
        float vol = audioSlider.value / 100f;

        GameStates.masterVolume = vol; // for saving system settings

        AudioManager.instance.ChangeMasterVolume(vol); // change vol

    }

    // Update is called once per frame
    void Update () {
		
	}
}
