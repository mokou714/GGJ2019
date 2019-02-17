using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Planet : MonoBehaviour
{
    /*
    Parent class for all kinds of planets
    */
    public GameObject thePlayerOnPlanet;
    public bool canPlaySound = true;
    protected GameObject playerObj;
    public float catchRadius;
    public int dustAmount;
    public float rotatingCheckTime;
    public bool isSwitching;
    private float checkTimeInterval = 0.1f;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    /*Abstract class to be implemented in child class, being called after catching the planet, 
    parameter is reference to spacecraft element on the player*/
    public abstract void catchedAction(spacecraft sc);

    public void checkCatching()
    {
        if (thePlayerOnPlanet != null){
            return;
        }

        //Debug.Log("Catched step 1" + name);

        //Keep scanning around itself to find if player is around
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, catchRadius);
        int i = 0;
        //if player leaves the planet, delete the reference
        if (thePlayerOnPlanet != null)
        {
            if (thePlayerOnPlanet.transform.GetChild(0).GetComponent<spacecraft>().moving == true)
                thePlayerOnPlanet = null;
        }

        while (i < hitColliders.Length)
        {
            GameObject ob = hitColliders[i].gameObject;
            //player catched
            if (ob != gameObject && ob.tag == "Player" && Mathf.Abs(Vector3.Distance(transform.position,ob.transform.position) - catchRadius) < 0.1f)
            {
                playerObj = ob;
                spacecraft sc = ob.transform.GetChild(0).GetComponent<spacecraft>();

                //Conditions of automatic switch of planets: the current rotatingPlanet is not null and not equal to the attracting planet
                if(sc.rotatingPlanet != null && sc.rotatingPlanet != gameObject){
                    Debug.Log("Slow response");
                    //When it is switching to a new planet automatically, the responding time show be longer
                    transferCenter(ob, sc, 1f);
                }else{
                    //Conditions of manual switch of planets: the current rotatingPlanet null or it is not equal to the attracting planet
                    if(sc.rotatingPlanet == null || (sc.rotatingPlanet != gameObject)){
                        Debug.Log("Quick response");
                        //When it is switching to a new planet manually, the responding time show be shorter
                        transferCenter(ob, sc, 0.1f);
                    }
                }
            }
            ++i;

        }
    }

    private void transferCenter(GameObject ob ,spacecraft sc, float checkInterval){

        Vector2 v1 = new Vector2(transform.position.x - sc.transform.position.x,
                                     transform.position.y - sc.transform.position.y);

        Vector2 v2 = sc.transform.parent.GetComponent<Rigidbody2D>().velocity;
        float angle = Vector2.SignedAngle(v1, v2);

        //check if spacecraft is not orbiting the same planet after launch
        Debug.Log(sc.rotatingPlanet + ", " + (Time.time - sc.checkRotatingTime));
        if (Time.time - sc.checkRotatingTime > checkInterval)
        ////&& angle <= 90f && angle >= -90f )//&& 
        {
            thePlayerOnPlanet = ob;

            if (sc.energy < Constants.deathHealthVal)
                return;

            //rotate
            if (!sc.moving)
            { //player did not launch
                sc.prevRotatingPlanet = sc.rotatingPlanet;
                sc.checkRotatingTime = Time.time;
                if (sc.prevRotatingPlanet != null)
                    sc.prevRotatingPlanet.GetComponent<Planet>().thePlayerOnPlanet = null;

            }
            sc.rotatingPlanet = gameObject;

            sc.rotation_center = transform.position;
            sc.rotate_on = true;
            sc.moving = false;
            sc.movingStart = false;
            sc.checkRotatingTime = Time.time;

            if (angle < 0f && angle < 90f)
                sc.rotating_dir = -1; //counterclockwise rotation
            else
                sc.rotating_dir = 1; //clockwise rotation 

            sc.prevRotatingPlanet = sc.preTemp;

            catchedAction(sc);
            Debug.Log("Switched");

            //landing sound  //comment for debug
            if (canPlaySound)
            {
                if (dustAmount > 0)
                {
                    //print("plays harp charge");

                    AudioManager.instance.PlaySFX("Harp Charge_2");   //Play the audio for absorbing dust
                }
                else
                {
                    if (SceneManager.GetActiveScene().buildIndex != 0)
                    {
                        AudioManager.instance.PlaySFX("Harp Land_" + AudioManager.sfxNormalLandID.ToString());

                        AudioManager.sfxNormalLandID++;
                        if (AudioManager.sfxNormalLandID > 4)
                        {
                            AudioManager.sfxNormalLandID = 1;
                        }
                    }
                }

                canPlaySound = false;
            }
        }
    }


}

