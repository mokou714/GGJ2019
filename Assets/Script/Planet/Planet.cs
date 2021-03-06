using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Planet : MonoBehaviour
{
    /*
    Parent class for all kinds of planets
    */
    public GameObject thePlayerOnPlanet;
    public bool canPlaySound = true;
    protected GameObject playerObj;
    public float catchRadius;
    public float slowRespTime;

    public Light lightController;

    private float glowIncre;

    public float origGlowSize;
    public Transform planetSprite;
    public Transform planetBottom;



    // Use this for initialization
    public void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        //If the planet already has the player, do not checkCatching until it leaves
        if (thePlayerOnPlanet == null)
            checkCatching();
    }

    public void setup(){
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        planetSprite = transform.Find("Planet2");

        planetBottom = transform.Find("Bottom");

        lightController = GetComponent<Light>();
        if(planetSprite)
            catchRadius = planetSprite.localScale.x;

        //planetBottom.localScale = transform.Find("Planet2").localScale * 0.5f;
        if (planetBottom != null)
        {
            if(planetSprite != null)
                planetBottom.localScale = planetSprite.localScale * (float)(0.3f);
        }

        if (lightController != null)
        {
            lightController.range = planetBottom.localScale.x * 2;
            origGlowSize = lightController.range;
        }



        //Make sure the planet glow's increment is consistent with the player's 
        glowIncre = 0.06f * ((catchRadius * 2.5f - origGlowSize) / 1.5f);
    }

    public virtual void catchedAction(spacecraft sc) { }

    public virtual void playerLeave(){
        canPlaySound = true;
        thePlayerOnPlanet = null;
        //Debug.Log("Player leave: " + thePlayerOnPlanet);

    }

    public virtual void playLandingSound() {

        int buildIndex = SceneManager.GetActiveScene().buildIndex;

        if (buildIndex > 0 && buildIndex <= 13)
        {
            AudioManager.instance.PlaySFX("Harp Land_" + AudioManager.landingSfxID.ToString());

            print("play landing sound");
        }
        else if (buildIndex > 13 && buildIndex < 26)
        {
            float t = AudioManager.instance.GetMusicPlaybackTime("bgm2");
            string pitch = "";
            if ((t >= 10 && t < 22)|| (t >= 50 && t < 62))
            {
                pitch = "-2";
            }
            else if ((t >= 28 && t < 39) || (t >= 68))
            {
                pitch = "-4";
            }
            //print("t: " + t + ", pitch: " + pitch);
            AudioManager.instance.PlaySFX("Lv2 " + "Harp Land" + pitch + "_" + AudioManager.landingSfxID.ToString());

            print("play landing sound");

        }

        AudioManager.landingSfxID++;

        if (AudioManager.landingSfxID > 4)
        {
            AudioManager.landingSfxID = 1;
        }
    }

    public virtual void checkCatching(){
        //Debug.Log("Catched step 1" + name);

        //Keep scanning around itself to find if player is around
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, catchRadius);
        int i = 0;

        while (i < hitColliders.Length){
            GameObject ob = hitColliders[i].gameObject;
            //player catched
            if (ob != gameObject && ob.tag == "Player")//&& Mathf.Abs(Vector3.Distance(transform.position,ob.transform.position) - catchRadius) < 0.1f)
            {
                playerObj = ob;
                spacecraft sc = ob.transform.GetChild(0).GetComponent<spacecraft>();

                //Conditions of automatic switch of planets: the current rotatingPlanet is not null and not equal to the attracting planet
                if(!sc.launched){
                    if (sc.rotatingPlanet != gameObject)//When it is switching to a new planet automatically, the responding time(checkInterval) show be longer so that it will not switch back
                    {
                        //Debug.Log("Slow response");
                        transferCenter(ob, sc, slowRespTime);
                    }
                }else{
                    if (sc.preRotatingPlanet != gameObject){
                        //Conditions of manual switch of planets: the current rotatingPlanet null or it is not equal to the attracting planet
                        //Debug.Log("Quick response");
                        //When it is switching to a new planet manually, the responding time should be 0(player can shoot right after it lands)
                        transferCenter(ob, sc);
                    }
                }
            }
            ++i;
        }
    }

    private void transferCenter(GameObject ob ,spacecraft sc, float checkInterval = 0){

        Vector2 v1 = new Vector2(transform.position.x - sc.transform.position.x,
                                     transform.position.y - sc.transform.position.y);
        Vector2 v2 = sc.transform.parent.GetComponent<Rigidbody2D>().velocity;
        float angle = Vector2.SignedAngle(v1, v2);

        //check if spacecraft is not orbiting the same planet after launch
        //Debug.Log(sc.rotatingPlanet + ", " + (Time.time - sc.checkRotatingTime));
        if (Time.time - sc.checkRotatingTime > checkInterval)
        ////&& angle <= 90f && angle >= -90f )//&& 
        {
            if (!sc.dead)
            {
                PlanetBlink();
            }
            //position fix
            ob.transform.position = (ob.transform.position - transform.position).normalized * catchRadius + transform.position;
            thePlayerOnPlanet = ob;
            //Debug.Log(thePlayerOnPlanet);

            if (sc.energy < Constants.deathHealthVal)
                return;

            //rotate
            //player did not launch
            if (!sc.launched)
            { 

                sc.checkRotatingTime = Time.time;

                sc.preRotatingPlanet = sc.rotatingPlanet;
                if (sc.rotatingPlanet != null)
                {
                    sc.rotatingPlanet.GetComponent<Planet>().playerLeave();
                }

                sc.rotatingPlanet = null;

            }
            sc.rotatingPlanet = gameObject;
            sc.rotation_center = transform.position;
            sc.rotate_on = true;
            sc.launched = false;
            sc.movingStart = false;
            sc.checkRotatingTime = Time.time;

            if (angle < 0f && angle < 90f)
                sc.rotating_dir = -1; //counterclockwise rotation
            else
                sc.rotating_dir = 1; //clockwise rotation 
            
            //Debug.Log("Switched");
            sc.landOn();
            //landing sound 
            if (canPlaySound){
                playLandingSound();
                canPlaySound = false;
            }

            if(transform.Find("secret") != null){
                BadgeManager.instance.showHiddenPlanet(transform.Find("secret").tag);
            }
            catchedAction(sc);

        }
    }

    public void PlanetBlink()
    {
        //if (lightController != null)
            //lightController.enabled = true;
        StartCoroutine(turnoffHalo());
    }

    IEnumerator turnoffHalo()
    {
        while (true)
        {
            if(lightController != null){
                lightController.range += glowIncre;
                //haloController.FindProperty("m_Size").floatValue += glowIncre;
                //haloController.ApplyModifiedProperties();
                yield return new WaitForSeconds(0.005f);

                if (lightController.range > catchRadius * 2.5f)
                {
                    glowIncre = -glowIncre;
                }
                else if (lightController.range < origGlowSize)
                {
                    //haloController.FindProperty("m_Enabled").boolValue = false;
                    //haloController.ApplyModifiedProperties();
                    //lightController.enabled = false;
                    glowIncre = -glowIncre;
                    break;
                }
            }
        }
    }

    public void Recover(){
        if (thePlayerOnPlanet != null)
            thePlayerOnPlanet = null;
    }


}

