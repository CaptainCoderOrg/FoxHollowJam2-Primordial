using System.Collections.Generic;

[System.Serializable]
public class Room
{
    public Room Up;
    public Room Right;
    public Room Down;
    public Room Left;
    public EnemyWaveData Wave;
    public List<MaterialsData> Rewards = new ();
    public int Y;
    public int X;
    public bool IsBossRoom;
    public bool IsStartRoom;
    public bool IsAccessible = false;
    public bool HasStarted = false;
    public bool IsComplete = false;

    public override string ToString()
    {
        return $"Room ({X}, {Y})";
    }
}