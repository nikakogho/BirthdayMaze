using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {
    public Image image;
    public Maze.TileState State { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }

    public void Init(Maze.TileState state, int x, int y, Vector3 offset, float scale)
    {
        X = x;
        Y = y;
        transform.localPosition = offset + new Vector3(x, y, 0) * scale;
        SetState(state);
    }

    public void SetState(Maze.TileState state)
    {
        State = state;
        image.sprite = GameManager.instance.tileIcons[state];
    }
}
