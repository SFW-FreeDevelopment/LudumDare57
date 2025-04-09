using UnityEngine;

public class OneShotAudioPlayer : MonoBehaviour
{
    public enum SoundEffect
    {
        Click,
        Back,
        GameOver,
        GameWon,
        Collect1,
        Collect2
    }
    
    public static void PlayClip(SoundEffect soundEffect, float volume = 0.8f, float pitch = 1f, float destroyAfter = 5f)
    {
        if (GameManager.Instance == null) return;
        var audioClip = soundEffect switch
        {
            SoundEffect.Back => GameManager.Instance.backSound,
            SoundEffect.GameOver => GameManager.Instance.gameOverSound,
            SoundEffect.GameWon => GameManager.Instance.gameWonSound,
            SoundEffect.Collect1 => GameManager.Instance.collect1Sound,
            SoundEffect.Collect2 => GameManager.Instance.collect2Sound,
            SoundEffect.Click => GameManager.Instance.clickSound,
            _ => GameManager.Instance.clickSound
        };
        PlayClip(audioClip, volume, pitch, destroyAfter);
    }
    
    public static void PlayClip(AudioClip clip, float volume = 0.8f, float pitch = 1f, float destroyAfter = 5f)
    {
        if (clip == null) return;

        GameObject go = new GameObject("OneShotAudio_" + clip.name);
        go.transform.position = new Vector3(0, 0, 0);

        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();

        DontDestroyOnLoad(go);
        Destroy(go, destroyAfter);
    }
}
