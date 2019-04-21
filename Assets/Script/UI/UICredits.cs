using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UICredits : MonoBehaviour
{
    public Button skipButton;
    public Text workText;
    public Text nameText;

    float showTime = 3f;
    float fadeInTime = 2f;
    float fadeOutTime = 2f;
    float intervalTime = 2f;

    string[] works = { "Presented by", "Art & Visual Effects", "Music & Sound", "Gameplay Programming", "Level Design", "UI & Audio Programming" };
    string[] names = { "StarGazers Studio", "Jeffrey Lee Ye", "Yihui Liu", "Silver Zhang\nZhehao Xie", "Xiyu Wang", "Ke Jing" };

    bool isShowingCredits = false;

    Color transparrent = new Color(1, 1, 1, 0.2f);

    // Use this for initialization
    void Start()
    {
        skipButton.onClick.AddListener(OnSkipButtonClicked);
        skipButton.GetComponentInChildren<Text>().color = transparrent;

        StartCoroutine(ShowCredits());
    }

    private void Update()
    {
        // twinkle the skip button
        skipButton.GetComponentInChildren<Text>().color = Color.Lerp(transparrent, new Color(1, 1, 1, 0.5f), Mathf.PingPong(Time.time, fadeInTime) / (fadeInTime ));

    }

    private void OnSkipButtonClicked()
    {
        StopCoroutine(ShowCredits());
        SceneManager.LoadScene("start page");
    }

    private void OnLevelWasLoaded(int level)
    {
        if(level != 26)
        {        
            nameText.gameObject.SetActive(false);
            workText.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(false);

            if (isShowingCredits)
                StopCoroutine(ShowCredits());
        }        
    }

    IEnumerator ShowCredits()
    {
        isShowingCredits = true;

        skipButton.gameObject.SetActive(true);
        workText.gameObject.SetActive(true);
        nameText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < works.Length; i++)
        {
            // load next text
            workText.text = works[i];
            nameText.text = names[i];
            
            //print("fade in");
            yield return StartCoroutine(UIManager.instance.FadeInTexts(fadeInTime, workText, nameText));

            //print("show");
            yield return new WaitForSeconds(i == 0 ? showTime * 1.5f : showTime);

            //print("fade out");
            yield return StartCoroutine(UIManager.instance.FadeOutTexts(fadeOutTime, workText, nameText));

            //print("wait");
            yield return new WaitForSeconds(intervalTime);
        }

        nameText.gameObject.SetActive(false);
        workText.gameObject.SetActive(false);
        
        isShowingCredits = false;

        // load start page
        SceneManager.LoadScene("start page");

    }

}
