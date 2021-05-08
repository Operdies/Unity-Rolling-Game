using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMoveObject : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Target;
    public Vector3 Direction;
    public float MaxDistance;
    public float Speed;
    private Vector3 startLocation;

    private void Start()
    {
        startLocation = Target.transform.position;
    }

    private void OnCollisionEnter(Collision other)
    {
        StopCoroutine(nameof(Reverse));
        StartCoroutine(nameof(Animate));
    }

    private void OnCollisionExit(Collision other)
    {
        StopCoroutine(nameof(Animate));
        StartCoroutine(nameof(Reverse));
    }

    IEnumerator Animate()
    {
        var position = Target.transform.position;
        while (Vector3.Distance(startLocation, position) < MaxDistance)
        {
            Target.transform.position = Vector3.Lerp(position, startLocation +  (Direction.normalized * MaxDistance), Speed * Time.deltaTime);
            yield return null;
            position = Target.transform.position;
        }
    }

    IEnumerator Reverse()
    {
        var elapsed = 1f;
        var position = Target.transform.position;
        while (Vector3.Distance(startLocation, position) > 0.02f)
        {
            Target.transform.position = Vector3.Lerp(position, startLocation +  (Direction.normalized * MaxDistance * -1), Speed * Time.deltaTime);
            yield return null;
            position = Target.transform.position;
            elapsed += Time.deltaTime;
            if (elapsed > 2)
                break;
        }

        yield return new WaitForSeconds(1);
        Target.transform.position = startLocation;
    }
}
