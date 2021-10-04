using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioPool))]
public class EnemyCry : MonoBehaviour
{
    private AudioPool _pool;
    
    void Awake()
    {
        _pool = GetComponent<AudioPool>();
    }

    public void PlayEnemyCry(Enemy e)
    {
        var clip = e.Cries[Random.Range(0, e.Cries.Length)];
        var source = _pool.GetOrAddAudioSource();
        source.clip = clip;
        source.volume = 0.5f;
        source.Play();
        StartCoroutine(Deactivate(source));
    }

    private IEnumerator Deactivate(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        _pool.DespawnAudioSource(source);
    }
}
