using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource MainMusicSource;
    [SerializeField] private AudioSource MainSFXSource;


    private void Awake()
    {
        _ = Instance;
    }
    public void Start()
    {
        MainMusicSource.Play();
    }

    public bool MainMusicActivate(bool activate)
    {
        if (activate)
        {
            MainMusicSource.volume = 1f;
            return true;
        }
        else
        {
            MainMusicSource.volume = 0f;
            return false;
        }
    }
}
