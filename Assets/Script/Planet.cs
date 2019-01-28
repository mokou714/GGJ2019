using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{

    public int dustAmount;
    public int dustRadius;


    public float catching_radius;
    private GameObject[] sounds;
    private GameObject sound;
    private AudioSource dustLand;
    private AudioSource normalLand;

    // Use this for initialization
    void Start()
    {
        sounds = GameObject.FindGameObjectsWithTag("background");
        sounds = GameObject.FindGameObjectsWithTag("bgm");

        foreach (GameObject obj in sounds)
        {
            sound = obj;
        }

        if (sound != null)
        {
            AudioSource[] ts = sound.GetComponentsInChildren<AudioSource>();
            dustLand = ts[0];
 

        }
        

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
                if(sc.rotating_planet == null || sc.rotating_planet != gameObject){
                    sc.rotating_planet = gameObject;
                    sc.rotation_center = transform.position;
                    sc.rotate_on = true;
                    sc.moving = false;
                    sc.movingStart = false;
                    if (dustLand != null && normalLand != null)
                    {
                        if (dustAmount > 0)
                        {
                            dustLand.Play();
                        }
                        else
                        {
                            normalLand.Play();
                        }
                    }

                }



                sc.enegy += dustAmount;

                if (sc.enegy > 100)
                    sc.enegy = 100;
                dustAmount = 0;

                ob.transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = sc.enegy / 100f;




            }
            ++i;
        }
    }
}

