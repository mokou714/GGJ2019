using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyBGM : MonoBehaviour
{

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}