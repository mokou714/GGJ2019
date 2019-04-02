// version 2 by Ke, 3/29/2019
// Added menu and redesigned UI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour {

    public Button menuButton;

    public Button backButton;
    public Button closeButton;

    // menuButtonsGroup
    public Button homeButton;
    public Button resumeButton;
    public Button settingsButton;
       
    // UI objects
    public GameObject menu;
    public GameObject menuButtonsGroup;
    public GameObject settings;
         
    // singleton instance
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

        // add UI listeners
        menuButton.onClick.AddListener(OnMenuButtonClicked);

        backButton.onClick.AddListener(OnBackButtonClicked);
        closeButton.onClick.AddListener(OnCloseButtonClicked);

        // menuButtonsGroup listeners
        resumeButton.onClick.AddListener(OnCloseButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        homeButton.onClick.AddListener(OnHomeButtonClicked);      

        // Initialize menu
        for (int i = 0; i < menuButtonsGroup.transform.childCount; i++)
            menuButtonsGroup.transform.GetChild(i).gameObject.SetActive(true);

        menu.SetActive(false);



    }


    private void OnSettingsButtonClicked()
    {
        settings.SetActive(true);
        menuButtonsGroup.SetActive(false);

        // To do: add UI animation

    }

    private void OnCloseButtonClicked()
    {
        // To do: resume from pause

        menu.SetActive(false);
    }

    private void OnBackButtonClicked()
    {
        // To do: add UI animations

        // close settings and show first-level menu
        menuButtonsGroup.SetActive(true);
        settings.SetActive(false);

    }

    private void OnMenuButtonClicked()
    {   
        // To do: pause?
        settings.SetActive(false);
        menuButtonsGroup.SetActive(true);

        for (int i = 0; i < menuButtonsGroup.transform.childCount; i++)  
            menuButtonsGroup.transform.GetChild(i).gameObject.SetActive(true);

        menu.SetActive(!menu.activeSelf);
    }



    private void OnHomeButtonClicked()
    {
        menu.SetActive(false);
        SceneManager.LoadScene("start page");
    }


    // Update is called once per frame
    void Update () {
		
	}
}
