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
    private float backSpeed = 80;
    private float maxX, maxY;
    private int offset = 10;

    private float maxWaitTime = 2f;
    public bool tell = false;
    // Use this for initialization
	void Start () {
        if (orbitCenter != null){
            radius = Vector3.Distance(transform.position, orbitCenter.position);
        }
        origPosition = transform.position;
        maxX = 20f;
        maxY = 20f;
	}
	
	// Update is called once per frame
	void Update () {
        if (!movingBack && orbitCenter != null){
            transform.RotateAround(orbitCenter.position, Vector3.forward, speed * 0.01f);
        }
        else if(movingBack){
            if (GetComponent<Rigidbody2D>().velocity.magnitude > 0)
                GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            float step = backSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, origPosition, step);
            //Disable the collider when moveing in case of hitting other asteroids
            if (transform.GetComponent<Collider2D>().enabled)
                //transform.GetComponent<Collider2D>().enabled = false;
            //print(Vector2.Distance(transform.position, origPosition));
            if (Vector2.Distance(transform.position, origPosition) < 0.01f)
            {
                //print("Got back");
                transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                transform.GetComponent<Rigidbody2D>().freezeRotation = true;
                transform.GetComponent<Collider2D>().enabled = true;
                if (transform.parent != null && transform.parent.GetComponent<AsteroidController>() != null)
                {
                    if(!tell){
                        transform.parent.GetComponent<AsteroidController>().readyNum += 1;
                        tell = true;
                    }
                }else{
                    movingBack = false;
                }
            }
        }else
        {
            if (Mathf.Abs(transform.position.x) > maxX + offset || Mathf.Abs(transform.position.y) > maxY + offset)
            {
                transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                //transform.GetComponent<Collider2D>().enabled = false;
            }
        }

	}


	public void Recover(){
        movingBack = true;
    }

    IEnumerator startToMove(){
        yield return new WaitForSeconds(0.5f);
        movingBack = false;
    }

}
