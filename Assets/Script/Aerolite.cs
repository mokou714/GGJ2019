using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aerolite : MonoBehaviour {
    public float EscapeRange;
    private int[] dirs = {1, 0, -1};
    public int damage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        
        Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] aroundColliders = Physics2D.OverlapCircleAll(myPos, EscapeRange);
        List<Vector2> escapeList = new List<Vector2>();
        if(myPos.x >= 11){
            escapeList.Add(new Vector2(myPos.x - 11, 1));
            escapeList.Add(new Vector2(myPos.x - 11, -1));
        }
        if (myPos.x <= -11)
        {
            escapeList.Add(new Vector2(myPos.x + 11, 1));
            escapeList.Add(new Vector2(myPos.x + 11, -1));
        }
        if (myPos.y >= 8)
        {
            escapeList.Add(new Vector2(1, myPos.y - 8));
            escapeList.Add(new Vector2(-1, myPos.y - 8));
        }
        if (myPos.y <= -8)
        {
            escapeList.Add(new Vector2(1, myPos.y + 8));
            escapeList.Add(new Vector2(-1, myPos.y + 8));
        }

        foreach (Collider2D objAround in aroundColliders){
            GameObject obj = objAround.gameObject;
            if(obj.tag == "planet"){
                Vector2 pltPos = new Vector2(obj.transform.position.x, obj.transform.position.y);
                Vector2 posDiff = pltPos - myPos;
                posDiff.Normalize();
                escapeList.Add(posDiff);
            }
        }


        Vector2 rand_dir = randDirGen();

        while(escapeList.Contains(rand_dir)){
            rand_dir = randDirGen();
        }
        //Debug.Log("Random Dir:" + rand_dir);

        int FloatStrenght = Random.Range(1, 5);

        GetComponent<Rigidbody2D>().AddForce(rand_dir * FloatStrenght);
        transform.Rotate(0, 0, 1);

    }

    private Vector2 randDirGen(){
        int rand_x = dirs[Random.Range(0, 3)];
        int rand_y = dirs[Random.Range(0, 3)];
        return new Vector2(rand_x, rand_y);
    }

}
