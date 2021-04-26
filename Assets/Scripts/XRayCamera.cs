using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class XRayCamera : MonoBehaviour
{
    public GameObject Player;

    private void Start()
    {
        StartCoroutine(nameof(XRay));
    }
    

    private IEnumerator XRay()
    {
        RaycastHit[] previous = null;
        
        while (gameObject.activeSelf)
        {
            if (previous?.Length > 0)
                Restore(previous);
            var cameraPosition = transform.position;
            var playerPosition = Player.transform.position;

            var distance = Vector3.Distance(cameraPosition, playerPosition);
            var obstructions = new RaycastHit[3];
            var size = Physics.RaycastNonAlloc(cameraPosition, playerPosition - cameraPosition, obstructions, distance);

            var items = obstructions.Take(size).Where(o =>
            {
                var transform = o.transform;
                if (transform != null)
                {
                    var go = transform.gameObject;
                    if (go.CompareTag("Player") || go.CompareTag("Deadly"))
                        return false;

                    return true;
                }

                return false;
            }).ToArray();

            MakeTransparent(items);
            previous = items;

            yield return new WaitForSeconds(0.05f);
        }
        
        yield break;
    }

    private void MakeTransparent(RaycastHit[] obstructions)
    {
        foreach (var obstruction in obstructions)
        {
            var obstructionRenderer = obstruction.transform.gameObject.GetComponent<MeshRenderer>();
            if (obstructionRenderer != null)
                obstructionRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }

    private void Restore(RaycastHit[] obstructions)
    {
        foreach (var obstruction in obstructions)
        {
            var obstructionRenderer = obstruction.transform.gameObject.GetComponent<MeshRenderer>();
            if (obstructionRenderer != null)
                obstructionRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }        
    }
}
