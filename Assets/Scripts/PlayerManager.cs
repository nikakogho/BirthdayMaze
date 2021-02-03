using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public int x, y;

    public static PlayerManager instance;

    void Awake()
    {
        instance = this;
    }

    MazeGenerator gen;

    void Start()
    {
        gen = MazeGenerator.instance;
    }

    void Update()
    {
        if (!gen.IsMazeInit) return;
        
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) MoveTowards(x + 1, y);
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) MoveTowards(x - 1, y);
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) MoveTowards(x, y + 1);
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) MoveTowards(x, y - 1);
    }

    void MoveTowards(int X, int Y)
    {
        if (X < 0 || Y < 0 || X >= gen.width || Y >= gen.height) return;
        var maze = Maze.active;
        if (maze.states[X, Y] == Maze.TileState.Wall) return;
        x = X;
        y = Y;
        transform.position = maze.grid[x, y].transform.position;
        if(maze.states[x, y] == Maze.TileState.Fruit)
        {
            maze.grid[x, y].SetState(Maze.TileState.Path);
            maze.EatFruit();
        }
    }
}
