using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class Splash : MonoBehaviour {

    string title;
    string text = "Star Gazers Studio";
    int i = 0;
    public Font font;
    GUIContent gUI;
    GUIStyle style;
    float time = 0.2f;
    float interval = 0.2f;

    private AudioSource source;
    public string showContent;

    public Text textfield;
	// Use this for initialization
	void Start () {
        title = "";
        source = GetComponent<AudioSource>();
        //source.clip = audioClip;
        StartCoroutine(loadText());
        //source.Play();

        StartCoroutine(playSound());


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator playSound()
    {
        source.Play();
        yield return new WaitForSeconds(0.3f);
        if (i < text.Length)
            StartCoroutine(playSound());
        else{
            SocialAccount();
        }
            
    }

    IEnumerator loadText(){
        title += text[i];
        if (text[i] == ' ')
            time = 0;
        else if(time < interval)
            time = interval;
        i++;
        textfield.text = title;
        yield return new WaitForSeconds(time);
        if (i < text.Length)
            StartCoroutine(loadText());
    }


    IEnumerator startGame()
    {
        yield return new WaitForSeconds(0.5f);
        int next = SceneManager.GetActiveScene().buildIndex;
        if (GameStates.instance.GetTutorialData("pre") == 1)
            next = SceneManager.GetActiveScene().buildIndex + 2;
        else
            next = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(next);
        Destroy(textfield);
    }


    private void SocialAccount(){
        if (Application.platform == RuntimePlatform.Android)
        {
            #if UNITY_ANDROID
            AndroidLogin();
            GameStates.instance.writeDevice(0);
            #endif
        }else if(Application.platform == RuntimePlatform.IPhonePlayer){
            
            iosLogin();
        }else{
            StartCoroutine(startGame());
        }
    }


#if UNITY_ANDROID
    //Connecting to google play game account for android user
    public void AndroidLogin()
    {
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) => {
            if (success)
            {
                try
                {
                    ((PlayGamesPlatform)Social.Active).SetGravityForPopups(Gravity.BOTTOM);
                }
                catch (System.InvalidCastException e)
                {
                    
                }
            }
            else
            {
                Debug.Log("Login failed");
            }
            StartCoroutine(startGame());
        });
    }

#endif

    public void iosLogin(){
        Social.localUser.Authenticate(success => {
            if (success)
            {
                Debug.Log("Authentication successful");
                string userInfo = "Username: " + Social.localUser.userName +
                    "\nUser ID: " + Social.localUser.id +
                    "\nIsUnderage: " + Social.localUser.underage;
                Debug.Log(userInfo);
                showContent = userInfo;
            }
            else{
                showContent = "Login failed";

            }
            StartCoroutine(startGame());  
                
        });
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
