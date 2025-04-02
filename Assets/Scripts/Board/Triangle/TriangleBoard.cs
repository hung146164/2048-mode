using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class TriangleBoard : Board
{
    protected override void GenerationBoard(int sizeGame)
    {
        int numberTri = sizeGame * 2 - 1;
        float yOffset = 0.35f;
        for (int y=0; y<sizeGame; y++)
        {
            for (int x = 0; x < numberTri; x++)
            {
                Cell cell = Instantiate(cellPrefab);
                shapes[(x, y)] = cell;
                if (x % 2 == 1)
                {
                    cell.transform.rotation = Quaternion.Euler(0, 0, 180);
                    cell.transform.position = new Vector3(y*0.55f + x * 0.55f, yOffset + y, 0);
                }
                else cell.transform.position = new Vector3(y * 0.55f + x * 0.55f, y, 0);
            }
            numberTri -= 2;
        }
    }
    private void Update()
    {
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.A)) StartCoroutine(Move(new Vector2Int(-1, 0)));
        else if (Input.GetKeyDown(KeyCode.D)) StartCoroutine(Move(new Vector2Int(1, 0)));
        else if (Input.GetKeyDown(KeyCode.Q)) StartCoroutine(Move(new Vector2Int(-1, 1)));
        else if (Input.GetKeyDown(KeyCode.E)) StartCoroutine(Move(new Vector2Int(1, 1)));
        else if (Input.GetKeyDown(KeyCode.Z)) StartCoroutine(Move(new Vector2Int(-1, -1)));
        else if (Input.GetKeyDown(KeyCode.C)) StartCoroutine(Move(new Vector2Int(1, -1)));
    }
    protected override IEnumerator Move(Vector2Int direction)
    {
        isMoving = true;
        bool moved = false;
        List<IEnumerator> moveCoroutines = new List<IEnumerator>();

        Vector2Int evenX=Vector2Int.zero;
        Vector2Int oddX=Vector2Int.zero;
        int startX=0, endX = 0;
        int startY=0, endY=0;
        int stepX=0,stepY=0;
        if (new Vector2Int(-1, 0) == direction)
        {
            evenX = direction;
            oddX = direction;
            startX= 0;
            startY = 0;
            endX = 1;
            endY = sizeGame;
            stepX = 0;
            stepY = 1;
        }
        else if(new Vector2Int(1, 0) == direction)
        {
            evenX = new Vector2Int(1, 0);
            oddX = new Vector2Int(-1, 1);
            startX = 0;
            startY = sizeGame - 1;
            endX = 2 * sizeGame;
            endY = -1;
            stepX = 2;
            stepY = -1;
        }
        else if (new Vector2Int(-1, 1)==direction)
        {
            evenX = new Vector2Int(-1,0);
            oddX = new Vector2Int(-1,1);
            startX = 0;
            startY = 0;
            endX = 1;
            endY = sizeGame;
            stepX = 0;
            stepY = 1;
        }
        else if(new Vector2Int(1, 1) == direction)
        {
            evenX = new Vector2Int(1, 0);
            oddX = new Vector2Int(-1, 1);
            startX = 0;
            startY = sizeGame-1;
            endX = 2 * sizeGame;
            endY = -1;
            stepX = 2;
            stepY = -1;
        }
        else if((new Vector2Int(-1, -1)==direction) || (new Vector2Int(1, -1)==direction))
        {
            evenX = new Vector2Int(1, -1);
            oddX = new Vector2Int(direction.x, 0);
            startX = 0;
            startY = 0;
            endX = 2*sizeGame;
            endY = 1;
            stepX = 2;
            stepY = 0;
        }
        //Debug.Log($"evenX: {evenX} oddX: {oddX} startX: {startX} startY: {startY} evenX: {evenX} endY: {endY} stepX: {stepX} stepY: {stepY} ");
        while(startX!=endX && startY!=endY)
        {
            Debug.Log("vv");

            int x =startX;
            int y=startY;
            while(shapes.ContainsKey((x,y)))
            {
                Vector2Int dir=x%2==0?-oddX: -evenX;
                if (shapes[(x,y)].tile!=null)
                {
                    Tile currentTile = shapes[(x, y)].tile;
                    shapes[(x, y)].tile = null;

                    Vector2Int dir2 = x % 2 == 0 ? evenX : oddX;
                    int clonex = x+dir2.x;
                    int cloney = y+dir2.y;
                    while (shapes.ContainsKey((clonex, cloney)) && shapes[(clonex, cloney)].tile == null)
                    {
                        dir2= clonex%2==0?evenX:oddX;
                        clonex += dir2.x;
                        cloney += dir2.y;
                        moved = true;
                    }
                    bool isMerge = false;
                    if (shapes.ContainsKey((clonex, cloney)) && shapes[(clonex, cloney)].tile != null)
                    {
                        if (shapes[(clonex, cloney)].tile.number == currentTile.number)
                        {
                            shapes[(clonex, cloney)].tile.ChangeState(tileStates[currentTile.number * 2]);
                            Destroy(currentTile.gameObject);
                            isMerge = true;
                            moved = true;
                        }
                    }
                    if (!isMerge)
                    {
                        clonex -= dir2.x;
                        cloney -= dir2.y;
                        shapes[(clonex, cloney)].tile = currentTile;
                        currentTile.transform.SetParent(shapes[(clonex, cloney)].transform);

                        // Chạy coroutine di chuyển
                        IEnumerator moveCoroutine = currentTile.ChangeLocalPosition(Vector2.zero);
                        moveCoroutines.Add(moveCoroutine);
                        StartCoroutine(moveCoroutine);
                    }
                }
                x += dir.x;
                y += dir.y;
            }
            startX += stepX;
            startY+= stepY;
        }
        foreach (var coroutine in moveCoroutines)
        {
            yield return coroutine;
        }

        yield return new WaitForSeconds(0.1f); // Chờ thêm một chút để ổn định

        if (moved)
        {
            if (!CreateRandomTile() && LoseCheck())
            {
                UIManager.Instance.Lose();
            }
        }

        isMoving = false;
    }
    protected override bool LoseCheck()
    {
        throw new System.NotImplementedException();
    }

    protected override void SetBackground()
    {
        throw new System.NotImplementedException();
    }
}
