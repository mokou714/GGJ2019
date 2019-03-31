using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.UI;


public class UISlider : MonoBehaviour {


    public Sprite whiteHandle;
    public Sprite backHandle;

    public Slider slider;

    public Image handleImage;


    float v;

    // Use this for initialization
    void Start () {
        
        if (slider == null)
            slider = GetComponent<Slider>();


        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });


        slider.value = slider.maxValue;

        switch (name)
        {
            case "MusicSlider":

                // load settings and initialize settings UI
                if (PlayerPrefs.HasKey("musicVolume"))      
                    
                    slider.value = PlayerPrefs.GetFloat("musicVolume");                

                break;

            case "SoundSlider":

                if (PlayerPrefs.HasKey("soundVolume"))

                    slider.value = PlayerPrefs.GetFloat("soundVolume");

                break;

        }

        if (slider.value == slider.minValue)
            handleImage.GetComponent<Image>().sprite = backHandle;
        else
            handleImage.GetComponent<Image>().sprite = whiteHandle;
    }

    private void ValueChangeCheck()
    {
        // animation
        handleImage.GetComponent<Image>().sprite = whiteHandle;
        
        if(slider.value == slider.minValue)
            handleImage.GetComponent<Image>().sprite = backHandle;

        // settings
        v = slider.value;

        switch (name)
        {
            case "MusicSlider":

                AudioManager.instance.ChangeBGMVolume(v);
                PlayerPrefs.SetFloat("musicVolume", v);

                break;

            case "SoundSlider":

                AudioManager.instance.ChangeSFXVolume(v);
                PlayerPrefs.SetFloat("soundVolume", v);

                break;

        }

        PlayerPrefs.Save();
    }

}
