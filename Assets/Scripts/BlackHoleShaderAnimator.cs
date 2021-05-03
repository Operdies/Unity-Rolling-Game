using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlackHoleShaderAnimator : MonoBehaviour
{
    private Renderer holeRenderer { get; set; }
    float tileX = 1f;
    float tileY = 1f;
    private float offsetX = 1f;
    private float offsetY = 1f;

    // Start is called before the first frame update
    void FixedUpdate()
    {
        const float range = 0.05f;
        tileX += Random.Range(-range, range);
        tileY += Random.Range(-range, range);
        
        offsetX += Random.Range(-range, range);
        offsetY += Random.Range(-range/2, range*2);
        
        // var texture = holeRenderer.material.GetTexture("MyCoolTexture");
        var textureName = holeRenderer.material.GetTexturePropertyNameIDs().First();
        holeRenderer.material.SetTextureScale(textureName, new Vector2(tileX, tileY));
        holeRenderer.material.SetTextureOffset(textureName, new Vector2(offsetX, offsetY));
    }

    private void Awake()
    {
        holeRenderer = GetComponent<Renderer>();
    }
}
