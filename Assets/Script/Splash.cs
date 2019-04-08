using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour {

    string title;
    string text = "StarGazers Studio";
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
        SocialSystem.instance.splash = this;
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
            //Ask for social account log in
            SocialSystem.instance.SocialAccount();
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

    public void startTheGame(){
        StartCoroutine(startGame());
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
