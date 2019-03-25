using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

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


    public SerializedObject haloController;
    private float glowIncre;
    public float origGlowSize;
    private Transform planetSprite;
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

        haloController = new SerializedObject(gameObject.GetComponent("Halo"));
        planetSprite = transform.Find("Planet2");
        planetBottom = transform.Find("Bottom");

        if(haloController != null){
            haloController.FindProperty("m_Size").floatValue = 0.5f * planetSprite.localScale.x;
            origGlowSize = haloController.FindProperty("m_Size").floatValue;
            haloController.ApplyModifiedProperties();
        }

        Debug.Log(haloController);
        if(planetBottom != null){
            planetBottom.localScale = planetSprite.localScale * (float)(0.3f);
        }
        //Make sure the planet glow's increment is consistent with the player's 
        glowIncre = 0.03f * ((catchRadius + 0.4f - origGlowSize) / 0.6f);
    }

    public virtual void catchedAction(spacecraft sc) { }

    public virtual void playerLeave(){
        canPlaySound = true;
        thePlayerOnPlanet = null;
    }

    public virtual void playLandingSound() {
        if (SceneManager.GetActiveScene().buildIndex != 0){
            AudioManager.instance.PlaySFX("Harp Land_" + AudioManager.sfxNormalLandID.ToString());

            AudioManager.sfxNormalLandID++;
            if (AudioManager.sfxNormalLandID > 4){
                AudioManager.sfxNormalLandID = 1;
            }
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
                        Debug.Log("Slow response");
                        transferCenter(ob, sc, slowRespTime);
                    }
                }else{
                    if (sc.preRotatingPlanet != gameObject){
                        //Conditions of manual switch of planets: the current rotatingPlanet null or it is not equal to the attracting planet
                        Debug.Log("Quick response");
                        //When it is switching to a new planet manually, the responding time should be 0(player can shoot right after it lands)
                        transferCenter(ob, sc);
                    }
                }
            }
            ++i;
        }
    }

    private void transferCenter(GameObject ob ,spacecraft sc, float checkInterval = 0){
        if (!sc.dead){
            PlanetBlink();
        }
        Vector2 v1 = new Vector2(transform.position.x - sc.transform.position.x,
                                     transform.position.y - sc.transform.position.y);
        Vector2 v2 = sc.transform.parent.GetComponent<Rigidbody2D>().velocity;
        float angle = Vector2.SignedAngle(v1, v2);

        //check if spacecraft is not orbiting the same planet after launch
        //Debug.Log(sc.rotatingPlanet + ", " + (Time.time - sc.checkRotatingTime));
        if (Time.time - sc.checkRotatingTime > checkInterval)
        ////&& angle <= 90f && angle >= -90f )//&& 
        {
            //position fix
            ob.transform.position = (ob.transform.position - transform.position).normalized * catchRadius + transform.position;

            thePlayerOnPlanet = ob;

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
                    //Debug.Log("!!!!!!!!");
                    sc.rotatingPlanet.GetComponent<Planet>().playerLeave();
                    Debug.Log(sc.rotatingPlanet.name);
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


            catchedAction(sc);
            //Debug.Log("Switched");
            sc.landOn();
            //landing sound 
            if (canPlaySound){
                playLandingSound();
                canPlaySound = false;
            }
        }
    }

    public void PlanetBlink()
    {
        if (haloController != null)
        {
            haloController.FindProperty("m_Enabled").boolValue = true;
            haloController.ApplyModifiedProperties();
        }
        StartCoroutine(turnoffHalo());
    }

    IEnumerator turnoffHalo()
    {
        while (true)
        {
            if(haloController != null){
                haloController.FindProperty("m_Size").floatValue += glowIncre;
                haloController.ApplyModifiedProperties();
                yield return new WaitForSeconds(0.001f);

                if (haloController.FindProperty("m_Size").floatValue > catchRadius + 0.4f)
                {
                    glowIncre = -glowIncre;
                }
                else if (haloController.FindProperty("m_Size").floatValue <= origGlowSize)
                {
                    haloController.FindProperty("m_Enabled").boolValue = false;
                    haloController.ApplyModifiedProperties();
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

