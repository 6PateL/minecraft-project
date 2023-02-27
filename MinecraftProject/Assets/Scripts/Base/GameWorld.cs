using System;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld : MonoBehaviour
{
  public Dictionary<Vector2Int, ChunkData> ChunkDatas = new Dictionary<Vector2Int, ChunkData>();
  public ChunkRenderer chunkPrefab;

  private Camera _camera;

  private void Start()
  {
    _camera = Camera.main;
    
    for (int x = 0; x < 10; x++)
    {
      for (int y = 0; y < 10; y++)
      {
        float xPos = x * ChunkRenderer.ChunkWidth * ChunkRenderer.BlockScale;
        float zPos = y * ChunkRenderer.ChunkWidth * ChunkRenderer.BlockScale;
        
        ChunkData chunkData = new ChunkData();
        chunkData.ChunkPosition = new Vector2Int(x, y);
        chunkData.Blocks = TerrainGenerator.GenerateTerrain((int)xPos, (int)zPos);
        ChunkDatas.Add(new Vector2Int(x,y), chunkData);

        var chunk = Instantiate(chunkPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity, transform);
        chunk.ChunkData = chunkData;
        chunk.ParentWorld = this;

        chunkData.Renderer = chunk; 
      }
    }
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
    {
      bool isDestroying = Input.GetMouseButtonDown(0); 
      
      Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f,0.5f));
      if (Physics.Raycast(ray, out var hitInfo))
      {
        Vector3 blockCenter;
        if (isDestroying) {
          blockCenter = hitInfo.point - hitInfo.normal * ChunkRenderer.BlockScale / 2;
        } else {
          blockCenter = hitInfo.point + hitInfo.normal * ChunkRenderer.BlockScale / 2;
        }
        Vector3Int blockWorldPos = Vector3Int.FloorToInt(blockCenter / ChunkRenderer.BlockScale);
        Vector2Int chunkPos = GetChunkContainingBlock(blockWorldPos);
        if (ChunkDatas.TryGetValue(chunkPos, out ChunkData chunkData))
        {
          Vector3Int chunkOrigin = new Vector3Int(chunkPos.x,0,chunkPos.y) * ChunkRenderer.ChunkWidth;
          if (isDestroying) {
            chunkData.Renderer.DestroyBlock(blockWorldPos - chunkOrigin);
          } else {
            chunkData.Renderer.SpawnBlock(blockWorldPos - chunkOrigin);
          }
        }
      }
    }
  }

  public Vector2Int GetChunkContainingBlock(Vector3Int blockWorldPos)
  {
    return new Vector2Int(blockWorldPos.x / ChunkRenderer.ChunkWidth, blockWorldPos.y / ChunkRenderer.ChunkWidth); 
  }
}
