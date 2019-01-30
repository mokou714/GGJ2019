using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Planet : MonoBehaviour
{

    public int dustAmount;
    public int dustRadius;
    
    public float catching_radius;


    // Use this for initialization
    void Start()
    {                

    }

    // Update is called once per frame
    void Update()
    {
        checkCatcing();
    }

    void checkCatcing()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, catching_radius);
        int i = 0;

        while (i < hitColliders.Length)
        {
            GameObject ob = hitColliders[i].gameObject;
            //player catched
            if (ob != gameObject && ob.tag == "spacecraft")
            {
                //Debug.Log("!!!!" +
                          //"");
                spacecraft sc = ob.transform.GetChild(0).GetComponent<spacecraft>(); 

                //check if spacecraft is not orbiting the same planet after launch
                if(sc.rotating_planet == null || sc.rotating_planet != gameObject)
                {
                    //rotate
                    sc.rotating_planet = gameObject;
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

                    // Debug.Log(angle);
                    // Time.timeScale = 0;



                    //enegy
                    sc.enegy += dustAmount;
                                                           

                    //landing sound
                    
                    if (dustAmount > 0)
                    {
                        AudioManager.instance.PlaySFX("Harp Charge_2");   //Yihui2
                    }
                    else
                    {
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


                    // change
                    if (sc.enegy > 100)
                        sc.enegy = 100;
                    dustAmount = 0;
                    ob.transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = sc.enegy / 100f;
                }
                                                             
            }


            ++i;

        }
    }
}

