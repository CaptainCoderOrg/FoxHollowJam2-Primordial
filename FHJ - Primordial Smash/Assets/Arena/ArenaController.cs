using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaController : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public Transform[] PlayerSpawnPoints;
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
    public PlayerComponents Player;
    public Animator FadeAnimator;

    void Awake()
    {
        Player = FindFirstObjectByType<PlayerComponents>();
        Debug.Assert(Player != null);
        Radar = FindFirstObjectByType<RadarController>();
        Debug.Assert(Radar != null);
        HUD = FindFirstObjectByType<HUDController>();
        Debug.Assert(HUD != null);
        Arrows = FindFirstObjectByType<ArrowsController>();
        Debug.Assert(Arrows != null);
    }

    public void Transition()
    {
        Debug.Log("Transition?");
        StartCoroutine(AnimateTransition());
    }

    public IEnumerator AnimateTransition()
    {
        if(Radar.NextRoom == null) { yield break; }
        FadeAnimator.SetTrigger("FadeOut");
        Radar.CurrentX = Radar.NextRoom.X;
        Radar.CurrentY = Radar.NextRoom.Y;
        Arrows.HideExits();
        yield return new WaitForSeconds(1);
        FadeAnimator.SetTrigger("FadeIn");
        Player.transform.position = PlayerSpawnPoints[Radar.ExitDirection.ToIndex()].position;
        
    }

    public void EnterArea()
    {
        if (Radar.CurrentRoom.IsComplete)
        { 
            Arrows.ShowExits(Radar.CurrentRoom);
            return; 
        }
        NextWaveIx = 0;
        HUD.ShowAreaReady();
    }

    public void StartArea()
    {
        if (Radar.CurrentRoom.HasStarted) { return; }
        HUD.HideAreaReady();
        Radar.CurrentRoom.HasStarted = true;
        WaveData = Radar.CurrentRoom.Wave;
        if (TestArea)
        {
            WaveData = TestWave;
        }
        SpawnNext();
    }

    public void Win() => StartCoroutine(AnimateWin());

    public IEnumerator AnimateWin()
    {
        Arrows.HideExits();
        FadeAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("EndCredits");

    }

    public void FinishArea()
    {
        if (Radar.CurrentRoom.IsComplete) { return; }
        Radar.CurrentRoom.IsComplete = true;        
        if (Radar.CurrentRoom == Radar.BossRoom)
        {
            Win();
        }
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
                Vector3 position = SpawnPoints[entry.SpawnPoint.ToIndex()].position;
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
        // yield return new WaitForSeconds(2);
        HUD.ShowAreaCleared();
        yield return new WaitForSeconds(2);
        Arrows.ShowExits(Radar.CurrentRoom);
        FinishArea();
    }
}
