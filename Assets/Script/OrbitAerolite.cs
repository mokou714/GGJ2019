using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitAerolite : MonoBehaviour {
    public int speed;
    public Rigidbody2D orbitCenter;
    public float damage;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if(orbitCenter != null)
            transform.RotateAround(orbitCenter.position, Vector3.forward, speed * Time.deltaTime);
	}
}
