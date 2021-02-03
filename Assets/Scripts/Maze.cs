using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    public enum TileState { Wall, Path, Fruit }
    public TileState[,] states;

    public float tileScale = 5;
    public GameObject tilePrefab;

    public Tile[,] grid;

    Stack<Pair> stack;
    bool[,] visited;

    public Vector3 offset;

    public bool IsInit { get; private set; }

    public static Maze active;

    int fruits;

    void Awake()
    {
        active = this;
    }

    public void Init(int width, int height)
    {
        Width = width;
        Height = height;
        //offset = new Vector3(-Screen.width / 2, -Screen.height / 2);
        StartCoroutine(InitStates());
    }
    
    void InitGrid()
    {
        grid = new Tile[Width, Height];
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                (grid[x, y] = Instantiate(tilePrefab, transform).GetComponent<Tile>()).Init(states[x, y], x, y, offset, tileScale);
    }

    void InitFruits()
    {
        var possibles = new List<Pair>();
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                if (states[x, y] == TileState.Path) possibles.Add(new Pair(x, y));
        for(int i = 0; i < MazeGenerator.instance.fruits; i++)
        {
            int ind = Random.Range(0, possibles.Count);
            var spot = possibles[ind];
            possibles.RemoveAt(ind);
            states[spot.x, spot.y] = TileState.Fruit;
            grid[spot.x, spot.y].SetState(TileState.Fruit);
        }
        fruits = MazeGenerator.instance.fruits;
    }

    public void EatFruit()
    {
        fruits--;
        if (fruits == 0) GameManager.instance.YouWin();
    }

    void DrawMaze()
    {
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                grid[x, y].SetState(states[x, y]);
    }

    IEnumerator InitStates()
    {
        states = new TileState[Width, Height];
        visited = new bool[Width, Height];
        for (int x = 0; x < Width; x += 2)
            for (int y = 0; y < Height; y += 2)
                states[x, y] = TileState.Path;
        stack = new Stack<Pair>();
        stack.Push(new Pair(0, 0));
        visited[0, 0] = true;
        InitGrid();
        yield return new WaitForSeconds(1);
        do
        {
            var spot = stack.Peek();
            var possibles = new List<Pair>();
            for(int x = spot.x - 2; x <= spot.x + 2; x += 2)
                for(int y = spot.y - 2; y <= spot.y + 2; y += 2)
                {
                    if (x < 0 || x >= Width || y < 0 || y >= Height) continue;
                    if (visited[x, y]) continue;
                    if((x == spot.x) ^ (y == spot.y)) possibles.Add(new Pair(x, y));
                }
            if (possibles.Count == 0) stack.Pop();
            else
            {
                var chosen = possibles[Random.Range(0, possibles.Count)];
                stack.Push(chosen);
                var pathway = new Pair((spot.x + chosen.x) / 2, (spot.y + chosen.y) / 2);
                states[pathway.x, pathway.y] = TileState.Path;
                visited[chosen.x, chosen.y] = true;
                DrawMaze();
                yield return new WaitForSeconds(0.02f);
            }
        } while (stack.Count > 0);
        InitFruits();
        PlayerManager.instance.transform.position = grid[0, 0].transform.position;
        IsInit = true;
    }

    public void MakeNew()
    {
        MazeGenerator.instance.GenerateMaze();
    }

    struct Pair
    {
        public int x, y;

        public Pair(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}