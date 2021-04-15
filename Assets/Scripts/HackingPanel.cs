using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingPanel : MonoBehaviour
{
    public static HackingPanel Instance;

    private GameObject[,] tiles;
    public GameObject tile;
    public int xSize, ySize;
    public int GameXSize, GameYSize;
    private float TileScale;

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        Vector2 tileSize = tile.GetComponentInChildren<SpriteRenderer>().bounds.size;
        TileScale = (GameXSize / xSize) / tileSize.x;

        CreateBoard(tileSize.x * TileScale, tileSize.y * TileScale);
    }



    private void CreateBoard(float xTileSize, float yTileSize)
    {
        tiles = new GameObject[xSize, ySize];

        float startX = transform.position.x;
        float startY = transform.position.y;

        

        //populate our tiles
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject newTile = Instantiate(tile, new Vector3(startX + (xTileSize * x), startY + (yTileSize * y), 0), tile.transform.rotation);
                newTile.transform.localScale = TileScale * Vector3.one;
                tiles[x, y] = newTile;
                newTile.transform.parent = transform;


                
            }
        }
    }

}
