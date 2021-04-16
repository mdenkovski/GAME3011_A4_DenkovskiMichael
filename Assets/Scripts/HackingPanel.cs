using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingPanel : MonoBehaviour
{
    public static HackingPanel Instance;

    //board Setup
    private GameObject[,] tiles;
    public GameObject tile;
    
    private int xSize, ySize;
    public int GameXSize, GameYSize;
    private Vector2 TileScale = Vector2.zero;
    Vector2 tileSize;

    private int NumOptions;
    private List<string> Options;
    string Letters = "ABCDEFG";

    


    //Buffer
    private int BufferSize;
    private List<string> BufferChoices;
    [SerializeField]
    BufferScript Buffer;

    //Goal
    private List<string> AnswerKey;
    [SerializeField]
    private TargetSequenceScript TargetSequencePanel;
   

    private int NumAnswerComponents;

    //HighlightParts
    [SerializeField]
    private GameObject HorizontalBox;
    [SerializeField]
    private GameObject VerticalBox;
    private int CurrentTurn = 0;
    private int ActiveRow = 0;
    private int ActiveCol = 0;


    [SerializeField]
    private GameUIManager GameUI;
    [SerializeField]
    private TimerBehaviour Timer;

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
                newTile.transform.localScale = new Vector3(TileScale.x, TileScale.y, 1);
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
                    Tile.Initialize(Options[Random.Range(0, Options.Count)],x,ySize -1- y, this);
                }

            }
        }

        SetHorizontalBoxPosition(0);
        SetVerticalBoxPosition(-1);
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
                        Tile.Initialize(AnswerKey[i], selection, (prevRow < 0 ? 0 :ySize-1- prevRow), this);
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
                        Tile.Initialize(AnswerKey[i], (prevCol < 0 ? xSize - 1 : prevCol), ySize - 1- selection, this);
                        validChoice = true;
                        prevRow = selection;
                    }
                }
            }
        }
    }


    public void SetEasyDifficulty()
    {
        ClearBoard();
        xSize = ySize = 3;
        NumOptions = 4;
        BufferSize = 4;
        NumAnswerComponents = 2;
        Timer.SetTimer(10);
        GameUI.EasyDifficultyChosen();
        SetupNewDifficulty();
    }

    public void SetMediumDifficulty()
    {
        ClearBoard();
        xSize = ySize = 4;
        NumOptions = 6;
        BufferSize = 5;
        NumAnswerComponents = 4;
        Timer.SetTimer(15);
        GameUI.MediumDifficultyChosen();

        SetupNewDifficulty();
    }
    public void SetHardDifficulty()
    {
        ClearBoard();
        xSize = ySize = 5;

        NumOptions = 8;
        BufferSize = 6;

        NumAnswerComponents = 6;
        Timer.SetTimer(20);
        GameUI.HardDifficultyChosen();


        SetupNewDifficulty();
    }


    private void SetupNewDifficulty()
    {
        
        BufferChoices = new List<string>();
        Buffer.Initialize(BufferSize);
        PopulateOptions();
        TargetSequencePanel.Initialize(AnswerKey);

        SetBoardScale();
        CreateBoard(tileSize.x * TileScale.x, tileSize.y * TileScale.y);
    }

    /// <summary>
    /// needas to be called before create board
    /// </summary>
    private void SetBoardScale()
    {
        tileSize = tile.GetComponentInChildren<SpriteRenderer>().bounds.size;
        TileScale.x = (GameXSize / xSize) / tileSize.x;
        TileScale.y = (GameYSize / ySize) / tileSize.y;

        Vector2 panelSize = HorizontalBox.GetComponentInChildren<SpriteRenderer>().bounds.size;
        HorizontalBox.transform.localScale = new Vector3(GameXSize / panelSize.x, (GameYSize / panelSize.y) / ySize, 1); ;

        panelSize = VerticalBox.GetComponentInChildren<SpriteRenderer>().bounds.size;
        VerticalBox.transform.localScale = new Vector3(GameXSize / panelSize.x / xSize, (GameYSize / panelSize.y), 1); ;
    }

    private void SetHorizontalBoxPosition(int colIndex)
    {
        if (colIndex < 0)
        {
            HorizontalBox.SetActive(false);
            return;
        }

        HorizontalBox.SetActive(true);
        if (colIndex >= ySize)
        {
            colIndex = ySize - 1;
        }

        HorizontalBox.transform.position = tiles[0, (ySize - 1) - colIndex].transform.position;
        HorizontalBox.transform.position -= new Vector3(tileSize.x * TileScale.x / 2, 0,0);
    }

    private void SetVerticalBoxPosition(int rowIndex)
    {
        if (rowIndex < 0)
        {
            VerticalBox.SetActive(false);
            return;
        }

        VerticalBox.SetActive(true);
        if (rowIndex >= xSize)
        {
            rowIndex = xSize - 1;
        }

        VerticalBox.transform.position = tiles[rowIndex, ySize-1].transform.position;
        VerticalBox.transform.position += new Vector3(0, tileSize.y * TileScale.y / 2, 0);
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

        VerticalBox.transform.localScale = Vector3.one;
        HorizontalBox.transform.localScale = Vector3.one;

        CurrentTurn = 0;
        ActiveRow = 0;
        ActiveCol = 0;

        Buffer?.ClearBuffer();
        TargetSequencePanel?.ClearBuffer();
        GameUI.ResetUI();

    }

    public void TileSelected(string content, Vector2 gridLocation)
    {

        if (CurrentTurn % 2 == 0) //even and need to be horizontal selection
        {
            SetHorizontalBoxPosition(-1);
            SetVerticalBoxPosition((int)gridLocation.x);
            ActiveCol = (int)gridLocation.x;
        }
        else
        {
            SetHorizontalBoxPosition((int)gridLocation.y);
            SetVerticalBoxPosition(-1);
            ActiveRow = (int)gridLocation.y;
        }
        
        ChooseAnswer(content);
    }

    public void ChooseAnswer(string choice)
    {
        BufferChoices.Add(choice);
        Buffer.SetElementContent(CurrentTurn, choice);

        if (!CheckAnswer() && BufferChoices.Count == BufferSize)
        {
            GameLose();
            return;
        }

        CurrentTurn++;
    }

    public bool IsValidTileSelection(Vector2 gridLocation)
    {
        //if unable to make more moves
        if (CurrentTurn >= BufferSize) return false;


        if (CurrentTurn % 2 == 0) //even and need to be horizontal
        {
            if (gridLocation.y == ActiveRow)
            {
                return true;
            }
        }
        else
        {
            if (gridLocation.x == ActiveCol)
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckAnswer()
    {
        if (!BufferChoices.Contains(AnswerKey[0])) return false;

        int StartingIndex = -1;
        bool answerFound = true;
        bool continueChecking = true;
        int prevIndex = -1;
        while (continueChecking)
        {
            answerFound = true;
            for (int i = StartingIndex + 1; i < BufferChoices.Count; i++)
            {
                if (BufferChoices[i] == AnswerKey[0])
                {
                    StartingIndex = i;
                    break;
                }
            }
            //not enough answers in the buffer
            if ((BufferChoices.Count - 1) - StartingIndex < AnswerKey.Count - 1 
                || prevIndex == StartingIndex) // no new index was found
            {
                continueChecking = false;
                answerFound = false;
            }
            if (continueChecking)
            {
                for (int i = 0; i < AnswerKey.Count; i++)
                {
                    //a mismatch
                    if (!(BufferChoices[StartingIndex + i] == AnswerKey[i]))
                    {
                        answerFound = false;
                    }
                }
                if (answerFound)
                {
                    continueChecking = false;
                }
                prevIndex = StartingIndex;
            }
            
        }
        

        if (answerFound)
        {
            Debug.Log("Answer Matches");
            //disable turns
            
            //remove any guiding panel
            SetVerticalBoxPosition(-1);
            SetHorizontalBoxPosition(-1);

            GameWin();
            return true;
        }
        return false;
    }

    private void GameWin()
    {
        CurrentTurn = 99;
        GameUI.GameWin();
        Timer.StopTimer();
    }   
    
    public void GameLose()
    {
        CurrentTurn = 99;
        GameUI.GameOver();
        Timer.StopTimer();

    }

}
