using System.Collections;
using System.Linq;
using UnityEngine;

public class SFXDestroyer : MonoBehaviour
{
    public AudioSource Source;
    public AudioClip[] Clips;
    public float PitchMin = 0.8f;
    public float PitchMax = 1.2f;
    public bool DestroyAfterPlay = true;


    void Awake()
    {
        if (Clips.Length > 0)
        {
            Source.clip = Clips[Random.Range(0, Clips.Length)];
        }
        Source = GetComponent<AudioSource>();
        Source.pitch = Random.Range(PitchMin, PitchMax);
    }

    void OnEnable()
    {
        if (!Source.isPlaying)
        {
            if (Clips.Length > 0)
            {
                Source.clip = Clips[Random.Range(0, Clips.Length)];
            }
            Source.Play();
        }
    }

    void Start()
    {
        Source.Play();
        if (DestroyAfterPlay)
        {
            StartCoroutine(DestroyOnFinished());
        }
    }

    private bool IsDone() => !Source.isPlaying;

    private IEnumerator DestroyOnFinished()
    {
        yield return new WaitUntil(IsDone);
        Destroy(gameObject);
    }
}