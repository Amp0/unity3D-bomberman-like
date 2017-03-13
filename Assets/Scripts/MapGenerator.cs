using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    private static List<Color> playerColors = new List<Color>()
    {
        Color.blue,
        Color.red,
        Color.green,
        Color.yellow
    };

    private float mapWidth;
    private float mapHeight;

    private int nbSquareHeight;
    private int nbSquareWidth;

    private float squareOffset;

    private Dictionary<Point, CaseType> map;

    private int countPlayer;

    public int squareSize;
    public int nbRocks;

    public GameObject unbreakableRock;
    public GameObject rock;
    public GameObject player;

	// Use this for initialization
	void Start () {
        squareOffset = squareSize/4;

        rock.transform.localScale = new Vector3(squareSize, squareSize, squareSize);
        rock.transform.position = new Vector3(0, squareSize / 2, 0);

        unbreakableRock.transform.localScale = new Vector3(squareSize, squareSize, squareSize);
        unbreakableRock.transform.position = new Vector3(0, squareSize / 2, 0);

        player.transform.localScale = new Vector3(squareSize*0.7F, squareSize * 0.7F, squareSize * 0.7F);
        player.transform.position = new Vector3(0, squareSize*0.7F / 2, 0);

        mapWidth = this.transform.localScale.x * 10;
        mapHeight = this.transform.localScale.z * 10;

        nbSquareWidth = Convert.ToInt32(mapWidth / squareSize);
        nbSquareHeight = Convert.ToInt32(mapHeight / squareSize);

        countPlayer = 0;

        // Generate map
        InitializeMap();
        // Create it in game
        GenerateMapGameObjects();
	}

    /// <summary>
    /// Construct the logical map
    /// </summary>
    void InitializeMap()
    {
        map = new Dictionary<Point, CaseType>();
        for(int y=0; y<nbSquareHeight; y++)
        {
            for(int x=0; x<nbSquareWidth; x++)
            {
                map[new Point { x = x, y = y }] = (x%2==1&&y%2==1)?CaseType.UnbreakableRock:CaseType.Empty;
            }
        }

        // Spawn the players (one on each side of the map symmetrically) [ p1 | p2 ]
        Point firstPlayer = GetRandomEmptySpace();
        map[firstPlayer] = CaseType.Player;

        Point secondPlayer = new Point()
        {
            x = (nbSquareWidth-1) - firstPlayer.x,
            y = (nbSquareHeight-1) - firstPlayer.y
        };
        map[secondPlayer] = CaseType.Player;
        // Ensure the case around the player (verticaly and horizontaly are empty)
        ForceEmptyAround(firstPlayer);
        ForceEmptyAround(secondPlayer);




        // Generate rocks randomly on the map
        for (int i = 0; i < nbRocks; i++)
        {
            map[GetRandomEmptySpace()] = CaseType.Rock;
        }
    }

    /// <summary>
    /// Create the map in game objects
    /// </summary>
    void GenerateMapGameObjects()
    {
        foreach(var square in map)
        {
            switch (square.Value)
            {
                case CaseType.UnbreakableRock:
                    MoveAndInstantiate(unbreakableRock, square.Key);
                    break;
                case CaseType.Rock:
                    MoveAndInstantiate(rock, square.Key);
                    break;
                case CaseType.Player:
                    countPlayer++;
                    var playerCopy = MoveAndInstantiate(player, square.Key);
                    playerCopy.GetComponent<PlayerController>().playerId = countPlayer;
                    playerCopy.GetComponent<Renderer>().material.color = playerColors[countPlayer-1];
                    break;
                case CaseType.Empty:
                    break;
            }
        }
    }

    /// <summary>
    /// Init map square type
    /// </summary>
    enum CaseType
    {
        Empty,
        UnbreakableRock,
        Rock,
        Player,
        StayEmpty // Case that should stay empty on generation
    }

    // Simple point struct for coord managment
    struct Point
    {
        public int x, y;
    }


    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Returns the absolute position from a grid coordinate
    /// Z is the absolute height you want to set for the object (should be his scale / 2)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private Vector3 GetPositionFromGridCoord(int x, int y, float z)
    {
        var position = new Vector3((-1 * mapWidth/2)+squareOffset, z, (-1*mapHeight/2)+squareOffset);

        position.x += x * squareSize + squareOffset;
        position.z += y * squareSize + squareOffset;

        return position;
    }

    /// <summary>
    /// Instantiate a given object to a given grid position
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="pt"></param>
    private GameObject MoveAndInstantiate(GameObject obj, Point pt)
    {
        obj.transform.position = GetPositionFromGridCoord(pt.x, pt.y, obj.transform.position.y);
        return Instantiate(obj);
    }
    
    /// <summary>
    /// Returns an empty square position if available
    /// </summary>
    /// <returns></returns>
    private Point GetRandomEmptySpace()
    {
        var emptySpaces = map.Where(p => p.Value == CaseType.Empty).ToList();
        if(emptySpaces.Count > 0)
            return emptySpaces[UnityEngine.Random.Range(0, emptySpaces.Count())].Key;
        return new Point() { x=-100, y=-100 };
    }

    /// <summary>
    /// Ensures the squares around the given position are empty
    /// </summary>
    /// <param name="pt"></param>
    private void ForceEmptyAround(Point pt)
    {
        // Check up
        if (pt.y > 0)
            map[new Point() { x = pt.x, y = pt.y - 1 }] = CaseType.StayEmpty;
        if (pt.x > 0)
            map[new Point() { x = pt.x - 1, y = pt.y }] = CaseType.StayEmpty;
        if (pt.y < nbSquareHeight)
            map[new Point() { x = pt.x, y = pt.y + 1 }] = CaseType.StayEmpty;
        if (pt.x < nbSquareWidth)
            map[new Point() { x = pt.x + 1, y = pt.y }] = CaseType.StayEmpty;
    }
}
