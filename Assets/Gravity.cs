using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {

    public float mass;
    public float gravityRadius;
    public float G;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        checkGravity();
	}

    void checkGravity(){
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, gravityRadius);
        int i = 0;

        while (i<hitColliders.Length){
            //check if collider is the allowed gameobject

            GameObject ob = hitColliders[i].gameObject;
            if (ob.tag == "spacecraft")
            {

                //apply gravity force
                Vector2 pos1 = new Vector2(transform.position.x, transform.position.y);
                Vector2 pos2 = new Vector2(ob.transform.position.x, ob.transform.position.y);
                float dis = Vector2.Distance(pos1,pos2);
                float cos = Mathf.Abs(pos1.x - pos2.x) / dis;

                ob.GetComponent<Rigidbody2D>().AddForce(ob.transform.right * 10.0f);

            }
            ++i;
        }
    }
}
