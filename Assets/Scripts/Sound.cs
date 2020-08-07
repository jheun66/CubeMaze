using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0.0f, 1.0f)]
    public float volume;
    // 음높이
    [Range(0.1f, 3.0f)]
    public float pitch;

    public bool loop;

    // inspector에서 안보이게
    [HideInInspector]
    public AudioSource source;
}
