using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
            StartCoroutine(startGame());
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
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Destroy(textfield);
    }
}
