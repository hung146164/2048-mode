using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Transform _lobby;
    [Header("Main Game")]
    [SerializeField] private TriangleBoard _triangleBoard;
    [SerializeField] private SquareBoard _squareBoard;
    [SerializeField] private HexagonBoard _hexagonBoard;

    [SerializeField] private GameMode _gameMode;
    [SerializeField] private GameLevel _gameLevel;
    //Configure
    public void ChangeGameMode(GameMode gameMode)
    {
        this._gameMode = gameMode;
    }
    public void ChangeGameLevel(GameLevel gameLevel)
    {
        this._gameLevel= gameLevel;  
    }
    public void StartGame()
    {
        _lobby.gameObject.SetActive(false);
        switch (_gameMode)
        {
            case GameMode.Square:
                _squareBoard.gameObject.SetActive(true);
                break;
            case GameMode.Triangle:
                _triangleBoard.gameObject.SetActive(true);
                break;
            case GameMode.Hexagon:
                _hexagonBoard.gameObject.SetActive(true); 
                break;
        }

    }
    public void ExitGame()
    {
        _lobby.gameObject.SetActive(true);
        _squareBoard.gameObject.SetActive(false);
        _triangleBoard.gameObject.SetActive(false);
        _hexagonBoard.gameObject.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
