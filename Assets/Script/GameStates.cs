using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class GameStates : MonoBehaviour {

    public static GameStates instance = null;

    public bool isSaving = true;

    // level data
    public static int curLevelID = 0;
    public static int unlockedLevelID = 0;

    // audio
    public static bool isAudio = true;

    [Range(0, 1f)]
    public static float masterVolume = 1.0f;

    [Range(0, 1f)] 
    public static float bgmVolume = 1.0f;

    [Range(0, 1f)]
    public static float sfxVolume = 1.0f;

    public string[] bigLevelNames;
    public string[] levels;

    public string showContent;

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

        //Set GameStates to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);

        //if(Application.platform == RuntimePlatform.Android){
        //}
        Login();

    }

    //GUI log on screen to debug on phones
	private void OnGUI()
	{
        //Debug.Log("GUI start");
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        GUIContent gUI = new GUIContent();
        style.fontSize = 30;
        gUI.text = showContent;
        GUI.Label(new Rect(0, 0, 50, 50), gUI,style);
	}

	//Connecting to google play game account for android user
	public void Login(){
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) => {
            if(success){
                try{
                    ((PlayGamesPlatform)Social.Active).SetGravityForPopups(Gravity.BOTTOM);
                }catch(System.InvalidCastException e){
                    showContent = e.ToString();
                }
            }else{
                Debug.Log("Login failed");
            }
        });
    }

    //private GUI(string text){
    //    GUI.Label()
    //}

    // Use this for initialization
    void Start()
    {
        if (isSaving)
        {
            LoadLevel();
            LoadSettings();
        }


        levels = new string[] {
            "start page", 
            "1-1", "1-2", "1-3", "1-4",
            "2-1", "2-2", "2-3", "2-4" };

        //PlayerPrefs.SetString("checkedList", null);
        //Debug.Log(PlayerPrefs.GetString("checkedList"));
    }

    // Update is called once per frame
    void Update()
    {

    }

    // save level after quit
    private void OnApplicationQuit()
    {
        if (isSaving)
        {
            SaveLevel();
            SaveSettings();
        }
        else{
            // clear keys
            PlayerPrefs.DeleteAll();
        }
           
    }


    // save system settings data and current level
    public static void SaveLevel()
    {
        PlayerPrefs.SetInt("hasSavedLevel", 1);

        curLevelID = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("curLevelID", curLevelID);

        if (unlockedLevelID < curLevelID)
            unlockedLevelID = curLevelID;

        PlayerPrefs.SetInt("unlockedLevelID", unlockedLevelID);

        PlayerPrefs.Save();
        print("save level: " + curLevelID);

    }

    public void SaveTutorialData(int checkMark, string saveName){
        PlayerPrefs.SetInt(saveName, checkMark);
    }

    public int GetTutorialData(string saveName){
        Debug.Log("Loading " + saveName + " " + PlayerPrefs.HasKey(saveName));
        if (!PlayerPrefs.HasKey(saveName))
            return 0;
        return PlayerPrefs.GetInt(saveName);
    }


    public void CommitSaving(){
        PlayerPrefs.Save();
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("hasSavedSettings", 1);
        PlayerPrefs.SetFloat("masterVolume", masterVolume);

        PlayerPrefs.SetFloat("bgmVolume", bgmVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.Save();

        print("saved settings");

    }

    public void LoadLevel()
    {
        print("LoadLevel");

        // check if it is the first time playing
        if (PlayerPrefs.HasKey("hasSavedLevel"))
        {

            curLevelID = PlayerPrefs.GetInt("curLevelID");

            unlockedLevelID = PlayerPrefs.GetInt("unlockedLevelID");

            print("load level: " + curLevelID);

            //

            // go to curLevel
            SceneManager.LoadScene(curLevelID);
        }


    }

    public static void LoadSettings()
    {
        // check if it is the first time playing
        if (PlayerPrefs.HasKey("hasSavedSettings"))
        {

            masterVolume = PlayerPrefs.GetFloat("masterVolume");
            bgmVolume = PlayerPrefs.GetFloat("bgmVolume");
            sfxVolume = PlayerPrefs.GetFloat("sfxVolume");


            // to do: apply settings 

            // load audio volume player has set last time;
            if(UIManager.instance != null)
                UIManager.instance.audioSlider.value = masterVolume * 100f;
        }
    }

}
