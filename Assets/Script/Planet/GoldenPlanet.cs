﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenPlanet : Planet {
    private int dir = 1;
    private bool changeAlpha = true;

    private GameObject award;
    private SpriteRenderer frame;
    private SpriteRenderer good;
    public bool awardAvailable = true;
    Transform planet2;

    private Vector3 size;
	// Use this for initialization
	void Start () {
        setup();

        frame = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        award = transform.Find("Planet2").GetChild(0).gameObject;
        good = award.gameObject.GetComponent<SpriteRenderer>();


        //GetComponent<Light>().range *= 10;
        //origGlowSize = GetComponent<Light>().range;

        StartCoroutine(fadeIn());
	}
	
	// Update is called once per frame
	void Update () {
        if (thePlayerOnPlanet == null)
            checkCatching();
	}

    public void initBadge(bool random = true, int sprite_index = 0){
        planet2 = transform.Find("Planet2");
        size = planet2.transform.localScale;
        Debug.Log("Catch radius:" + catchRadius);
        int index = 0;
        if(random){
            int num_sprite = Random.Range(0, 1000);
            if (num_sprite < 50)
            {
                index = 1;
            }
            else if (num_sprite < 150)
            {
                index = 2;
            }
            else if (num_sprite < 300)
            {
                index = 3;
            }
            else if (num_sprite < 500)
            {
                index = 4;
            }
            else if (num_sprite < 750)
            {
                index = 5;
            }
            else
            {
                tag = "aqua";
            }
        }else{
            index = sprite_index;
        }

        Transform sprites = transform.parent.GetChild(0);
        tag = sprites.GetChild(index).tag;
        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites.GetChild(index).gameObject.GetComponent<SpriteRenderer>().sprite;

        planet2.transform.localScale = new Vector3(0, 0, 0);
        StartCoroutine(popOut());
    }

    IEnumerator fadeIn(){
        float currentAlpha = 0f;
        int dir = 1;
        float time = 0.1f;
       
        while (true){
            if (thePlayerOnPlanet != null && currentAlpha >= 0.5f){
                Debug.Log("Stopped");
            }else{
                currentAlpha = frame.color.a;
                float newAlpha = currentAlpha + 0.05f * dir;

                frame.color = new Color(255f, 255f, 255f, newAlpha);
                good.color = new Color(255f, 255f, 255f, newAlpha);

                if (currentAlpha <= 0f)
                {
                    dir = 1;
                    time = Random.Range(1, 2);
                }
                else if (currentAlpha >= 1f)
                {
                    dir = -1;
                    time = Random.Range(3, 4);
                }
                else
                {
                    time = 0.1f;
                }
            }

            if (currentAlpha < 0.5)
                GetComponent<GoldenPlanet>().enabled = false;
            else
                GetComponent<GoldenPlanet>().enabled = true;

            yield return new WaitForSeconds(time);
        }
    }



	public override void catchedAction(spacecraft sc)
	{
        base.catchedAction(sc);
        AudioManager.instance.PlaySFX("badge");
        if(awardAvailable){
            Debug.Log("Award");
            StartCoroutine(award.GetComponent<GainedAnimation>().moveStart(sc.transform.parent.transform, 1));
            awardAvailable = false;
            sc.wonAward = tag;
        }
	}

    private bool checkHasTaken(string key){
        return GameStates.instance.hasKey(key);
    }

    IEnumerator popOut(){
        planet2.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
        yield return new WaitForSeconds(0.01f);
        if (planet2.transform.localScale.x < size.x)
        {
            StartCoroutine(popOut());
        }else{
            catchRadius = size.x * 3;
        }
    }
}