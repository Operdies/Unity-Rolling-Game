using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Sucker : MonoBehaviour
{
    [Tooltip("Increasing the mass increases the attraction force")]
    [Range(10, 10000)]
    public float Mass;
    
    GameObject Player { get; set; }
    Rigidbody rb { get; set; }
    private float snapped { get; set; }

    void Start()
    {
        Player = GameObject.Find("Player");
        rb = Player.GetComponent<Rigidbody>();
    }

    IEnumerator MovePlayerIntoHole(Vector3 holePosition)
    {
        Vector3 start = rb.position;
        var distance = Vector3.Distance(holePosition, rb.position);
        var initialDistance = distance;
 
        while (distance > 0.1f)
        {
            distance = Vector3.Distance(holePosition, rb.position);
            rb.MovePosition (Vector3.Lerp (start, holePosition, initialDistance / 40));
            start = rb.position;
 
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (snapped > 0)
        {
            snapped = -1;
            // StartCoroutine(MovePlayerIntoHole(transform.position));
        }
    }

    private void Update()
    {
        snapped += Time.deltaTime;
        var G = 200f * Time.deltaTime;
        var holePosition = transform.position;
        var playerPosition = Player.transform.position;
        
        var r = Vector3.Distance(holePosition, playerPosition);

        const float playerWeight = 1;

        // F = (GMm)/(^2
        var F = (G * playerWeight * Mass) / (r*r);
        Vector3 targetDirection = (holePosition - playerPosition).normalized;
        // if (targetDirection.y < 0)
        //     targetDirection.y = -targetDirection.y * 0.1f;
        rb.AddForce(F * targetDirection);
    }
}
