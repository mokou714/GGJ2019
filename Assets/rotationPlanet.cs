using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class rotationPlanet : MonoBehaviour
{
    /*
    This class is attached on planet units, responsible for attracting the player, audio playing when orbiting starts
    */

    public float catchRadius;
    public GameObject thePlayerOnPlanet;


 
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkCatching();
    }

    void checkCatching()
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


                    //landing sound  //comment for debug

                   
                    if (SceneManager.GetActiveScene().buildIndex != 0)
                    {
                        //print("sfxNormalLand id: " + AudioManager.sfxNormalLandID);
                        AudioManager.instance.PlaySFX("Harp Land_" + AudioManager.sfxNormalLandID.ToString());

                        AudioManager.sfxNormalLandID++;
                        if (AudioManager.sfxNormalLandID > 4)
                        {
                            AudioManager.sfxNormalLandID = 1;
                        }
                    }

                }

            }
            ++i;

        }
    }

    public void Recover()
    {
        /*
        Todo: none for now
        */

    }


}

