using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                Debug.Log("!!!!" +
                          "");
                spacecraft sc = ob.transform.GetChild(0).GetComponent<spacecraft>(); 
                if(sc.rotating_planet == null || sc.rotating_planet != gameObject){
                    sc.rotating_planet = gameObject;
                    sc.rotation_center = transform.position;
                    sc.rotate_on = true;
                    sc.moving = false;
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

