using System;
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
        Materials.text = GenerateRewardsScan(room);
    }

    private string GenerateRewardsScan(Room room)
    {
        if (room.IsBossRoom)
        {
            return "<color=\"yellow\">Helicopter Detected</color>";
        }
        if (room.Rewards.Count == 0 || !room.IsAccessible) { return "None Detected"; }
        HashSet<string> rewards = new();
        foreach (var reward in room.Rewards)
        {
            rewards.Add(reward.Name);
        }
        return $" * {string.Join("\n * ", rewards)}".TrimEnd();
    }

    private string GenerateBioScan(Room room)
    {
        if (!room.IsAccessible) { return "None Detected"; }
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