using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ArrowsController : MonoBehaviour
{
    public Showable UpArrow;
    public Showable DownArrow;
    public Showable LeftArrow;
    public Showable RightArrow;

    public void ShowExits(Room room)
    {
        if (room.Up != null) { UpArrow.Show(); }
        if (room.Down != null) { DownArrow.Show(); }
        if (room.Left != null) { LeftArrow.Show(); }
        if (room.Right != null) { RightArrow.Show(); }
    }

    public void HideExits()
    {
        UpArrow.Hide();
        DownArrow.Hide();
        LeftArrow.Hide();
        RightArrow.Hide();
    }
}

