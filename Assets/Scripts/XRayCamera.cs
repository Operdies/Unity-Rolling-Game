using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public static class DumbExtensions
{
    public static void InvokeAll(this IEnumerable<Action> actions)
    {
        foreach (var action in actions)
        {
            action.Invoke();
        }
    }
}
public class XRayCamera : MonoBehaviour
{
    public GameObject Player;
    [Range(0, 1)]
    public float XRayAmount = 0.5f;

    private float Transparency => 1 - XRayAmount;

    private void Start()
    {
        StartCoroutine(nameof(XRay));
    }
    

    private IEnumerator XRay()
    {
        Action[] restore = new Action[] { };

        while (gameObject.activeSelf)
        {
            restore.InvokeAll();

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

            restore = MakeTransparent(items);

            yield return new WaitForSeconds(0.05f);
        }
    }

    /// <summary>
    /// Make obstacles transparent
    /// Return array of methods to restore previous state
    /// </summary>
    /// <param name="obstructions"></param>
    /// <returns></returns>
    private Action[] MakeTransparent(RaycastHit[] obstructions)
    {
        var restore = new List<Action>();
        foreach (var obstruction in obstructions)
        {
            var obstructionRenderer = obstruction.transform.gameObject.GetComponent<Renderer>();
            if (obstructionRenderer != null)
            {
                var material = obstructionRenderer.material;
                var materialColor = material.color;
                var previousAlpha = materialColor.a;
                
                materialColor.a = Transparency;
                material.color = materialColor;
                
                restore.Add(() =>
                {
                    materialColor.a = previousAlpha;
                    material.color = materialColor;
                });
            }
        }

        return restore.ToArray();
    }
}
