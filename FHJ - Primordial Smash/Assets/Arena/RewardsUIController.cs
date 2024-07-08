using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardsUIController : MonoBehaviour
{
    public PowerUpData[] TestData;
    public TextMeshProUGUI Rewards;
    public ContentSizeFitter Fitter;
    public Animator Animator;
    public List<PowerUpData> PowerUps;
    public PlayerComponents Player;
    void Awake()
    {
        Animator = GetComponent<Animator>();
        Player = FindFirstObjectByType<PlayerComponents>();
    }
    public void SetRewards(PowerUpData[] Powerups)
    {
        PowerUps = new List<PowerUpData>(Powerups);
        StringBuilder builder = new();
        foreach (PowerUpData powerup in Powerups)
        {
            builder.AppendLine($"<color=\"yellow\">{powerup.Name}</color>\n{powerup.Description}");
        }
        Rewards.text = builder.ToString();
        Animator.SetTrigger("Show");
    }

    public void Claim()
    {
        Animator.SetTrigger("Hide");
        if (PowerUps.Count > 0)
        {
            if (PowerUps.FirstOrDefault(v => v.HealAll) != null)
            {
                Player.Damage = 0;
            }
            Player.PowerUps.AddRange(PowerUps);
            Player.NotifyHealthChanged();
            PowerUps.Clear();
        }
               
    }

    [Button("Test")]
    public void Test() => SetRewards(TestData);
}