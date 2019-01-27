using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartAsteroid : MonoBehaviour {
    private Transform player;
	// Use this for initialization
	void Start () {
        player = transform.Find("Spacecraft");
        Debug.Log(player);
        transform.GetComponent<Rigidbody2D>().velocity = new Vector2(player.position.x, player.position.y + 30);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
