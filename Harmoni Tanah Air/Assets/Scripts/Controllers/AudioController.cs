using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource soundSource;

    public void PlayAudio(AudioClip music, AudioClip sound, bool soundLoop)
    {
        if (sound != null)
        {
            soundSource.clip = sound;
            soundSource.Play();
            soundSource.loop = soundLoop;
        }
        else
        {
            soundSource.Stop();
        }

        if (music != null && musicSource.clip != music)
        {
            StartCoroutine(SwitchMusic(music));
        }
    }

    private IEnumerator SwitchMusic(AudioClip music)
    {
        if (musicSource.clip != null)
        {
            while (musicSource.volume > 0)
            {
                musicSource.volume -= 0.05f;
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            musicSource.volume = 0;
        }

        musicSource.clip = music;
        musicSource.Play();

        while (musicSource.volume < 1f)
        {
            musicSource.volume += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
