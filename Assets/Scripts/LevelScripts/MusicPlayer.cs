using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] public AudioSource safeAreaMusic;
    [SerializeField] public AudioSource levelMusic;
    [SerializeField] public SafeArea safeArea;

    public bool levelMusicPlaying;
    public bool safeAreaMusicPlaying;

    // Start is called before the first frame update
    void Awake()
    {
        if (safeArea.inSafeArea)
        {
            safeAreaMusic.Play();
            levelMusicPlaying = false;
            safeAreaMusicPlaying = true;
        }
        else
        {
            levelMusic.Play();
            levelMusicPlaying = true;
            safeAreaMusicPlaying = false;
        }
    }

    private IEnumerator Crossfade(AudioSource current, AudioSource newSource)
    {
        const float fadeDuration = 1.5f;

        float timer = 0f;

        while (timer < fadeDuration)
        {
            current.volume = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            newSource.volume = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        // Stop the current audio source and reset its volume
        current.Stop();
        current.volume = 1f;

        // Play the new audio source
        newSource.Play();
    }

    public void switchToSafeAreaMusic()
    {
        levelMusicPlaying = false;
        safeAreaMusicPlaying = true;
        StartCoroutine(Crossfade(levelMusic, safeAreaMusic));
    }

    public void switchToLevelMusic()
    {
        levelMusicPlaying = true;
        safeAreaMusicPlaying = false;
        StartCoroutine(Crossfade(safeAreaMusic, levelMusic));
    }

}
