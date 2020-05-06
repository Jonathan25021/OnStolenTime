using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDungeonGeneratorScript : MonoBehaviour
{ 
    public int boardRows, boardColumns;
    public int minRoomSize, maxRoomSize;
    public GameObject[] floorTiles;
    public GameObject floorTile, floorTileN, floorTileNE, floorTileE, floorTileSE, floorTileS, floorTileSW, floorTileW, floorTileNW;
    private GameObject[,] boardPositionsFloor;
    public GameObject corridorTile;
    public GameObject wallTile;
    private bool playerSpawned;
    public GameObject[] enemies;
    public GameObject Torch;

    public class SubDungeon
    {
        public SubDungeon left, right;
        public Rect rect;
        public Rect room = new Rect(-1, -1, 0, 0); // i.e null
        public int debugId;
        public List<Rect> corridors = new List<Rect>();

        private static int debugCounter = 0;

        public SubDungeon(Rect mrect)
        {
            rect = mrect;
            debugId = debugCounter;
            debugCounter++;
        }

        public bool IAmLeaf()
        {
            return left == null && right == null;
        }

        public Rect GetRoom()
        {
            if (IAmLeaf())
            {
                return room;
            }
            if (left != null)
            {
                Rect lroom = left.GetRoom();
                if (lroom.x != -1)
                {
                    return lroom;
                }
            }
            if (right != null)
            {
                Rect rroom = right.GetRoom();
                if (rroom.x != -1)
                {
                    return rroom;
                }
            }

            // workaround non nullable structs
            return new Rect(-1, -1, 0, 0);
        }
        

        public void CreateRoom()
        {
            
            int roomWidth = (int)Random.Range(rect.width / 2, rect.width - 2);
            int roomHeight = (int)Random.Range(rect.height / 2, rect.height - 2);
            int roomX = (int)Random.Range(1, rect.width - roomWidth - 1);
            int roomY = (int)Random.Range(1, rect.height - roomHeight - 1);

            // room position will be absolute in the board, not relative to the sub-dungeon
            room = new Rect(rect.x + roomX, rect.y + roomY, roomWidth, roomHeight);
            
        }
        
    }
    

    public void DrawRooms(SubDungeon subDungeon)
    {
        if (subDungeon == null)
        {
            return;
        }
        for (int i = (int)subDungeon.room.x - 1; i <= subDungeon.room.xMax; i++)
        {
            for (int j = (int)subDungeon.room.y - 1; j <= subDungeon.room.yMax; j++)
            {
                if (i == (int)subDungeon.room.x - 1 || j == (int)subDungeon.room.y - 1
                    || i == subDungeon.room.xMax || j == subDungeon.room.yMax)
                {
                    setTileHard(wallTile, i, j);
                }
                else
                {
                    if (i == (int)subDungeon.room.x && j == (int)subDungeon.room.y)
                    {
                        setTileHard(floorTileSW, i, j);
                    }
                    else if (i == (int)subDungeon.room.x && j == subDungeon.room.yMax - 1)
                    {
                        setTileHard(floorTileNW, i, j);
                    }
                    else if (i == subDungeon.room.xMax - 1 && j == (int)subDungeon.room.y)
                    {
                        setTileHard(floorTileSE, i, j);
                    }
                    else if (i == subDungeon.room.xMax - 1 && j == subDungeon.room.yMax - 1)
                    {
                        setTileHard(floorTileNE, i, j);
                    }
                    else if (i == (int)subDungeon.room.x)
                    {
                        setTileHard(floorTileW, i, j);
                    }
                    else if (i == subDungeon.room.xMax - 1)
                    {
                        setTileHard(floorTileE, i, j);
                    }
                    else if (j == (int)subDungeon.room.y)
                    {
                        setTileHard(floorTileS, i, j);
                    }
                    else if (j == subDungeon.room.yMax - 1)
                    {
                        setTileHard(floorTileN, i, j);
                    }
                    else
                    {
                        setTileHard(floorTiles[Random.Range(0, floorTiles.Length - 1)], i, j);
                            
                    }
                    if (Random.Range(0,50) == 0)
                    {
                        setTileHard(wallTile, i, j);
                    }
                    if (Random.Range(0,20) == 0)
                    {
                        Instantiate(Torch, new Vector3(i,j,0f), Quaternion.identity);
                    }
                }
                    
            }
        }
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector2((subDungeon.room.x + subDungeon.room.xMax)/2, (subDungeon.room.y + subDungeon.room.yMax) / 2);
        GameObject.FindGameObjectWithTag("Enemy").transform.position = new Vector2(Random.Range(subDungeon.room.x, subDungeon.room.xMax), Random.Range(subDungeon.room.y, subDungeon.room.yMax));
        
        
    }

    // places a tile if there is no tile already in that place
    private void setTileSoft(GameObject tile, int i, int j)
    {
        if (boardPositionsFloor[i, j] == null)
        {
            GameObject instance = Instantiate(tile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
            instance.transform.SetParent(transform);
            boardPositionsFloor[i, j] = instance;
        }
    }

    // places a tile even if there is a tile already in that place
    private void setTileHard(GameObject tile, int i, int j)
    {
        if (boardPositionsFloor[i, j] != null)
        {
            Destroy(boardPositionsFloor[i, j]);
        }
        GameObject instance = Instantiate(tile, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
        instance.transform.SetParent(transform);
        boardPositionsFloor[i, j] = instance;
    }

    void Start()
    {
        playerSpawned = false;
        SubDungeon rootSubDungeon = new SubDungeon(new Rect(0, 0, boardRows, boardColumns));
        rootSubDungeon.CreateRoom();

        boardPositionsFloor = new GameObject[boardRows, boardColumns];
        DrawRooms(rootSubDungeon);
    }
}