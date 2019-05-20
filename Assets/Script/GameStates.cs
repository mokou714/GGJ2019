using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class GameStates : MonoBehaviour
{

    public static GameStates instance = null;

    public bool isSaving = true;

    // settings
    public static bool isPointer = true;
    public bool destroy = false;
    public string[] bigLevelNames;

    public string showContent;
    public int deviceId;
    public bool isLoggedIn = false;

    public int globalContinuousJump = 0;
    public int firstLevelJumpDisrupt = 0;
    public int globalContinuousJumpMax = 0;

    public GameObject milkywayTitle;

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

        if(destroy){
            PlayerPrefs.DeleteAll();
        }
        //Login();
        GameStates.instance.globalContinuousJumpMax = (int)getData(Constants.maxConstJumpKey, typeof(int));

    }

    public void reinit(){
        PlayerPrefs.DeleteAll();
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


    private void OnLevelWasLoaded(int level)
    {
        string n = SceneManager.GetActiveScene().name;
        if (n == "1" || n == "11")
        {
            GameObject title = GameObject.FindGameObjectWithTag("levelTitle");

            if (title != null)
            {
                title.GetComponent<TitleScript>().showTitle();
            }
            
        }
        

    }

    // Use this for initialization
    void Start()
    {
        //fixed FPS 60
        Application.targetFrameRate = 60;

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

        if(SceneManager.GetActiveScene().buildIndex == 5 && getUnlockedLevels() < 2){
            if(milkywayTitle != null){
                milkywayTitle.GetComponent<TitleScript>().showTitle();
            }
        }

        //PlayerPrefs.SetString("checkedList", null);
        //Debug.Log(PlayerPrefs.GetString("checkedList"));
    }

    // Update is called once per frame
    void Update()
    {
        //if(SceneManager.GetActiveScene().buildIndex > 4 && SceneManager.GetActiveScene().buildIndex <= 14){
        //    showContent = getData(Constants.bestMilkywayScoreKey, typeof(float)).ToString();
        //}else if(SceneManager.GetActiveScene().buildIndex > 14){
        //    showContent = getData(Constants.bestWhirlpoolScoreKey, typeof(float)).ToString();
        //}

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
    public void SaveLevel(int curID = -3)
    {
        PlayerPrefs.SetInt("hasSavedLevel", 1);
        int curLevelID;
        int maxlevel = getUnlockedLevels();
        if (curID > -3){
            curLevelID = curID;
            if(curLevelID > maxlevel){
                PlayerPrefs.SetInt(Constants.unlockedLevelKey, curLevelID);
                PlayerPrefs.Save();
            }
        } 
    }



    public void SaveTutorialData(int checkMark, string saveName){
        PlayerPrefs.SetInt(saveName, checkMark);
        PlayerPrefs.Save();
    }

    public int GetTutorialData(string saveName){
        //Debug.Log("Loading " + saveName + " " + PlayerPrefs.HasKey(saveName));
        if (!PlayerPrefs.HasKey(saveName))
            return 0;
        return PlayerPrefs.GetInt(saveName);
    }


    public void CommitSaving(){
        PlayerPrefs.Save();
    }

    public string getProgress(){
        if (PlayerPrefs.HasKey(Constants.curLevelKey))
        {
            string curlevel = PlayerPrefs.GetString(Constants.curLevelKey);
            print("Got key progress " + curlevel);

            return curlevel;
        }else{
            print("No progress");

            return "-2"; 
        }
    }

    public int getUnlockedLevels(){
        if (PlayerPrefs.HasKey(Constants.unlockedLevelKey))
        {
            int unlockedLevelID = PlayerPrefs.GetInt(Constants.unlockedLevelKey);
            return unlockedLevelID;
        }else
        {
            return -1;
        }
    }

    public void LoadLevel()
    {
        // go to curLevel
        string levelToLoad = getProgress();
        //print("level to load:" + levelToLoad);
        SceneManager.LoadScene(levelToLoad);
    }

    public bool hasKey(string key){
        return PlayerPrefs.HasKey(key);
    }

    // save system settings data and current level
    public void saveData(string key, object val){
        if(val is int){
            //print(key + " is set to " + val);
            PlayerPrefs.SetInt(key, (int)val);
        }else if(val is string){
            PlayerPrefs.SetString(key, (string)val);
        }else if (val is float)
        {
            PlayerPrefs.SetFloat(key, (float)val);
        }
        //print("saved " + key + ", " + val);
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
        else if (type == typeof(string))
        {
            if (!hasKey(key))
                return "";
            ret = PlayerPrefs.GetString(key);
        }
        else if (type == typeof(float))
        {
            if (!hasKey(key))
                return 0f;
            ret = PlayerPrefs.GetFloat(key);
        }
        return ret;
    }

    //IEnumerator blinkOnce(){
    //    yield return new WaitForSeconds(0.1f);
    //} 
}
