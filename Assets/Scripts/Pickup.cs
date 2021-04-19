using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Objective.Register(this);
        
        // Add random rotation to pickup
        this.gameObject.transform.rotation = Random.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.parent.gameObject.SetActive(false);
            Objective.PickupCollected(this);
        }
    }
}
