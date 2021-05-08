using System;
using UnityEngine;

public class GrassCollisionChecker : MonoBehaviour
{
    public int[,] Details { get; set; }
    public Terrain terrain { get; set; }
    [Range(0, 512)]
    public float GrassRadius = 100f;
    [Range(5, 20)]
    public int CollectionRadius = 12;
    void SetFadeFromCenter(Terrain t, float radius)
    {
        var terrainData = t.terrainData;
        var center = new Vector2(terrainData.detailWidth / 2f, terrainData.detailHeight / 2f);
        var map = t.terrainData.GetDetailLayer(0, 0, terrainData.detailWidth, terrainData.detailHeight, 0);

        // For each pixel in the detail map...
        for (var y = 0; y < t.terrainData.detailHeight; y++)
        {
            for (var x = 0; x < t.terrainData.detailWidth; x++)
            {
                if (Vector2.Distance(center, new Vector2(x, y)) < radius)
                {
                    map[x, y] = 1;
                    TotalGrass += 1;
                }
                else
                    map[x, y] = 0;
            }
        }

        // Assign the modified map back.
        Details = map;
        t.terrainData.SetDetailLayer(0, 0, 0, map);

    }

    private int TotalGrass { get; set; } = 0;
    private int GrassCollected { get; set; } = 0;

    private void SetDetails()
    {
        terrain.terrainData.SetDetailLayer(0, 0, 0, Details);
    }

    private Vector2 Center => new Vector2(terrain.terrainData.detailWidth / 2f, terrain.terrainData.detailHeight / 2f);

    (int x, int z) PositionToIndex(Vector3 pos)
    {
        var terPosition = terrain.transform.position;
        var data = terrain.terrainData;
        var x = ((pos.x - terPosition.x) / data.size.x) * data.detailWidth;
        var z = ((pos.z - terPosition.z) / data.size.z) * data.detailHeight;

        return ((int) x, (int) z);
    }

    void Start()
    {
        terrain = gameObject.GetComponent<Terrain>();
        SetFadeFromCenter(terrain, GrassRadius);
    }

    private void OnDestroy()
    {
        SetFadeFromCenter(terrain, 0);
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var (playerX, playerY) = PositionToIndex(other.gameObject.transform.position);
            var playerVector = new Vector2(playerX, playerY);

            if (playerY < 0 || playerX < 0 || playerY * playerX >= Details.Length)
                return;
            
            bool detailsSet = false;

            for (int y2 = playerY - CollectionRadius; y2 < playerY + CollectionRadius; y2++)
            {
                for (int x2 = playerX - CollectionRadius; x2 < playerX + CollectionRadius; x2++)
                {
                    if (y2 < 0 || x2 < 0 || y2 * x2 >= Details.Length)
                        continue;
                    if (Vector2.Distance(new Vector2(x2, y2), playerVector) > CollectionRadius)
                        continue;
                    
                    if (Details[y2, x2] > 0)
                    {
                        detailsSet = true;
                        Details[y2, x2] = 0;
                        OnGrassCollected(other);
                    }
                }
            }
            if (detailsSet)
                SetDetails();
        }
    }

    private void OnGrassCollected(Collision player)
    {
        GrassCollected += 1;
        player.rigidbody.velocity *= 0.99f;

        if (GrassCollected % (TotalGrass / 1000) == 0)
        {
            player.gameObject.transform.localScale *= 1.002f;
            player.gameObject.GetComponent<PlayerController>().PowerUp(1.01f);
        }
    }
}
