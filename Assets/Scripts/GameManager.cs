using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject winUI;

    public Sprite wallIcon, pathwayIcon, fruitIcon;

    public Dictionary<Maze.TileState, Sprite> tileIcons;

    void Awake()
    {
        instance = this;
        tileIcons = new Dictionary<Maze.TileState, Sprite>
        {
            { Maze.TileState.Wall, wallIcon },
            { Maze.TileState.Path, pathwayIcon },
            { Maze.TileState.Fruit, fruitIcon }
        };
    }

    public void YouWin()
    {
        Destroy(Maze.active.gameObject);
        winUI.SetActive(true);
    }
}
