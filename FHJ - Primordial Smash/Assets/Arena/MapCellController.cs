using UnityEngine;
using UnityEngine.UI;

public class MapCellController : MonoBehaviour
{
    public Color BossRoomColor;
    public Color StartRoomColor;
    public Room RoomData;
    public Image RightExit;
    public Image LeftExit;
    public Image UpExit;
    public Image DownExit;
    public Image TileImage;

    public void SetRoom(Room roomData)
    {
        RoomData = roomData;
        if (roomData.IsStartRoom) { TileImage.color = StartRoomColor; }
        else if (roomData.IsBossRoom) { TileImage.color = BossRoomColor; }
        RightExit.enabled = RoomData.Right != null;
        LeftExit.enabled = RoomData.Left != null;
        UpExit.enabled = RoomData.Up != null;
        DownExit.enabled = RoomData.Down != null;
    }
}