using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controls the grid
/// </summary>
public class WorldController : MonoBehaviour {
    public const float GLOBAL_Z = 1.0f;

    public GameObject PREFAB_TILE1;

    public GameObject TileContainer;

    public Vector2 worldSize;

    /// <summary>
    /// THE GRID. X,Y based. Make sure to round.
    /// </summary>
    public Dictionary<int, Dictionary<int, GameObject>> THE_GRID;

	// Use this for initialization
	void Start () {
        
	    // Instantiate the world according to worldSize
        for (int x = (int)-worldSize.x; x < worldSize.x; ++x)
        {
            for (int y = (int)-worldSize.y; y < worldSize.y; ++y)
            {
                GameObject newTile = GameObject.Instantiate(PREFAB_TILE1) as GameObject;
                newTile.transform.parent = TileContainer.transform;
                newTile.transform.position = new Vector3(x, y, GLOBAL_Z);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
