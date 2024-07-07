using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class RadarController : MonoBehaviour
{
    static (int X, int Y)[] Offsets = {(-1, 0), (1, 0), (0, -1), (0, 1)};
    public int Width = 6;
    public int Height = 6;
    public Room[,] Rooms;
    public EnemyWaveData[] BossWave;
    public EnemyWaveData[] StartWave;
    public Room StartRoom;
    public Room BossRoom;

    [Button("Generate Rooms")]
    public void GenerateRooms()
    {
        InitRooms();        
        BossRoom = InitBossRoom();
        StartRoom = InitStartRoom();
        GeneratePathToBossRoom(StartRoom, new HashSet<(int, int)>() { (StartRoom.X, StartRoom.Y) });
        GeneratePathToBossRoom(StartRoom, new HashSet<(int, int)>() { (StartRoom.X, StartRoom.Y) });
    }
    private void InitRooms()
    {
        Rooms = new Room[Width, Height];
        for (int row = 0; row < Width; row++)
        {
            for (int col = 0; col < Height; col++)
            {
                Rooms[col, row] = new Room() { Y = row, X = col };
            }
        }
    }
    
    private Room InitBossRoom()
    {
        int bossRow = Random.Range(0, Height);
        Room bossRoom = Rooms[Width-1, bossRow];
        bossRoom.IsBossRoom = true;
        bossRoom.Wave = BossWave[Random.Range(0, BossWave.Length)];
        return bossRoom;
    }

    private Room InitStartRoom()
    {
        int startRow = Random.Range(0, Width);
        Room startRoom = Rooms[0, startRow];
        startRoom.IsStartRoom = true;
        startRoom.Wave = StartWave[Random.Range(0, StartWave.Length)];
        return startRoom;
    }

    private void GeneratePathToBossRoom(Room currentRoom, HashSet<(int, int)> seenRooms)
    {
        var possibleExits = Offsets
            .Select(pair => (pair.X + currentRoom.X, pair.Y + currentRoom.Y))
            .Where(pair => pair.Item1 >= 0 && pair.Item1 < Width && pair.Item2 >= 0 && pair.Item2 < Height)
            .Where(pair => !seenRooms.Contains(pair))            
            .ToArray();
        if (possibleExits.Length == 0) 
        { 
            GeneratePathToBossRoom(StartRoom,  new HashSet<(int, int)>() { (StartRoom.X, StartRoom.Y) }); 
            return;
        }

        (int x, int y) = possibleExits[Random.Range(0, possibleExits.Length)];
        seenRooms.Add((x, y));
        Room nextRoom = Rooms[x, y];
        ConnectRooms(currentRoom, nextRoom, 0);
        if (nextRoom == BossRoom) 
        { 
            return; 
        }
        GeneratePathToBossRoom(nextRoom, seenRooms);
    }

    private void ConnectRooms(Room First, Room Second, int depth)
    {
        if (depth > 1) { Debug.Log($"Depth == {depth} -- something went wrong."); }
        if (First == Second) 
        { 
            return;
        }
        int xOff = First.X - Second.X;
        int yOff = First.Y - Second.Y;
        // Second is to right of first
        if (xOff < 0)
        {
            Rooms[First.X, First.Y].Right = Rooms[First.X + 1, First.Y];
            Rooms[First.X + 1, First.Y].Left = Rooms[First.X, First.Y];
            ConnectRooms(Rooms[First.X + 1, First.Y], Second, depth+1);
        }
        // Second is to left of first
        else if (xOff > 0)
        {
            Rooms[First.X, First.Y].Left = Rooms[First.X - 1, First.Y];
            Rooms[First.X - 1, First.Y].Right = Rooms[First.X, First.Y];
            ConnectRooms(Rooms[First.X - 1, First.Y], Second, depth+1);
        }
        // Second is below first
        else if (yOff < 0)
        {
            Rooms[First.X, First.Y].Down = Rooms[First.X, First.Y + 1];
            Rooms[First.X, First.Y + 1].Up = Rooms[First.X, First.Y].Down;
            ConnectRooms(Rooms[First.X, First.Y + 1], Second, depth+1);
        }
        // Second is above first
        else if (yOff > 0)
        {
            Rooms[First.X, First.Y].Up = Rooms[First.X, First.Y - 1];
            Rooms[First.X, First.Y - 1].Down = Rooms[First.X, First.Y];
            ConnectRooms(Rooms[First.X, First.Y - 1], Second, depth+1);
        }
    }

}

public class Room
{
    public Room Up;
    public Room Right;
    public Room Down;
    public Room Left;
    public EnemyWaveData Wave;
    public int Y;
    public int X;
    public bool IsBossRoom;
    public bool IsStartRoom;

    public override string ToString()
    {
        return $"Room ({X}, {Y})";
    }
}
