using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DefaultNamespace;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Objective.Register(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            Objective.PickupCollected(this);
        }
    }
}
