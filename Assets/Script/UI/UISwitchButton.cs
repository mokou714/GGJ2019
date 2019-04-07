using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UISwitchButton : MonoBehaviour {

    public Button button;
    
    public RectTransform blackImageRT;
    public RectTransform whiteImageRT;

    public float xl = 60f;
    public float xr = 179f;
    public float y = -60f;

    public float minPressTimeInterval = 0.5f;    

    float lastTime = 0;
    float switchDuration = 0.4f;
    

    // Use this for initialization
    void Start () {
        
        button.onClick.AddListener(OnClicked);

        lastTime = Time.time;               
        
    }

    private void OnEnable()
    {
        // show settings
        switch (name)
        {
            case "PointerButton":

                InitPos(GameStates.isPointer);

                break;
        }
    }


    public void InitPos(bool on)
    {
        if(on)
        {
            whiteImageRT.anchoredPosition = new Vector2(xl, y);
            blackImageRT.anchoredPosition = new Vector2(xr, y);
        }
        else
        {
            whiteImageRT.anchoredPosition = new Vector2(xr, y);
            blackImageRT.anchoredPosition = new Vector2(xl, y);
        }
    }


    private void OnClicked()
    {
        // detect input intervals to prevent multiple fast inputs
        if (Time.unscaledTime - lastTime < minPressTimeInterval)
            return;
        else
            lastTime = Time.unscaledTime;


        switch (gameObject.name)
        {
            case "PointerButton":

                StartCoroutine(  Switch(!GameStates.isPointer, switchDuration) );

                // change states
                GameStates.isPointer = !GameStates.isPointer;

                // save settings
                PlayerPrefs.SetInt("isPointer", GameStates.isPointer ? 1 : 0);
                spacecraft.instance.showArrow = GameStates.isPointer;
                PlayerPrefs.Save();

                break;
        }
        
    }

    // switch animation
    IEnumerator Switch(bool on, float duration)
    {
        if (on)
        {

            whiteImageRT.anchoredPosition = new Vector2(xr, y);
            blackImageRT.anchoredPosition = new Vector2(xl, y);

            while (whiteImageRT.anchoredPosition.x > xl)
            {
                // volume needs to go up by this much very frame in order to fade in during "duration" sec
                whiteImageRT.anchoredPosition += Time.unscaledDeltaTime / duration * (xr - xl) * Vector2.left;
                blackImageRT.anchoredPosition += Time.unscaledDeltaTime / duration * (xr - xl) * Vector2.right;
                yield return null;
            }
            whiteImageRT.anchoredPosition = new Vector2(xl, y);
            blackImageRT.anchoredPosition = new Vector2(xr, y);

        }
        else
        {
            whiteImageRT.anchoredPosition = new Vector2(xl, y);
            blackImageRT.anchoredPosition = new Vector2(xr, y);

            while (whiteImageRT.anchoredPosition.x < xr)
            {
                // volume needs to go up by this much very frame in order to fade in during "duration" sec
                whiteImageRT.anchoredPosition += Time.unscaledDeltaTime / duration * (xr - xl) * Vector2.right;
                blackImageRT.anchoredPosition += Time.unscaledDeltaTime / duration * (xr - xl) * Vector2.left;
                yield return null;
            }

            whiteImageRT.anchoredPosition = new Vector2(xr, y);
            blackImageRT.anchoredPosition = new Vector2(xl, y);
        }
    }
}
