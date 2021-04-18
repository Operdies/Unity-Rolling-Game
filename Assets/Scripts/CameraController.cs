using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // public Camera Camera { get; set; }
    private Vector3 PlayerOffset;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        // Camera = GetComponent<Camera>();
        PlayerOffset = transform.position - Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
    
    
    private void LateUpdate()
    {
        transform.position = Player.transform.position + PlayerOffset;
    }
}
