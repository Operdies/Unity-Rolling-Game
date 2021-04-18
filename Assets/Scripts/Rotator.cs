using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float x = 0f;

    public float y = 0f;

    public float z = 0f;

    void Start()
    {
        // Avoid calling update if the rotator isn't active
        if (math.abs(x + y + z) < 0.01f)
        {
            enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(x, y, z) * Time.deltaTime );
    }
}
