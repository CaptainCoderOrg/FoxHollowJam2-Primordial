using System.Collections.ObjectModel;
using UnityEngine;
public static class MusicExtensions
{

    public static float ScaleDb(float normalized)
    {
        if (normalized >= 1) { return 5; }
        if (normalized <= 0) { return -80; }
        return (normalized * 35) - 30;
    }
}