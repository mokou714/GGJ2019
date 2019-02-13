using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlanet : MonoBehaviour {
    public enum direction {horizontal, vertical};
    public direction movDir;
    private Vector3 curDir;
    private float x1, y1;
    public float x2, y2;
    private Vector3 start,dest;
    public float speed;
    private Vector3 lastPos;
    private float fixdeDist;
	// Use this for initialization
	void Start () {
        x1 = transform.position.x;
        y1 = transform.position.y;
        start = new Vector3(x1, y1, 0);
        dest = new Vector3(x2, y2, 0);
        curDir = dest - start;
        lastPos = start;
        fixdeDist = Vector3.Distance(dest, start);
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(transform.position, lastPos) >= fixdeDist)
        {
            curDir = curDir * -1;
            lastPos = transform.position;
        }
        float step = speed * Time.deltaTime;
        transform.position = transform.position + curDir * speed;

	}
}
