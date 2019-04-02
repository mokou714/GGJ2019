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
        //if(Application.platform == RuntimePlatform.Android){
        //}
    }

    // Use this for initialization
    void Start()
    {
        gameStates = GameStates.instance;
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void SocialAccount()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            androidLogin();
            GameStates.instance.saveData("device", 0);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            iosLogin();
            GameStates.instance.saveData("device", 1);
        }
        else
        {
            splash.startTheGame();
        }
    }


    public void setAchievement(string id)
    {
        Social.ReportProgress(id, 100.0f, (bool success) =>
        {
            if (success)
            {
                showContent = "Achievement got";
            }
            else
            {
                showContent = "Failed";
            }
        });
    }

    public void iosLogin()
    {
        Social.localUser.Authenticate(success =>
        {
            if (success)
            {
                Debug.Log("Authentication successful");
                string userInfo = "Username: " + Social.localUser.userName +
                    "\nUser ID: " + Social.localUser.id +
                    "\nIsUnderage: " + Social.localUser.underage;
                Debug.Log(userInfo);
                showContent = userInfo;

                listAchievements();
            }
            else
            {
                showContent = "Login failed";

            }
            splash.startTheGame();
        });


    }
#if UNITY_ANDROID
    public void androidLogin(){
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) => {
            // handle success or failure
            if (success){
                ((PlayGamesPlatform)Social.Active).SetGravityForPopups(Gravity.TOP);
                //Social.ShowAchievementsUI();
                Social.ShowLeaderboardUI();
            }else{
                showContent = "Login failed";
            }
            splash.startTheGame();
        });
    }
#endif
    public void listAchievements(){
        switch(gameStates.deviceId){
            case 0:
                break;
            case 1:
                Social.ShowAchievementsUI();
                break;
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
