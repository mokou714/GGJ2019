using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbitAsteroid : MonoBehaviour {
    public float speed;
    public Rigidbody2D orbitCenter;
    public float damage;
    private Vector3 origPosition;
    public float radius;
    public bool movingBack = false;
    private float backSpeed = 50;
    private float maxX, maxY;
    private int offset = 10;

    // Use this for initialization
	void Start () {
        origPosition = transform.position;
        radius = Vector3.Distance(transform.position, orbitCenter.position);
        maxX = 100f;
        maxY = 100f;
	}
	
	// Update is called once per frame
	void Update () {
        if(!movingBack && orbitCenter != null)
            transform.RotateAround(orbitCenter.position, Vector3.forward, speed * Time.deltaTime);
        if(movingBack == true){
            float step = backSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, origPosition, step);

            //Disable the collider when moveing in case of hitting other asteroids
            if (transform.GetComponent<BoxCollider2D>().enabled)
                transform.GetComponent<BoxCollider2D>().enabled = false;
            if (transform.position == origPosition)
            {
                movingBack = false;
                transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                transform.GetComponent<Rigidbody2D>().freezeRotation = true;
                transform.GetComponent<BoxCollider2D>().enabled = true;
            }
        }else
        {
            if (Mathf.Abs(transform.position.x) > maxX + offset || Mathf.Abs(transform.position.y) > maxY + offset)
            {
                transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                transform.GetComponent<BoxCollider2D>().enabled = false;
            }
        }

	}
}
