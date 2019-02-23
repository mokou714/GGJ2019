﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : MonoBehaviour
{


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
                ob.transform.GetChild(0).GetComponent<spacecraft>().rotation_center = transform.position;
                ob.transform.GetChild(0).GetComponent<spacecraft>().rotate_on = true;
                ob.transform.GetChild(0).GetComponent<spacecraft>().launched = false;

            }
            ++i;
        }
    }
}

