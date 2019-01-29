// Added by Ke on 1/28/2019
// implemented object pooling to reuse asteroids, also added code in MeteorSpawner.cs
// reference: https://unity3d.com/learn/tutorials/topics/scripting/object-pooling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAsteroid : MonoBehaviour {

    public float lifeTime = 5f;

    void OnEnable()
    {
        Invoke("Destroy", lifeTime);
    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
