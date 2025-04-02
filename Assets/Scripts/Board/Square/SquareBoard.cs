using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

public class SquareBoard : Board
{
    [SerializeField] private float backgroundOffset;
    private void Update()
    {
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.W)) StartCoroutine(Move(Vector2Int.up));
        else if (Input.GetKeyDown(KeyCode.S)) StartCoroutine(Move(Vector2Int.down));
        else if (Input.GetKeyDown(KeyCode.A)) StartCoroutine(Move(Vector2Int.left));
        else if (Input.GetKeyDown(KeyCode.D)) StartCoroutine(Move(Vector2Int.right));
    }
    protected override void GenerationBoard(int sizeGame)
    {
        int Off = sizeGame / 2;
        for (int x = 0; x < sizeGame; x++)
        {
            for (int y = 0; y < sizeGame; y++)
            {
                Cell cell = Instantiate(cellPrefab, Grid);
                shapes[(x, y)] = cell;
                cell.transform.position = new Vector2((x- Off) *(1+ tileOffSet), (y - Off) * (1 + tileOffSet));
            }
        }
    }
    
    protected override IEnumerator Move(Vector2Int direction)
    {
        isMoving = true;
        bool moved = false;
        List<IEnumerator> moveCoroutines = new List<IEnumerator>();

        int start = (direction.x == 1 || direction.y == 1) ? sizeGame - 1 : 0;
        int end = (start == 0) ? sizeGame - 1 : 0;
        int step = (start == 0) ? 1 : -1;

        for (int x = start; (step > 0) ? x <= end : x >= end; x += step)
        {
            for (int y = start; (step > 0) ? y <= end : y >= end; y += step)
            {
                if (shapes.ContainsKey((x, y)) && shapes[(x, y)].tile != null)
                {
                    int currX = x + direction.x;
                    int currY = y + direction.y;

                    Tile currentTile = shapes[(x, y)].tile;
                    shapes[(x, y)].tile = null;
                    while (shapes.ContainsKey((currX, currY)) && shapes[(currX, currY)].tile == null)
                    {
                        currX += direction.x;
                        currY += direction.y;
                        moved = true;
                    }

                    bool isMerge = false;
                    if (shapes.ContainsKey((currX, currY)) && shapes[(currX, currY)].tile != null)
                    {
                        if (shapes[(currX, currY)].tile.number == currentTile.number)
                        {
                            shapes[(currX, currY)].tile.ChangeState(tileStates[currentTile.number * 2]);
                            isMerge = true;
                            moved = true;
                        }
                    }

                    currX -= direction.x;
                    currY -= direction.y;
                    currentTile.transform.SetParent(shapes[(currX, currY)].transform);
                    MoveTile(currentTile, isMerge,currX,currY);
                    
                    
                }
            }
        }

        yield return new WaitForSeconds(0.1f); 

        if (moved)
        {
            if (!CreateRandomTile() && LoseCheck())
            {
                UIManager.Instance.Lose();
            }
        }

        isMoving = false;
    }
    IEnumerator MoveTile(Tile currentTile, bool isMerge, int currX, int currY)
    {
        
        yield return StartCoroutine(currentTile.ChangeLocalPosition(Vector2.zero));

        if (isMerge)
        {
            Destroy(currentTile);
        }
        else
        {
            shapes[(currX, currY)].tile = currentTile;
        }
    }


    protected override bool LoseCheck()
    {
        Debug.Log("LoseCheck");
        if (sizeGame < 2) return true;
        for(int i=0; i<sizeGame-1;i++)
        {
            for (int j = 0; j < sizeGame-1;j++)
            {
                int val=shapes[(i, j)].GetValueInTile();

                if(val== shapes[(i+1, j)].GetValueInTile())
                {
                    return false;
                }
                if (val == shapes[(i, j+1)].GetValueInTile())
                {
                    return false;
                }
            }
        }
        if(shapes[(sizeGame-2, sizeGame-1)].GetValueInTile() == shapes[(sizeGame-1,sizeGame-1)].GetValueInTile())
        {
            return false;
        }
        if (shapes[(sizeGame - 1, sizeGame - 2)].GetValueInTile() == shapes[(sizeGame - 1, sizeGame - 1)].GetValueInTile())
        {
            return false;
        }
        return true;
    }
    //GameSave

    protected override void SetBackground()
    {
        backgroundSize.localScale = new Vector2((sizeGame + 1) + tileOffSet+backgroundOffset, (sizeGame + 1) + tileOffSet+backgroundOffset);
    }

}
