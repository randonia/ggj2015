using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the grid
/// </summary>
public class WorldController : MonoBehaviour
{
    #region Directions

    /// <summary>
    /// Make sure this is sync'd with WorldController
    /// </summary>

    public const int NORTH = 0;
    public const int SOUTH = 1;
    public const int EAST = 2;
    public const int WEST = 3;

    #endregion Directions

    public const float GLOBAL_Z = 1.0f;

    public GameObject PREFAB_TILE1;

    public GameObject TileContainer;

    public Vector2 worldSize;

    public Sprite[] grass_basics;

    /// <summary>
    /// THE GRID. X,Y based. Make sure to round.
    /// </summary>
    public static Dictionary<int, Dictionary<int, GameObject>> THE_GRID;

    // Use this for initialization
    private void Start()
    {
        THE_GRID = new Dictionary<int, Dictionary<int, GameObject>>();
        foreach (GameObject tile in GameObject.FindGameObjectsWithTag("Terrain"))
        {
            int x = (int)tile.transform.position.x;
            int y = (int)tile.transform.position.y;
            tile.transform.position.Set(x, y, 0.5f);
            if (!THE_GRID.ContainsKey(x))
            {
                THE_GRID.Add(x, new Dictionary<int, GameObject>());
            }
            if (THE_GRID[x].ContainsKey(y))
            {
                Debug.Log("Duplicate tile detected:" + x + "," + y);
                Debug.DrawLine(tile.transform.position, Vector3.forward, Color.red, 15.0f);
            }
            if (tile.name.Equals("grass_0000"))
            {
                // Pick a random grass
                Debug.Log("Random grass!");
            }
            THE_GRID[x].Add(y, tile);
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public static bool CanMove(int tileX, int tileY, int dir)
    {
        // Simple bounds checking
        if (!THE_GRID.ContainsKey(tileX))
        {
            return false;
        }
        if (!THE_GRID[tileX].ContainsKey(tileY))
        {
            return false;
        }
        string currTileName = THE_GRID[tileX][tileY].name;
        int namePadding = currTileName.IndexOf("_") + 1;
        switch (dir)
        {
            case NORTH:
                if (!THE_GRID[tileX].ContainsKey(tileY + 1))
                {
                    return false;
                }
                break;
            case SOUTH:
                if (!THE_GRID[tileX].ContainsKey(tileY - 1))
                {
                    return false;
                }
                break;
            case EAST:
                if (!THE_GRID.ContainsKey(tileX + 1) || !THE_GRID[tileX + 1].ContainsKey(tileY))
                {
                    return false;
                }
                break;
            case WEST:
                if (!THE_GRID.ContainsKey(tileX - 1) || !THE_GRID[tileX - 1].ContainsKey(tileY))
                {
                    return false;
                }
                break;
            default:
                return false;
        }
        if (currTileName[namePadding + dir].Equals('1'))
        {
            return false;
        }
        return true;
    }
}