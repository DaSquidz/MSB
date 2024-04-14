using UnityEngine;

public class OSTManager : MonoBehaviour
{
    public static OSTManager Instance;

    [SerializeField] private AudioSource noDrumsSource;
    [SerializeField] private AudioSource calmSource;
    [SerializeField] private AudioSource mainSource;

    [Range(0, 1)]
    public float musicVolume = 0.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        noDrumsSource.volume = 0; 
        calmSource.volume = musicVolume;
        mainSource.volume = 0;
    }

    public void PlayNoDrums()
    {
        AdjustVolume(noDrumsSource, musicVolume);
        AdjustVolume(calmSource, 0);
        AdjustVolume(mainSource, 0);
    }

    public void PlayCalm()
    {
        AdjustVolume(noDrumsSource, 0);
        AdjustVolume(calmSource, musicVolume * 0.7f);
        AdjustVolume(mainSource, 0);
    }

    public void PlayMain()
    {
        AdjustVolume(noDrumsSource, 0);
        AdjustVolume(calmSource, 0);
        AdjustVolume(mainSource, musicVolume * 0.6f);
    }

    private void AdjustVolume(AudioSource source, float volume)
    {
        source.volume = volume;
    }
}