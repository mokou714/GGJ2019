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

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        checkCatching();
    }

    /*Abstract class to be implemented in child class, being called after catching the planet, 
    parameter is reference to spacecraft element on the player*/
    public abstract void catchedAction(spacecraft sc);

    public void checkCatching()
    {
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
            if (ob != gameObject && ob.tag == "Player")
            {
                playerObj = ob;
                spacecraft sc = ob.transform.GetChild(0).GetComponent<spacecraft>();

                //check if spacecraft is not orbiting the same planet after launch
                if (sc.prevRotatingPlanet == null || sc.prevRotatingPlanet != gameObject)
                {
                    //store player reference
                    thePlayerOnPlanet = ob;
                    if (sc.energy < Constants.deathHealthVal)
                        return;
                    //rotate
                    sc.rotatingPlanet = gameObject;
                    sc.rotation_center = transform.position;
                    sc.rotate_on = true;
                    sc.moving = false;
                    sc.movingStart = false;

                    Vector2 v1 = new Vector2(transform.position.x - sc.transform.position.x,
                                             transform.position.y - sc.transform.position.y);
                    //v1.Normalize();
                    Vector2 v2 = sc.transform.parent.GetComponent<Rigidbody2D>().velocity;
                    //v2.Normalize();

                    float angle = Vector2.SignedAngle(v1, v2);

                    if (angle < 0f && angle < 90f)
                        sc.rotating_dir = -1; //counterclockwise rotation
                    else
                        sc.rotating_dir = 1; //clockwise rotation 

                    catchedAction(sc);

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
            ++i;

        }
    }

}

