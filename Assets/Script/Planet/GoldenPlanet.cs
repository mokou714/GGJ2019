using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoldenPlanet : Planet {
    private int dir = 1;
    private bool changeAlpha = true;

    private GameObject award;
    private SpriteRenderer frame;
    private SpriteRenderer good;
    public bool awardAvailable = true;
    Transform planet2;
    private Color origColor;

    private Vector3 size;
    private Color origFrameColor;
    public bool inverted;
    private GameObject text;
    private Transform intro;

	// Use this for initialization
	void Start () {
        setup();
        frame = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        award = transform.Find("Planet2").GetChild(0).gameObject;
        good = award.gameObject.GetComponent<SpriteRenderer>();


        if( 16 <= SceneManager.GetActiveScene().buildIndex && SceneManager.GetActiveScene().buildIndex <= 17){
            print("Invert color");
            frame.color = Color.black;

        }else{
            frame.color = Color.white;
        }
        origFrameColor = frame.color;

        planetBottom.localScale = planetSprite.localScale * 0.5f;
        origColor = good.color;
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
        //Debug.Log("Catch radius:" + catchRadius);

        if(!checkHasTaken("firstBadge") && SceneManager.GetActiveScene().buildIndex == 6){
            intro = transform.Find("intro");
            if(intro)
                intro.gameObject.SetActive(true);

            text = GameObject.FindGameObjectWithTag("tutorialText");

            float ratio = ((float)Screen.width / (float)Screen.height) / (16f/9f);

            print("ratio " +ratio);

            float word_x = 400f * ratio;
            text.GetComponent<RectTransform>().anchoredPosition = new Vector3(word_x, 270, 0);
            if (text != null)
            {
                text.GetComponent<Text>().text = "You just discovered a badge,\n get it and pass the level!";
            }

            GameStates.instance.saveData("firstBadge", 1);

        }

        int index = 0;
        if(random){
            int num_sprite = Random.Range(0, 1000);
            if (num_sprite < 100)
            {
                index = 3;
            }
            else if (num_sprite < 300)
            {
                index = 4;
            }
            else if (num_sprite < 600)
            {
                index = 2;
            }
        }else{
            index = sprite_index;
        }

        Transform sprites = transform.parent.GetChild(0);
        tag = sprites.GetChild(index).tag;

        if (checkHasTaken(tag)){
            print("badge has been taken");
            gameObject.SetActive(false);
            return;
        }
            

        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites.GetChild(index).gameObject.GetComponent<SpriteRenderer>().sprite;

        if(GetComponent<Light>() != null)
            GetComponent<Light>().range = 0;

        planet2.transform.localScale = new Vector3(0, 0, 0);

        

        StartCoroutine(popOut());
    }

    IEnumerator fadeIn(){
        float currentAlpha = 0f;
        int dir = 1;
        float time = 0.1f;
       
        while (true){
            if (thePlayerOnPlanet != null && currentAlpha >= 0.5f){
                //Debug.Log("Stopped");
            }else{
                currentAlpha = frame.color.a;
                float newAlpha = currentAlpha + 0.05f * dir;

                frame.color = new Color(origFrameColor.r, origFrameColor.b, origFrameColor.g, newAlpha);
                origColor.a = newAlpha;
                good.color = origColor;


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


    public override void playLandingSound()
    {        
        AudioManager.instance.PlaySFX("BadgeLand");
    }

    public override void catchedAction(spacecraft sc)
	{
        base.catchedAction(sc);
        if(awardAvailable){
            //Debug.Log("Award");
            StartCoroutine(award.GetComponent<GainedAnimation>().moveStart(sc.transform.parent.transform, 1));
            awardAvailable = false;
            sc.wonAward = tag;

            if(text != null){
                if(text.GetComponent<Text>() != null){
                    text.GetComponent<Text>().text = "";
                }
            }

            if(intro != null){
                intro.gameObject.SetActive(false);
            }

        }

        if(SceneManager.GetActiveScene().buildIndex == 18){
            if(!sc.camera.transform.GetChild(0).GetComponent<colorInverter>().invertingBack && !inverted){
                if(frame != null)
                    frame.color = Color.black;
                if (good != null)
                    good.color = Color.black;
            }
        }

	}

    private bool checkHasTaken(string key){
        //print("Check key: " + key + ", " + GameStates.instance.hasKey(key));
        return GameStates.instance.hasKey(key);
    }

    IEnumerator popOut(){
        
        //planet2.transform.localScale = Vector3.zero;

        while(planet2.transform.localScale.x < size.x)
        {
            planet2.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);            
            yield return new WaitForSeconds(0.01f);

        }

        //play sfx
        // AudioManager.instance.PlaySFX(0.5f, "BadgeEmerge");
        catchRadius = size.x * 3;
    }
}
