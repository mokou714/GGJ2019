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

    // startPageButtonGroup
    public Button discoveriesButton;
    public Button startSettingsButton;
    public Button aboutButton;

    // UI objects
    public GameObject menu;
    public GameObject menuButtonsGroup;
    public GameObject startMenuButtonsGroup; // startPage


    public GameObject settingsPanel;
    public GameObject discoveriesPanel;
    public GameObject aboutPanel;

    string pageName = "game";
    public string[] discoveredStarNames =
        { "Sagittarius", "Pisces", "Cancer", "Taurus", "Aquarius", "Libra" };

    // singleton instance
    public static UIManager instance = null;

    bool isStartPage = false;
    
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

        //Set Manager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
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

        // start page menu group listeners
        discoveriesButton.onClick.AddListener(OnDiscoveryButtonClicked);
        startSettingsButton.onClick.AddListener(OnSettingsButtonClicked);
        aboutButton.onClick.AddListener(OnAboutButtonClicked);

        // Initialize menu
        isStartPage = SceneManager.GetActiveScene().name == "start page";
        
        for (int i = 0; i < menuButtonsGroup.transform.childCount; i++)
        menuButtonsGroup.transform.GetChild(i).gameObject.SetActive(!isStartPage);
        
        for (int i = 0; i < startMenuButtonsGroup.transform.childCount; i++)
            startMenuButtonsGroup.transform.GetChild(i).gameObject.SetActive(isStartPage);


        settingsPanel.SetActive(false);
        discoveriesPanel.SetActive(false);
        aboutPanel.SetActive(false);
        menu.SetActive(false);
        
    }



    private void OnAboutButtonClicked()
    {
        aboutPanel.SetActive(true);
        startMenuButtonsGroup.SetActive(false);
        pageName = "about";
    }

    private void OnDiscoveryButtonClicked()
    {
        discoveriesPanel.SetActive(true);
        startMenuButtonsGroup.SetActive(false);
        pageName = "discoveries";

    }

    private void OnSettingsButtonClicked()
    {
        settingsPanel.SetActive(true);
        menuButtonsGroup.SetActive(false);
        startMenuButtonsGroup.SetActive(false);
        pageName = "settings";
        // To do: add UI animation

    }

    private void OnCloseButtonClicked()
    {
        // To do: resume from pause

        print("close button clicked");

        menu.SetActive(false);
        pageName = "game";
        menuButton.gameObject.SetActive(true);

    }

    private void OnBackButtonClicked()
    {
        // To do: add UI animations

        // close settings and show first-level menu

        print("close button clicked");


        isStartPage = SceneManager.GetActiveScene().name == "start page";
        if (isStartPage)
            startMenuButtonsGroup.SetActive(true);
        else          
            menuButtonsGroup.SetActive(true);

        switch (pageName)
        {
            case "settings":
                settingsPanel.SetActive(false);
                break;
            case "discoveries":
                discoveriesPanel.SetActive(false);
                break;
            case "about":
                aboutPanel.SetActive(false);
                break;            
        }

        pageName = "menu";

    }

    private void OnMenuButtonClicked()
    {
        // To do: pause?
        pageName = "menu";
        isStartPage = SceneManager.GetActiveScene().name == "start page";

        print("cur page: " + SceneManager.GetActiveScene().name);
        print("isStartPage: " + isStartPage);

        startMenuButtonsGroup.SetActive(isStartPage);
        menuButtonsGroup.SetActive(!isStartPage);
        for (int i = 0; i < startMenuButtonsGroup.transform.childCount; i++)
            startMenuButtonsGroup.transform.GetChild(i).gameObject.SetActive(isStartPage);

        for (int i = 0; i < menuButtonsGroup.transform.childCount; i++)
            menuButtonsGroup.transform.GetChild(i).gameObject.SetActive(!isStartPage);

        settingsPanel.SetActive(false);
        discoveriesPanel.SetActive(false);
        aboutPanel.SetActive(false);
        menu.SetActive(true);
        menuButton.gameObject.SetActive(false);
    }



    private void OnHomeButtonClicked()
    {
        menu.SetActive(false);
        SceneManager.LoadScene("start page");
    }


    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha1))
            PlayerPrefs.SetInt("Sagittarius", 0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            PlayerPrefs.SetInt("Pisces", 0);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            PlayerPrefs.SetInt("Cancer", 0);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            PlayerPrefs.SetInt("Taurus", 0);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            PlayerPrefs.SetInt("Aquarius", 0);
        if (Input.GetKeyDown(KeyCode.Alpha6))
            PlayerPrefs.SetInt("Libra", 0);
        if (Input.GetKeyDown(KeyCode.Alpha0))        
            foreach (string key in discoveredStarNames)
                PlayerPrefs.DeleteKey(key);

    }

    public void menuButtonInvert()
    {
        for (int i = 0; i < menuButton.transform.childCount; i++)
        {
            for (int j = 0; j < menuButton.transform.GetChild(i).childCount; j++)
            {
                if (menuButton.transform.GetChild(i).GetChild(j).GetComponent<Image>().color == Color.black)
                    menuButton.transform.GetChild(i).GetChild(j).GetComponent<Image>().color = Color.white;
                else
                    menuButton.transform.GetChild(i).GetChild(j).GetComponent<Image>().color = Color.black;
            }
        }
    }
}
