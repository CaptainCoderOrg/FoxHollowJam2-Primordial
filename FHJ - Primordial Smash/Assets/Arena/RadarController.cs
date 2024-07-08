using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class RadarController : MonoBehaviour
{
    static (int X, int Y)[] Offsets = { (-1, 0), (1, 0), (0, -1), (0, 1) };
    public int Width = 6;
    public int Height = 6;
    public Room[,] Rooms;
    public MapCellController MapCellPrefab;
    public Transform CellTarget;
    public EnemyWaveData[] BossWave;
    public EnemyWaveData[] StartWave;
    public EnemyWaveData[] MiddleWaves;
    public Room StartRoom;
    public Room BossRoom;
    public GameObject RadarPanel;
    public Animator Animator;
    public bool IsHidden;
    private bool _up = false;
    private bool _down = false;
    private bool _left = false;
    private bool _right = false;

    public int CurrentX = 0;
    public int CurrentY = 0;
    public int CursorX = 0;
    public int CursorY = 0;
    public InfoPanelController InfoPanel;
    public MaterialsData[] Rewards;

    public MapCellController Selected => CellTarget.GetChild(CursorX + CursorY * Width).GetComponent<MapCellController>();
    public MapCellController Current => CellTarget.GetChild(CurrentX + CurrentY * Width).GetComponent<MapCellController>();
    public Room CurrentRoom => Current.RoomData;
    public Room NextRoom;
    public SpawnPoint ExitDirection;

    public void SetNextRoomRight() => SetNextRoom(SpawnPoint.East);
    public void SetNextRoomUp() => SetNextRoom(SpawnPoint.North);
    public void SetNextRoomLeft() => SetNextRoom(SpawnPoint.West);
    public void SetNextRoomDown() => SetNextRoom(SpawnPoint.South);
    public void SetNextRoom(SpawnPoint direction)
    {
        ExitDirection = direction.Opposite();
        NextRoom = direction switch
        {
            SpawnPoint.North => CurrentRoom.Up,
            SpawnPoint.East => CurrentRoom.Right,
            SpawnPoint.South => CurrentRoom.Down,
            SpawnPoint.West => CurrentRoom.Left,
            _ => throw new System.NotImplementedException($"Unknown directionL {direction}"),
        };
    }

    public void ClearNextRoom()
    {
        NextRoom = null;
    }

    public void Awake()
    {
        Animator = GetComponent<Animator>();
        InfoPanel = GetComponentInChildren<InfoPanelController>();
        GenerateRooms();
        IsHidden = true;
    }

    void Start()
    {
        StartCoroutine(InitSelected());
    }

    private IEnumerator InitSelected()
    {
        yield return null;
        Select(StartRoom.X, StartRoom.Y);
    }

    void Update()
    {
        if (Input.GetButtonDown("Radar"))
        {
            Toggle();
        }
        HandleDPad();
    }

    void HandleDPad()
    {
        if (IsHidden) { return; }
        int xChange = 0;
        int yChange = 0;
        float dpad_h = Input.GetAxis("DPad_h");
        float dpad_v = Input.GetAxis("DPad_v");
        if (dpad_h == 0) { _left = _right = false; }
        else if (dpad_h == 1 && !_right) { xChange = 1; _right = true; }
        else if (dpad_h == -1 && !_left) { xChange = -1; _left = true; }
        if (dpad_v == 0) { _up = _down = false; }
        else if (dpad_v == 1 && !_down) { yChange = -1; _down = true; }
        else if (dpad_v == -1 && !_up) { yChange = 1; _up = true; }


        if (xChange != 0 || yChange != 0)
        {
            Select(xChange + CursorX, yChange + CursorY);
        }
    }

    public void Select(int x, int y)
    {
        x = Mathf.Clamp(x, 0, Width - 1);
        y = Mathf.Clamp(y, 0, Height - 1);
        Selected.UnSelect();
        (CursorX, CursorY) = (x, y);
        Selected.Select();
        InfoPanel.SetRoomInfo(Selected.RoomData);
    }


    [Button("Toggle")]
    public void Toggle()
    {
        if (IsHidden)
        {
            Select(CurrentX, CurrentY);
            Animator.SetTrigger("Show");
            Selected.Select();
            IsHidden = false;
        }
        else
        {
            Animator.SetTrigger("Hide");
            IsHidden = true;
        }
    }

    [Button("Generate Rooms")]
    public void GenerateRooms()
    {
        InitRooms();
        BossRoom = InitBossRoom();
        StartRoom = InitStartRoom();
        CurrentX = StartRoom.X;
        CurrentY = StartRoom.Y;
        GeneratePathToBossRoom(StartRoom, new HashSet<(int, int)>() { (StartRoom.X, StartRoom.Y) });
        GeneratePathToBossRoom(StartRoom, new HashSet<(int, int)>() { (StartRoom.X, StartRoom.Y) });
        AddReward(StartRoom);
        PopulateUIMap();

    }

    private void PopulateUIMap()
    {
        CellTarget.DestroyChildren();
        for (int y = 0; y < Width; y++)
        {
            for (int x = 0; x < Height; x++)
            {
                MapCellController cell = Instantiate(MapCellPrefab, CellTarget);
                cell.SetRoom(Rooms[x, y]);
            }
        }
    }
    private void InitRooms()
    {
        Rooms = new Room[Width, Height];
        for (int row = 0; row < Width; row++)
        {
            for (int col = 0; col < Height; col++)
            {
                Rooms[col, row] = new Room
                {
                    Y = row,
                    X = col,
                    Wave = MiddleWaves[Random.Range(0, MiddleWaves.Length)]
                };
            }
        }
    }

    private Room InitBossRoom()
    {
        int bossRow = Random.Range(0, Height);
        Room bossRoom = Rooms[Width - 1, bossRow];
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
        currentRoom.IsAccessible = true;
        var possibleExits = Offsets
            .Select(pair => (pair.X + currentRoom.X, pair.Y + currentRoom.Y))
            .Where(pair => pair.Item1 >= 0 && pair.Item1 < Width && pair.Item2 >= 0 && pair.Item2 < Height)
            .Where(pair => !seenRooms.Contains(pair))
            .ToArray();

        AddReward(currentRoom);
        if (possibleExits.Length == 0)
        {
            GeneratePathToBossRoom(StartRoom, new HashSet<(int, int)>() { (StartRoom.X, StartRoom.Y) });
            return;
        }
        (int x, int y) = possibleExits[Random.Range(0, possibleExits.Length)];
        seenRooms.Add((x, y));
        Room nextRoom = Rooms[x, y];
        ConnectRooms(currentRoom, nextRoom, 0);
        if (nextRoom == BossRoom)
        {
            BossRoom.IsAccessible = true;
            return;
        }
        GeneratePathToBossRoom(nextRoom, seenRooms);
    }

    private void AddReward(Room room)
    {
        if (room == BossRoom) { return; }
        if (room.Rewards.Count > 0) { return; }
        float chance = Random.Range(0.0f, 1.0f);
        int rewards = 1;
        if (chance > 0.3f)
        {
            rewards = 2;
        }
        else if (chance > 0.50f)
        {
            rewards = 3;
        }
        while (rewards > 0)
        {
            room.Rewards.Add(Rewards[Random.Range(0, Rewards.Length)]);
            rewards--;
        }


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
            ConnectRooms(Rooms[First.X + 1, First.Y], Second, depth + 1);
        }
        // Second is to left of first
        else if (xOff > 0)
        {
            Rooms[First.X, First.Y].Left = Rooms[First.X - 1, First.Y];
            Rooms[First.X - 1, First.Y].Right = Rooms[First.X, First.Y];
            ConnectRooms(Rooms[First.X - 1, First.Y], Second, depth + 1);
        }
        // Second is below first
        else if (yOff < 0)
        {
            Rooms[First.X, First.Y].Down = Rooms[First.X, First.Y + 1];
            Rooms[First.X, First.Y + 1].Up = Rooms[First.X, First.Y].Down;
            ConnectRooms(Rooms[First.X, First.Y + 1], Second, depth + 1);
        }
        // Second is above first
        else if (yOff > 0)
        {
            Rooms[First.X, First.Y].Up = Rooms[First.X, First.Y - 1];
            Rooms[First.X, First.Y - 1].Down = Rooms[First.X, First.Y];
            ConnectRooms(Rooms[First.X, First.Y - 1], Second, depth + 1);
        }
    }

}


