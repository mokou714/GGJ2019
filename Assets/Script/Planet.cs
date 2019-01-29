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
                if(sc.rotating_planet == null || sc.rotating_planet != gameObject){
                    sc.rotating_planet = gameObject;
                    sc.rotation_center = transform.position;
                    sc.rotate_on = true;
                    sc.moving = false;
                    sc.movingStart = false;


                    if (SceneManager.GetActiveScene().buildIndex != 0)
                    {                        
                        //print("sfxNormalLand id: " + AudioManager.sfxNormalLandID);
                        AudioManager.instance.PlaySFX("Harp EFX Land_" + AudioManager.sfxNormalLandID.ToString());

                        AudioManager.sfxNormalLandID++;
                        if (AudioManager.sfxNormalLandID > 4)
                        {
                            AudioManager.sfxNormalLandID = 1;
                        }
                    }

                    if (dustAmount > 0)
                    {
                        float id = Random.Range(0f, 10.1f);
                        print("sfxDustLand id: " + (int)id);
                        AudioManager.instance.PlaySFXEnergyCharge("Sound Charge " + (int)id);
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

