using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class GameStates : MonoBehaviour {

    public static GameStates instance = null;

    public bool isSaving = true;

    // settings
    public static bool isPointer = true;


    public string[] bigLevelNames;
    public string[] levels;

    public string showContent;
    public int deviceId;
    public bool isLoggedIn = false;

    public int globalContinuousJump = 0;
    public int firstLevelJumpDisrupt = 0;
    public int globalContinuousJumpMax = 0;
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

        if(Application.platform == RuntimePlatform.Android){
            deviceId = 0;

        }else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            deviceId = 1;

        }
        //Login();

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


    // Use this for initialization
    void Start()
    {
        if (isSaving)
        {
            //LoadLevel();

            if (PlayerPrefs.HasKey("isPointer"))
            {
                isPointer = PlayerPrefs.GetInt("isPointer") == 1;
            }
            else
            {
               isPointer = true;
            }

            print("isPointer: " + isPointer.ToString());

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
        }
        else{
            // clear keys
            PlayerPrefs.DeleteAll();
        }
           
    }


    // save system settings data and current level
    public void SaveLevel(int curID = -1)
    {
        PlayerPrefs.SetInt("hasSavedLevel", 1);
        int curLevelID;
        if (curID > 0)
            curLevelID = curID;
        else
            curLevelID = SceneManager.GetActiveScene().buildIndex + 1;
        
        int unlockedLevelID = SceneManager.GetActiveScene().buildIndex;

        if (unlockedLevelID < curLevelID)
            unlockedLevelID = curLevelID;

        PlayerPrefs.SetInt(Constants.curLevelKey, unlockedLevelID);

        PlayerPrefs.Save();

    }



    public void SaveTutorialData(int checkMark, string saveName){
        PlayerPrefs.SetInt(saveName, checkMark);
        PlayerPrefs.Save();
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

    public int getProgress(){
        if (PlayerPrefs.HasKey(Constants.curLevelKey))
        {
            int unlockedLevelID = PlayerPrefs.GetInt(Constants.curLevelKey);
            return unlockedLevelID;
        }else{
            return SceneManager.GetActiveScene().buildIndex + 1; 
        }
    }

    public void LoadLevel()
    {

            // go to curLevel
        SceneManager.LoadScene(getProgress());
    }

    public bool hasKey(string key){
        return PlayerPrefs.HasKey(key);
    }

    // save system settings data and current level
    public void saveData(string key, object val){
        if(typeof(object) == typeof(int)){
            PlayerPrefs.SetInt(key, (int)val);
        }else if(typeof(object) == typeof(string)){
            PlayerPrefs.SetString(key, (string)val);
        }else if (typeof(object) == typeof(float))
        {
            PlayerPrefs.SetFloat(key, (float)val);
        }
        PlayerPrefs.Save();
    }

    public object getData(string key, System.Type type){
        object ret = null;
        if (type == typeof(int))
        {
            if (!hasKey(key))
                return 0;
            ret = PlayerPrefs.GetInt(key);
        }
        else if (typeof(object) == typeof(string))
        {
            if (!hasKey(key))
                return "";
            ret = PlayerPrefs.GetString(key);
        }
        else if (typeof(object) == typeof(float))
        {
            if (!hasKey(key))
                return 0;
            ret = PlayerPrefs.GetString(key);
        }
        return ret;
    }

    //IEnumerator blinkOnce(){
    //    yield return new WaitForSeconds(0.1f);
    //} 
}
