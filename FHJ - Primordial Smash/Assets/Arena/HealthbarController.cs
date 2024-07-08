using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HealthbarController : MonoBehaviour
{

    public PlayerComponents Player;
    public HealthPillController HealthPillPrefab;
    public Transform PillContainer;
    public List<HealthPillController> HealthPills;
    

    void Awake()
    {
        Player = FindFirstObjectByType<PlayerComponents>();
        PillContainer.DestroyChildren();
        HealthPills = new();
        PopulateContainer(Player.MaxHealth);
        Player.HealthChanged.AddListener(Render);
    }

    private void PopulateContainer(int count = -1)
    {
        if (count == -1) { count = Player.MaxHealth - PillContainer.childCount; }
        while (count > 0)
        {
            var pill = Instantiate(HealthPillPrefab, PillContainer);
            HealthPills.Add(pill);
            count--;
        }
    }

    public void Render(int health)
    {
        PopulateContainer();
        for (int ix = 0; ix < HealthPills.Count; ix++)
        {
            if (ix < health)
            {
                HealthPills[ix].Heal();
            }
            else
            {
                HealthPills[ix].Damage();
            }
        }
    }

}