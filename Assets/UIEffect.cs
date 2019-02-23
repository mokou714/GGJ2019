using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEffect : MonoBehaviour {

    public float fadeInSpeed;
    public float fadeOutSpeed;

    public bool fadeIn;
    public bool fadeOut;
    public bool blink;

    // Use this for initialization
    void Start () {

        //fadeIn = true;

	}
	
	// Update is called once per frame
	void Update () {
        if (fadeOut)
        {
            Debug.Log("!!!!!");
            GetComponent<RawImage>().color = new Color(255f,255f,255f,0f);
            StartCoroutine(Out());
            fadeOut = false;

        }
        else if (fadeIn) {
            GetComponent<RawImage>().color = new Color(255f, 255f, 255f, 1f);
            StartCoroutine(In());
            fadeIn = false;
        }
        else if (blink) {
            StartCoroutine(Blink());
            blink = false;
        }

    }

    IEnumerator Out() {
        float alpha = GetComponent<RawImage>().color.a;
        while (alpha < 1f)
        {
            GetComponent<RawImage>().color = new Color(255f, 255f, 255f, alpha += fadeOutSpeed / 100f);
            yield return new WaitForSeconds(0.1f);
        }
        GetComponent<RawImage>().color = new Color(255f, 255f, 255f, 1f);
    }

    IEnumerator In()
    {
        float alpha = GetComponent<RawImage>().color.a;
        while (alpha > 0f)
        {
            GetComponent<RawImage>().color = new Color(255f, 255f, 255f, alpha -= fadeOutSpeed / 100f);
            yield return new WaitForSeconds(0.1f);
        }
        GetComponent<RawImage>().color = new Color(255f, 255f, 255f, 0f);
    }

    IEnumerator Blink()
    {
        GetComponent<RawImage>().color = new Color(255f, 255f, 255f, 1f);
        yield return new WaitForSeconds(0.05f);
        GetComponent<RawImage>().color = new Color(255f, 255f, 255f, 0f);
    }







}
