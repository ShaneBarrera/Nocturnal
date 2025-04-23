using UnityEngine;
using System.Collections.Generic;

/****************************************************
 *                  SOUND MANAGER                  *
 ****************************************************
 * Description: This class manages all one-shot    *
 * sound effects in the game. It uses an audio     *
 * source pool to efficiently play 3D spatialized  *
 * sounds at runtime, avoiding audio instantiation *
 * overhead. It also ensures global access via a   *
 * singleton pattern.                              *
 *                                                 *
 * Features:                                       *
 * - Singleton-based access                        *
 * - AudioSource pooling for performance           *
 * - Plays spatial 3D audio with custom parameters *
 * - Automatically disables sources after use      *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class SoundManager : MonoBehaviour
    {
        // Singleton pattern for accessing SoundManager
        public static SoundManager Instance { get; private set; }

        [Header("AudioSource Pool Settings")]
        public int poolSize = 10;  
        private readonly List<AudioSource> _audioSourcePool = new List<AudioSource>(); 

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            for (int i = 0; i < poolSize; i++)
            {
                AudioSource newSource = new GameObject("AudioSource_" + i).AddComponent<AudioSource>();
                newSource.transform.parent = transform;  // Make children of SoundManager for better organization
                newSource.playOnAwake = false;  
                _audioSourcePool.Add(newSource);
            }
        }

        // Play a sound with the provided parameters (position, volume, pitch, etc.)
        public void PlaySound(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f, float minDistance = 1f, float maxDistance = 50f)
        {
            AudioSource source = GetAvailableAudioSource();
            if (!source) return; 
           
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.spatialBlend = 1f; // 3D sound
            source.minDistance = minDistance;
            source.maxDistance = maxDistance;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.transform.position = position; // Set position in 3D space
            
            source.Play();
            
            StartCoroutine(ResetAfterDelay(source, clip.length));
        }
        
        private AudioSource GetAvailableAudioSource()
        {
            foreach (var source in _audioSourcePool)
            {
                if (source.isPlaying) continue; // Check if the source is not currently playing a sound
                source.gameObject.SetActive(true);  
                return source;
            }
            
            return null;
        }

        // Reset AudioSource after sound finishes playing
        private System.Collections.IEnumerator ResetAfterDelay(AudioSource source, float delay)
        {
            yield return new WaitForSeconds(delay);
            source.Stop();  
            source.gameObject.SetActive(false);  
        }
    }
}
