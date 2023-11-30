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
    private bool firstMusicPlayed;
    private bool isPlayingDeathSound;
    private PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        firstMusicPlayed = false;
        player = FindAnyObjectByType<PlayerMovement>();
    }

    private void Update()
    {
        if (player.isDead)
        {
            levelMusic.Stop();
        }

        if (safeArea.inSafeArea && !firstMusicPlayed)
        {
            safeAreaMusic.Play();
            levelMusicPlaying = false;
            safeAreaMusicPlaying = true;
            firstMusicPlayed = true;
        }
        else if (!safeArea.inSafeArea && !firstMusicPlayed)
        {
            levelMusic.Play();
            levelMusicPlaying = true;
            safeAreaMusicPlaying = false;
            firstMusicPlayed = true;
        }
        else if (safeArea.inSafeArea && firstMusicPlayed && !safeAreaMusicPlaying)
        {
            switchToSafeAreaMusic();
        }
        else if (!safeArea.inSafeArea && firstMusicPlayed && !levelMusicPlaying)
        {
            switchToLevelMusic();
        }
    }

    private IEnumerator Crossfade(AudioSource current, AudioSource newSource)
    {
        const float fadeDuration = 1.5f;

        float timer = 0f;

        while (timer < fadeDuration)
        {
            current.volume = Mathf.Lerp(0.3f, 0f, timer / fadeDuration);
            newSource.volume = Mathf.Lerp(0f, 0.3f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        // Stop the current audio source and reset its volume
        current.Stop();
        current.volume = 0.3f;

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


    public void deathSound(bool play, AudioSource deathSound)
    {
        if (play)
        {
            StopCoroutine("stopDeathSound");
            isPlayingDeathSound = true;
            StartCoroutine(playDeathSound(deathSound));
        }
        else
        {
            StopCoroutine("playDeathSound");
            isPlayingDeathSound = false;
            StartCoroutine(stopDeathSound(deathSound));
        }
    }

    private IEnumerator playDeathSound(AudioSource deathSound)
    {
        const float fadeDuration = 5f;

        float timer = 0f;

        deathSound.Stop();
        deathSound.volume = 0;
        deathSound.Play();
        while (timer < fadeDuration && isPlayingDeathSound)
        {
            deathSound.volume = Mathf.Lerp(0f, 1.0f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator stopDeathSound(AudioSource deathSound)
    {
        const float fadeDuration = 0.1f;

        float timer = 0f;

        while (timer < fadeDuration && isPlayingDeathSound)
        {
            deathSound.volume = Mathf.Lerp(0.5f, 0.0f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        deathSound.Stop();
    }

}
