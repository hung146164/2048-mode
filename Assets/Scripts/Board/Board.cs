using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Board:MonoBehaviour
{
    protected const int SIZE_TILE = 1;

    [Header("Board Configure")]
    [SerializeField] protected Transform Grid;
    [SerializeField] protected int sizeGame = 4;
    [SerializeField] protected float tileOffSet = 0.1f;
    [SerializeField] protected Transform backgroundSize;
    [SerializeField] private TileState[] _tileData;
    [Header("Prefabs")]
    [SerializeField] protected Cell cellPrefab;
    [SerializeField] protected Tile tilePrefab;

    protected Dictionary<int, TileState> tileStates=new Dictionary<int, TileState>();
    protected Dictionary<(int, int), Cell> shapes= new Dictionary<(int, int), Cell>();

    protected bool isLose = false;
    protected bool isMoving = false;


    [SerializeField] protected string saveKey;
    private void OnEnable()
    {
        LoadGame(sizeGame);
    }
    private void OnDisable()
    {
        SaveGame(sizeGame);
    }
    protected void Awake()
    {
        ConvertDataToDictionary();
        SetBackground();
    }
    private void ConvertDataToDictionary()
    {
        int start = 2;
        for (int i = 0; i < _tileData.Length; i++)
        {
            tileStates[start] = _tileData[i];
            start *= 2;
        }
    }
    
    protected TileState GetTileState(int id)
    {
        return tileStates[id];
    }
    protected bool CreateRandomTile()
    {
        (int, int) unoccupiedPositionpos = GetRandomUnoccupiedPosition();
        if (unoccupiedPositionpos == (-1, -1)) return false;
        int numState = Random.Range(0, 10) < 9 ? 2 : 4;

        CreateTileAt(unoccupiedPositionpos, numState);
        return true;
    }
    protected void CreateTileAt((int, int) pos, int value)
    {
        Cell cell = shapes[pos];
        if (cell == null) return;
        //pooling
        Tile tile= Instantiate(tilePrefab, cell.transform);
        tile.ChangeState(tileStates[value]);
        cell.tile = tile;
        tile.transform.localPosition = Vector3.zero;
    }
    protected (int, int) GetRandomUnoccupiedPosition()
    {
        List<(int, int)> tileValid = new List<(int, int)>();
        tileValid = shapes.Where(c => c.Value.tile == null).Select(c => c.Key).ToList();

        return tileValid.Count == 0 ? (-1, -1) : tileValid[Random.Range(0, tileValid.Count)];
    }

    protected abstract void SetBackground();

    //GameSave
    protected void LoadGame(int sizeGame)
    {
        GenerationBoard(sizeGame);
        int presize = PlayerPrefs.GetInt(SaveKey.Square + "sizeGame");
        if (presize == sizeGame)
        {
            LoadData(saveKey, sizeGame);
        }
        else
        {
            CreateRandomTile();
            CreateRandomTile();
        }
    }
    protected void SaveGame(int sizeGame)
    {
        if(isLose)
        {
            PlayerPrefs.SetInt(saveKey + "sizeGame",-1);
        }
        else
        {
            SaveData(saveKey, sizeGame);
        }
    }
    protected abstract void GenerationBoard(int sizeGame);
    protected abstract IEnumerator Move(Vector2Int direction);
    protected abstract bool LoseCheck();
    protected void LoadData(string keySave, int sizeGame)
    {
        foreach (var i in shapes)
        {
            int tileValue = PlayerPrefs.GetInt(keySave + $"{i.Key}",0);
            if (tileValue != 0)
            {
                CreateTileAt(i.Key, tileValue);
            }
        }
        
    }

    protected void SaveData(string keySave, int sizeGame)
    {
        PlayerPrefs.SetInt(keySave + "sizeGame", sizeGame);
        foreach(var i in shapes) 
        {
            PlayerPrefs.SetInt(keySave + $"{i.Key}", i.Value.GetValueInTile());
        }
    }

}