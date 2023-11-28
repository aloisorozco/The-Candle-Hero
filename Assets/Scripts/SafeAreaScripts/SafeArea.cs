using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    public MusicPlayer musicManager;
    public bool inSafeArea;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            inSafeArea = true;
            if (!musicManager.safeAreaMusicPlaying)
            {
                musicManager.switchToSafeAreaMusic();

            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            inSafeArea = false;
            if (!musicManager.levelMusicPlaying)
            {
                musicManager.switchToLevelMusic();
            }
        }
    }
}
