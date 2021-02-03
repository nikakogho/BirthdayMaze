using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {
    public static MazeGenerator instance;
    public Transform mazeParent;
    public GameObject mazePrefab;
    public int width, height;
    public int fruits = 5;

    public bool IsMazeInit { get { return maze != null && maze.IsInit; } }

    Maze maze;

    void Awake()
    {
        instance = this;
    }

    public void GenerateMaze()
    {
        if (maze != null) Destroy(maze.gameObject);
        var obj = Instantiate(mazePrefab, mazeParent);
        obj.name = "Maze";
        maze = obj.GetComponent<Maze>();
        maze.Init(width, height);
    }
}
