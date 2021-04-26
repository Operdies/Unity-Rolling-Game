using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyController : MonoBehaviour
{
    public float Velocity = 10;
    [Tooltip("Degrees / second")]
    public float Torque = 10;
    public GameObject Target;
    private Rigidbody rb { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.Find("Player");
        transform.LookAt(Target.transform);
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        var transformPosition = transform.position;
        var t = Target.transform;
        
        Vector3 targetDirection = (t.position - transformPosition).normalized;
        var targetRotation = Quaternion.LookRotation(targetDirection);

        var direction = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * Torque);
        transform.rotation = direction;
        
        if (math.abs(Vector3.Angle(targetDirection, transform.forward)) < 20.0f)
            rb.AddForce(transform.forward * (Velocity * Time.deltaTime));
        else
            rb.AddForce(-rb.velocity * (Velocity * Time.deltaTime));
    }
}
