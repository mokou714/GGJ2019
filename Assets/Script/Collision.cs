using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

    public float collide_strengh;
    public bool collided;
    public float asteroidDamage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	

	}

    private void LateUpdate()
    {
        if (collided)
        {
            gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
            collided = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Collision:" + col.gameObject.tag);
        if (col.gameObject.tag == "aerolite")
        {
            col.gameObject.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * collide_strengh;
            transform.GetChild(0).GetComponent<spacecraft>().enegy -= col.gameObject.GetComponent<Aerolite>().damage;
            transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = transform.GetChild(0).GetComponent<spacecraft>().enegy / 100f;
            collided = true;
        }
        else if (col.gameObject.tag == "orbaeroliteEc")
        {
            col.gameObject.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * collide_strengh;
            transform.GetChild(0).GetComponent<spacecraft>().enegy -= col.gameObject.GetComponent<orbAeroliteEclipse>().damage;
            transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = transform.GetChild(0).GetComponent<spacecraft>().enegy / 100f;
            collided = true;
        }
        else if (col.gameObject.tag == "asteroid")
        {
            col.gameObject.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * collide_strengh;
            transform.GetChild(0).GetComponent<spacecraft>().enegy -= asteroidDamage;
            transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = transform.GetChild(0).GetComponent<spacecraft>().enegy / 100f;
            collided = true;
        }
        else if (col.gameObject.tag == "orbaerolite")
        {
            col.gameObject.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * collide_strengh;
            transform.GetChild(0).GetComponent<spacecraft>().enegy -= col.gameObject.GetComponent<OrbitAerolite>().damage;
            transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = transform.GetChild(0).GetComponent<spacecraft>().enegy / 100f;
            collided = true;
        }
    }

}
