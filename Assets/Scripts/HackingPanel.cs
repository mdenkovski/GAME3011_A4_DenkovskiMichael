using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingPanel : MonoBehaviour
{
    public static HackingPanel Instance;

    //board Setup
    private GameObject[,] tiles;
    public GameObject tile;
    //number of tiles to make the game
    public int xSize, ySize;
    public int GameXSize, GameYSize;
    private float TileScale;
    Vector2 tileSize;

    private int NumOptions;
    private List<string> Options;
    string Letters = "ABCDEFG";


    //Buffer
    private int BufferSize;
    private List<string> BufferChoices;

    //Goal
    private List<string> AnswerKey;
    private int NumAnswerComponents;

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

        tileSize = tile.GetComponentInChildren<SpriteRenderer>().bounds.size;
        TileScale = (GameXSize / xSize) / tileSize.x;

        SetEasyDifficulty();
        
    }

    public void OnDisable()
    {
        ClearBoard();
    }

    private void CreateBoard(float xTileSize, float yTileSize)
    {
        tiles = new GameObject[xSize, ySize];

        float startX = transform.position.x;
        float startY = transform.position.y;

        

        //generate our tiles
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

        //ensure there is at least one solution
        GenerateSolution();

        //fill out remaining tiles
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {

                TileScript Tile = tiles[x,y].GetComponent<TileScript>();
                if (!Tile.HasContent())
                {
                    //populate empty tiles with a random option
                    Tile.Initialize(Options[Random.Range(0, Options.Count)]);
                }

            }
        }
    }

    private void GenerateSolution()
    {
        int prevRow = -1;
        int prevCol = -1;
        bool validChoice = false;
        for (int i = 0; i < NumAnswerComponents; i++)
        {
            validChoice = false;
            if (i % 2 == 0) //even number so a row
            {
                while (!validChoice)
                {
                    int selection = Random.Range(0, xSize);
                    TileScript Tile = tiles[selection, (prevRow < 0 ? ySize-1 : prevRow)].GetComponent<TileScript>();
                    if (!Tile.HasContent())
                    {
                        Tile.Initialize(AnswerKey[i]);
                        validChoice = true;
                        prevCol = selection;
                    }
                }
            }
            else //  odd number so in a column
            {
                while (!validChoice)
                {
                    int selection = Random.Range(0, ySize);
                    TileScript Tile = tiles[(prevCol < 0 ? xSize-1 : prevCol), selection].GetComponent<TileScript>();
                    if (!Tile.HasContent())
                    {
                        Tile.Initialize(AnswerKey[i]);
                        validChoice = true;
                        prevRow = selection;
                    }
                }
            }
        }
    }


    public void SetEasyDifficulty()
    {
        NumOptions = 4;
        BufferSize = 4;
        NumAnswerComponents = 2;

        ClearBoard();

        PopulateOptions();
        CreateBoard(tileSize.x * TileScale, tileSize.y * TileScale);
    }

    public void PopulateOptions()
    {
        Options = new List<string>();
        AnswerKey = new List<string>();

        for (int i = 0; i < NumOptions; i++)
        {
            string choice = Letters[Random.Range(0, Letters.Length)] + Random.Range(0, 10).ToString();
            Options.Add(choice);
        }

        for (int i = 0; i < NumAnswerComponents; i++)
        {
            //add a random selection to the Answer Key Goal
            AnswerKey.Add(Options[Random.Range(0, Options.Count)]);
        }

    }

    private void ClearBoard()
    {
        if (tiles == null) return;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                Destroy(tiles[x, y]);
            }
        }
    }
}
