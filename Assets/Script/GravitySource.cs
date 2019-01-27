using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySource : MonoBehaviour
{
    
    public Rigidbody2D rb;
    public Vector3 force;
    public List<GravitySource> bodies;
    private const int G = 600;
    public Rigidbody2D _body;

    void Start()
    {
        if (bodies == null)
            bodies = new List<GravitySource>();
        //Addiing this body to the list of all the bodies in the game
        bodies.Add(this);

        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float m1 = rb.mass;
        float m2 = _body.mass;

        float r = Vector2.Distance(this.transform.position, _body.transform.position);
        Debug.Log("distance of " + _body + ": " + r);

        float ForceMag = (G * m1 * m2) / (r * r);
        Vector2 dir = _body.transform.position - transform.position;
        dir.Normalize();

        Vector2 F = ForceMag * dir;
        //If the body gets too close, repel it so that they won't get attached. But if you dont want that, you can remove this line            
        if (r < 18) F = -F / 4;
        force = F;
        rb.AddForce(F);

    }
}