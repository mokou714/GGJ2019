using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giantPlanet : MonoBehaviour {


    public float effectRadius;
    public float rotatingSpeed;
    public bool rotating;


	// Use this for initialization
	void Start () {
		rotating = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(rotating){
            transform.Rotate(Vector3.forward * Time.deltaTime * rotatingSpeed);
        }
	}
    

    void checkCatching()
    {
        //Keep scanning around itself to find if player is around
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, effectRadius);
        int i = 0;

        while (i < hitColliders.Length)
        {
            GameObject ob = hitColliders[i].gameObject;

            //still thinking...

            ++i;
        }
    }
}
