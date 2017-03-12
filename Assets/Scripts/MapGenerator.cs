using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    private float mapWidth;
    private float mapHeight;

    private int nbSquareHeight;
    private int nbSquareWidth;

    private float squareOffset;

    private Dictionary<Point, CaseType> map;

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
        player.transform.position = new Vector3(0, squareSize / 2, 0);

        mapWidth = this.transform.localScale.x * 10;
        mapHeight = this.transform.localScale.z * 10;

        nbSquareWidth = Convert.ToInt32(mapWidth / squareSize);
        nbSquareHeight = Convert.ToInt32(mapHeight / squareSize);

        // Init map to default state
        InitializeMap();

        GenerateMapGameObjects();
	}

    void InitializeMap()
    {
        map = new Dictionary<Point, CaseType>();
        for(int y=0; y<nbSquareHeight; y++)
        {
            for(int x=0; x<nbSquareWidth; x++)
            {
                map[new Point { x = x, y = y }] = CaseType.Empty;
            }
        }

        // Spawn the players (one on each side of the map symmetrically) [ p1 | p2 ]
        Point firstPlayer = GetRandomEmptySpace();
        map[firstPlayer] = CaseType.StartingPlayer;


        // Generate rocks randomly on the map
        for (int i = 0; i < nbRocks; i++)
        {
            map[GetRandomEmptySpace()] = CaseType.UnbreakableRock;
        }
    }

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
                case CaseType.StartingPlayer:
                    MoveAndInstantiate(player, square.Key);
                    break;
                case CaseType.Empty:
                    break;
            }
        }
    }

    enum CaseType
    {
        Empty,
        UnbreakableRock,
        Rock,
        StartingPlayer
    }

    struct Point
    {
        public int x, y;
    }


    // Update is called once per frame
    void Update()
    {

    }


    private Vector3 GetPositionFromGridCoord(int x, int y, float z)
    {
        var position = new Vector3((-1 * mapWidth/2)+squareOffset, z, (-1*mapHeight/2)+squareOffset);

        position.x += x * squareSize + squareOffset;
        position.z += y * squareSize + squareOffset;

        return position;
    }

    private void MoveAndInstantiate(GameObject obj, Point pt)
    {
        obj.transform.position = GetPositionFromGridCoord(pt.x, pt.y, obj.transform.position.y);
        Instantiate(obj);
    }
    
    private Point GetRandomEmptySpace()
    {
        var emptySpaces = map.Where(p => p.Value == CaseType.Empty).ToList();
        return emptySpaces[UnityEngine.Random.Range(0, emptySpaces.Count())].Key;
    }
}
