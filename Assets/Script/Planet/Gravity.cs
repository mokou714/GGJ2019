using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {

    private float mass;
    public float gravityRadius;
    public float velocity;

    private bool collided;

	// Use this for initialization
	void Start () {

        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, velocity);
        mass = gameObject.GetComponent<Rigidbody2D>().mass;
    }
	
	// Update is called once per frame
	void Update () {

        //checkGravity();

	}

    void checkGravity(){
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, gravityRadius);
        int i = 0;

        while (i < hitColliders.Length)
        {
            //check if collider is the allowed gameobject
            GameObject ob = hitColliders[i].gameObject;
            if (ob != gameObject
                &&
                (
                (ob.tag == "planet" && gameObject.tag == "spacecraft")
                ||
                (ob.tag == "spacecraft" && gameObject.tag == "planet")
                   )
               )
                
            {
                if (ob.tag == "spacecraft")
                    ob.GetComponent<Gravity>().gravityRadius = gravityRadius;

                //apply gravity force
                Vector2 pos1 = new Vector2(transform.position.x, transform.position.y);
                Vector2 pos2 = new Vector2(ob.transform.position.x, ob.transform.position.y);
                float dis = Vector2.Distance(pos1,pos2);

                float cos = Mathf.Abs(pos1.x - pos2.x) / dis;
                float sin = Mathf.Abs(pos1.y - pos2.y) / dis;

                float F = mass * ob.gameObject.GetComponent<Gravity>().mass / (dis*dis);

                Vector2 x_y_dir = new Vector2(pos1.x - pos2.x, pos1.y - pos2.y);

                x_y_dir.Normalize();

                ob.GetComponent<Rigidbody2D>().AddForce(ob.transform.right * x_y_dir.x);
                ob.GetComponent<Rigidbody2D>().AddForce(ob.transform.up * x_y_dir.y);

           
            }
            ++i;
        }
    }


    bool floatisEqual(float a, float b)
    {
        if (a >= b - Mathf.Epsilon && a <= b + Mathf.Epsilon)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "planet")
        {
            collided = true;
            //gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
        }
    }

    private void LateUpdate()
    {
        if(collided){
            gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
        }
    }

}
