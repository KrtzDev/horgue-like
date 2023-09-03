using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : Singleton<AudioManager>
{
    public Sound[] sounds;
    [SerializeField] private AudioMixerGroup _master;
    [SerializeField] private AudioMixer _mixer;

    private void Start()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        // AudioManager.Instance.PlaySound("NAME");
        // AudioManager.Instance.StopSound("NAME", 0.25f);
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: '" + name + "' not found!");
            return;
        }

        s.source.outputAudioMixerGroup = _master;
        s.source.Play();
    }

    public void StopSound(string name, float duration)
    {
        StartCoroutine(SoundStop(name, duration));
    }

    public bool IsSoundPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(s.source.isPlaying)
            return true;

        return false;
    }

    public IEnumerator SoundStop(string name, float duration)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: '" + name + "' not found!");
            yield break;
        }

        float currenTime = 0;
        float start = s.source.volume;

        while(currenTime < duration)
        {
            currenTime += Time.deltaTime;
            s.source.volume = Mathf.Lerp(start, 0, currenTime / duration);
            yield return null;
        }

        s.source.Stop();

        yield break;
    }

    public void SetAudioVolume(float volume)
    {
        _master.audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        _mixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }
}
