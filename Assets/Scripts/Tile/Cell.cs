using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Tile tile { get; set; }
    public int GetValueInTile()
    {
        return tile == null ? 0 : tile.number;
    }
}
