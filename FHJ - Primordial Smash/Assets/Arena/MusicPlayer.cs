using UnityEngine;
using UnityEngine.Events;

public class MusicPlayer : MonoBehaviour
{

    void Awake()
    {
        MusicPlayer[] players = FindObjectsByType<MusicPlayer>(FindObjectsSortMode.None);
        if(players.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

}