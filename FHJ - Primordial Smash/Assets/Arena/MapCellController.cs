using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapCellController : MonoBehaviour
{
    public Color BossRoomColor;
    public Color StartRoomColor;
    public Color InaccessibleColor;
    public Color AccessibleColor;
    public Room RoomData;
    public Image RightExit;
    public Image LeftExit;
    public Image UpExit;
    public Image DownExit;
    public Image TileImage;
    public Image Selected;
    public RadarController Parent;

    void Awake()
    {
        Parent = GetComponentInParent<RadarController>();
        Debug.Assert(Parent != null);
    }

    public void OnClick()
    {
        Parent.Select(RoomData.X, RoomData.Y);
    }

    public void Select()
    {
        Selected.enabled = true;
    }

    public void UnSelect()
    {
        Selected.enabled = false;
    }

    public void SetRoom(Room roomData)
    {
        RoomData = roomData;
        if (!roomData.IsAccessible) { TileImage.color = InaccessibleColor; }
        else { TileImage.color = AccessibleColor; }
        if (roomData.IsStartRoom) { TileImage.color = StartRoomColor; }
        else if (roomData.IsBossRoom) { TileImage.color = BossRoomColor; }
        RightExit.enabled = RoomData.Right != null;
        LeftExit.enabled = RoomData.Left != null;
        UpExit.enabled = RoomData.Up != null;
        DownExit.enabled = RoomData.Down != null;
    }
}