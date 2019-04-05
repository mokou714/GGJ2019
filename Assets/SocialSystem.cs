using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IPHONE
using UnityEngine.SocialPlatforms.GameCenter;
#endif
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class SocialSystem : MonoBehaviour
{
    public static SocialSystem instance = null;
    public GameStates gameStates;
    private string showContent = "";
    public Splash splash;
    public bool debug = true;

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
#if UNITY_IPHONE
        GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
#endif

#if UNITY_ANDROID
        //PlayGamesPlatform.Activate();
#endif
        //if(Application.platform == RuntimePlatform.Android){
        //}
    }

    // Use this for initialization
    void Start()
    {
        gameStates = GameStates.instance;
        //PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        //PlayGamesPlatform.InitializeInstance(config);
        //// recommended for debugging:
        //PlayGamesPlatform.DebugLogEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void SocialAccount()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            GameStates.instance.saveData("device", 0);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            GameStates.instance.saveData("device", 1);
        }
        else
        {
            splash.startTheGame();
        }
        logIn();
    }


    public void setAchievement(string id, float percentage = 100){
        if(GameStates.instance.isLoggedIn){
            Social.ReportProgress(id, 100f, (bool success) =>
            {
                if (success)
                {
                    if (debug)
                        showContent = "Achievement got";
                }
                else
                {
                    if (debug)
                        showContent = "Set Achievement Failed";
                }
            });

        }
    }

    public void setLeaderBoard(string id, long score){
        if (GameStates.instance.isLoggedIn){
            Social.ReportScore(score, id, (bool success) => {
                if(success){
                    if (debug)
                        showContent = "Leaderboard got";
                }else{
                    if (debug)
                        showContent = "Leaderboard failed";
                }
            });
        }
    }

    public void logIn(){
#if UNITY_ANDROID  
        PlayGamesPlatform.Activate();
#endif

        Social.localUser.Authenticate(success =>
        {
            if (success){
#if UNITY_ANDROID 
                ((PlayGamesPlatform)Social.Active).SetGravityForPopups(Gravity.TOP);
#endif
                showContent = "Success";
                GameStates.instance.isLoggedIn = true;
                //setAchievement(Achievements.achievement_passed_tutorial);
                //setLeaderBoard(Achievements.cont_jump_leaderboard, 10);
            }else{
                showContent = "Login failed";
            }
            splash.startTheGame();
        });
    }

//    public void iosLogin()
//    {
//        Social.localUser.Authenticate(success =>
//        {
//            if (success)
//            {
//                Debug.Log("Authentication successful");
//                string userInfo = "Username: " + Social.localUser.userName +
//                    "\nUser ID: " + Social.localUser.id +
//                    "\nIsUnderage: " + Social.localUser.underage;
//                Debug.Log(userInfo);
//                showContent = userInfo;

//                listAchievements();
//            }
//            else
//            {
//                showContent = "Login failed";

//            }
//            splash.startTheGame();
//        });


//    }
//#if UNITY_ANDROID
//    public void androidLogin(){
//        PlayGamesPlatform.Activate();
//        Social.localUser.Authenticate((bool success) => {
//            // handle success or failure
//            if (success){
                //((PlayGamesPlatform)Social.Active).SetGravityForPopups(Gravity.TOP);
//                //Social.ShowAchievementsUI();
//                //Social.ShowLeaderboardUI();
//                GameStates.instance.isLoggedIn = true;
//            }else{
//                showContent = "Login failed";
//            }
//            splash.startTheGame();
//        });
//    }
//#endif

    public void syncUserData(){
        
    }

    public void listAchievements(){
        if(GameStates.instance.isLoggedIn){
            Social.ShowAchievementsUI();
        }else{
            print("Not logged In");
        }
    }

    public void listLeaderboard()
    {
        if (GameStates.instance.isLoggedIn)
        {
            Social.ShowLeaderboardUI();
        }else{
            print("Not logged In");
        }
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
        GUI.Label(new Rect(0, 0, 50, 50), gUI, style);
    }



}
