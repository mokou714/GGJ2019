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

    public GameObject gameTitle;

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

        string sceneName = SceneManager.GetActiveScene().name;
        menuButton.gameObject.SetActive(sceneName != "splash page");

    }


    // Use this for initialization
    public void Start () {
                
        Time.timeScale = 1;

        // add UI listeners
        menuButton.onClick.AddListener(OnMenuButtonClicked);

        backButton.onClick.AddListener(OnBackButtonClicked);
        closeButton.onClick.AddListener(OnCloseButtonClicked);

        // menuButtonsGroup listeners
        resumeButton.onClick.AddListener(OnCloseButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        homeButton.onClick.AddListener(OnHomeButtonClicked);

        // start page menu group listeners
        discoveriesButton.onClick.AddListener(OnDiscoveriesButtonClicked);
        startSettingsButton.onClick.AddListener(OnSettingsButtonClicked);
        aboutButton.onClick.AddListener(OnAboutButtonClicked);

        // Initialize 
        
        isStartPage = SceneManager.GetActiveScene().name == "start page";

        gameTitle.SetActive(isStartPage);

        for (int i = 0; i < menuButtonsGroup.transform.childCount; i++)
            menuButtonsGroup.transform.GetChild(i).gameObject.SetActive(!isStartPage);
        
        for (int i = 0; i < startMenuButtonsGroup.transform.childCount; i++)
            startMenuButtonsGroup.transform.GetChild(i).gameObject.SetActive(isStartPage);

        
        settingsPanel.SetActive(false);
        discoveriesPanel.SetActive(false);
        aboutPanel.SetActive(false);
        menu.SetActive(false);
        
    }

    public void Pause()
    {
        spacecraft.instance.requiredSleep = true;
        Time.timeScale = Time.timeScale == Constants.minTimeScale ? 1 : Constants.minTimeScale;
        AudioManager.instance.PauseTransition();
    }

    private void OnAboutButtonClicked()
    {
        aboutPanel.SetActive(true);
        backButton.gameObject.SetActive(true);

        startMenuButtonsGroup.SetActive(false);
        pageName = "about";
    }

    private void OnDiscoveriesButtonClicked()
    {
        discoveriesPanel.SetActive(true);
        backButton.gameObject.SetActive(true);

        startMenuButtonsGroup.SetActive(false);
        pageName = "discoveries";

    }

    private void OnSettingsButtonClicked()
    {
        settingsPanel.SetActive(true);
        backButton.gameObject.SetActive(true);

        menuButtonsGroup.SetActive(false);
        startMenuButtonsGroup.SetActive(false);
        pageName = "settings";
        // To do: add UI animation

    }

    IEnumerator RequireSleep(float startTime)
    {
        yield return new WaitForSecondsRealtime(startTime);
        spacecraft.instance.requiredSleep = false;
    }

    private void OnCloseButtonClicked()
    {
        StartCoroutine(RequireSleep(0.5f));
        menu.SetActive(false);
        pageName = "game";
        menuButton.gameObject.SetActive(true);
        gameTitle.SetActive(isStartPage);
        // unpause
        Pause();
    }

    private void OnBackButtonClicked()
    {
        // To do: add UI animations

        // close settings and show first-level menu
        
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

        backButton.gameObject.SetActive(false);
        pageName = "menu";

    }

    private void OnMenuButtonClicked()
    {

        print("clicked menu");

        pageName = "menu";
        gameTitle.SetActive(false);

        isStartPage = SceneManager.GetActiveScene().name == "start page";
        
        startMenuButtonsGroup.SetActive(isStartPage);
        for (int i = 0; i < startMenuButtonsGroup.transform.childCount; i++)
            startMenuButtonsGroup.transform.GetChild(i).gameObject.SetActive(isStartPage);

        menuButtonsGroup.SetActive(!isStartPage);
        for (int i = 0; i < menuButtonsGroup.transform.childCount; i++)
            menuButtonsGroup.transform.GetChild(i).gameObject.SetActive(!isStartPage);

        settingsPanel.SetActive(false);
        discoveriesPanel.SetActive(false);
        aboutPanel.SetActive(false);

        menu.SetActive(true);
        backButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);

        // pause
        Pause();
    }
    

    private void OnHomeButtonClicked()
    {
        menu.SetActive(false);
        Pause();
        SceneManager.LoadScene("start page");
        AudioManager.instance.SwitchToStartMusic();
    }


    //void Hide(params GameObject[] objs)
    //{
    //    foreach (GameObject obj in objs)
    //        obj.SetActive(false);
    //}
    //void Show(params GameObject[] objs)
    //{
    //    foreach (GameObject obj in objs)
    //        obj.SetActive(true);
    //}
    //void Invert(params GameObject[] objs)
    //{
    //    foreach (GameObject obj in objs)
    //        obj.SetActive(obj.activeSelf);
    //}

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

    public IEnumerator FadeOutWhiteImages(float time, params Image[] images)
    {
        float a0 = images[0].color.a * 255;// 0-1
        float a = a0;
        while (a > 0.1f)
        {
            a -= Time.unscaledDeltaTime / time * a0;
            foreach (Image i in images)            
                i.color = new Color32(255, 255, 255, (byte)a);
            yield return null;
        }
    }
    public IEnumerator FadeInWhiteImages(float time, params Image[] images)
    {
        float a = 0;
        while (a <255f)
        {
            a += Time.unscaledDeltaTime / time * 255f;
            foreach (Image i in images)
                i.color = new Color32(255, 255, 255, (byte)a);
            yield return null;
        }

        foreach (Image i in images)
            i.color = new Color32(255, 255, 255, 255);
    }
    public IEnumerator FadeOutTexts(float time, params Text[] texts)
    {
        float a0 = texts[0].color.a * 255;
        float a = a0;
        while (a > 0.1f)
        {
            a -= Time.unscaledDeltaTime / time * a0;
            foreach (Text text in texts)
                text.color = new Color32(255, 255, 255, (byte)a);
            yield return null;
        }
    }
    public IEnumerator FadeInTexts(float time, params Text[] texts)
    {
        float a = 0;
        while (a > 0.1f)
        {
            a += Time.unscaledDeltaTime / time * 255f;
            foreach (Text text in texts)
                text.color = new Color32(255, 255, 255, (byte)a);
            yield return null;
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        string sceneName = SceneManager.GetActiveScene().name;

        isStartPage = sceneName == "start page";
        gameTitle.SetActive(isStartPage);
        menuButton.gameObject.SetActive(sceneName!="splash page");
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
