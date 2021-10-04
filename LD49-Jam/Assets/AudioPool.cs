using System.Collections.Generic;
using UnityEngine;

public class AudioPool : MonoBehaviour
{
    private readonly Queue<AudioSource> _audioSourcePool = new Queue<AudioSource>();

    public AudioSource GetOrAddAudioSource()
    {
        if (_audioSourcePool.Count == 0)
        {
            return gameObject.AddComponent<AudioSource>();
        }

        var source = _audioSourcePool.Dequeue();
        source.enabled = true;
        return source;
    }

    public void DespawnAudioSource(AudioSource source)
    {
        source.enabled = false;
        _audioSourcePool.Enqueue(source);
    }
}
