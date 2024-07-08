using System.Collections.Generic;
using System.Linq;
using System.Text;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class InfoPanelController : MonoBehaviour
{
    public TextMeshProUGUI Position;
    public TextMeshProUGUI BioScan;
    public TextMeshProUGUI Materials;

    public void SetRoomInfo(Room room)
    {
        Position.text = $"Position: ({room.X}, {room.Y})";
        BioScan.text = GenerateBioScan(room);
        Materials.text = "* ???";
    }

    private string GenerateBioScan(Room room)
    {
        HashSet<string> EnemyNames = new();
        foreach (var entry in room.Wave.Entries)
        {
            foreach (var group in entry.SpawnGroup.Entries)
            {
                foreach (var enemy in group.Enemies)
                {
                    EnemyNames.Add(enemy.Name);
                }
            }
        }
        return $" * {string.Join("\n * ", EnemyNames)}".TrimEnd();
    }
}