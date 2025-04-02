using UnityEngine;

public enum PoolType
{
    SquareCell,
    TriangleCell,
    HexagonCell
}
public enum GameMode
{
    Square,
    Triangle,
    Hexagon,
}
public enum GameLevel
{
    Level3=3,
    Level4=4,
    Level5=5,
    Level6=6,
    Level7=7,
    Level8=8,
}
public static class SaveKey
{
    public const string Square = "square";
    public const string Triangle = "triangle";
    public const string Hexagon = "hexagon";
}
