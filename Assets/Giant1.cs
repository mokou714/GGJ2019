using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giant1 : MonoBehaviour {


  
    public float endX;
    public GameObject player;
    public float moveSpeed;
    public bool moving;



	// Use this for initialization
	void Start () {
        moving = true;
		
	}
	
	// Update is called once per frame
	void Update () {
        if (moving) {
            Vector3 newPos = transform.position;
            Vector3 newPos2 = player.transform.position;
            newPos.x += moveSpeed * Time.deltaTime;
            newPos2.x += moveSpeed * Time.deltaTime;


            if (newPos.x > endX)
            {
                moving = false;
                newPos.x = endX;
                player.transform.GetChild(0).GetComponent<spacecraft>().enabled = true;
            }
            transform.position = newPos;
            player.transform.position = newPos2;
        }
    }
}
