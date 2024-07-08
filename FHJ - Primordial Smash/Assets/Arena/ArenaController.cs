using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public EnemyWaveData WaveData;
    public int NextWaveIx = 0;
    public int LivingEnemies;
    public int MinimumEnemies = 10;
    public int MaximumEnemies = 128;
    public RadarController Radar;
    public HUDController HUD;
    public ArrowsController Arrows;
    public bool TestArea;
    public EnemyWaveData TestWave;
    

    void Awake()
    {
        Radar = FindFirstObjectByType<RadarController>();
        Debug.Assert(Radar != null);
        HUD = FindFirstObjectByType<HUDController>();
        Debug.Assert(HUD != null);
        Arrows = FindFirstObjectByType<ArrowsController>();
        Debug.Assert(Arrows != null);
    }

    public void EnterArea()
    {
        if (Radar.CurrentRoom.IsComplete) { return; }
        HUD.ShowAreaReady();
    }

    public void StartArea()
    {
        if (Radar.CurrentRoom.HasStarted) { return; }
        HUD.HideAreaReady();
        Radar.CurrentRoom.HasStarted = true;
        if (!TestArea)
        {
            WaveData = Radar.CurrentRoom.Wave;
        }
        {
            WaveData = TestWave;
        }
        SpawnNext();
    }

    public void FinishArea()
    {
        if (Radar.CurrentRoom.IsComplete) { return; }
        Radar.CurrentRoom.IsComplete = true;        
    }

    public IEnumerator SpawnCoroutine(EnemySpawnGroup enemySpawnGroup, float delay)
    {
        WaitUntil waitForFewerEnemies = new (() => LivingEnemies < MaximumEnemies);
        foreach (SpawnGroupEntry entry in enemySpawnGroup.Entries)
        {
            WaitForSeconds wait = new (entry.SpawnDelay);
            foreach (EnemyData enemy in entry.Enemies)
            {
                GameObject newEnemy = Instantiate(enemy.Prefab);
                float xOff = Random.Range(-2, 2);
                float yOff = Random.Range(-2, 2);
                Vector3 position = SpawnPoints[(int)entry.SpawnPoint].position;
                position.x += xOff;
                position.y += yOff;
                newEnemy.transform.position = position;
                yield return wait;
                yield return waitForFewerEnemies;
            }
        }
        while (delay > 0 && LivingEnemies > MinimumEnemies)
        {
            delay -= Time.deltaTime;
            yield return null;
        }
        SpawnNext();
    }


    [Button("SpawnNext")]
    public void SpawnNext()
    {
        // TODO: Wave complete nothing to spawn
        if (NextWaveIx >= WaveData.Entries.Count) 
        { 
            StartCoroutine(WaitForFinish());
            return; 
        }
        StartCoroutine(SpawnCoroutine(WaveData.Entries[NextWaveIx].SpawnGroup, WaveData.Entries[NextWaveIx].Delay));
        NextWaveIx++;
    }

    private IEnumerator WaitForFinish()
    {
        yield return new WaitUntil(() => LivingEnemies == 0);
        yield return new WaitForSeconds(2);
        FinishArea();
        HUD.ShowAreaCleared();
        yield return new WaitForSeconds(2);
        Arrows.ShowExits(Radar.CurrentRoom);
    }
}
