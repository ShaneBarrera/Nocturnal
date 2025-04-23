using UnityEngine;

/****************************************************
 *                  DOOR SOUND PLAYER              *
 ****************************************************
 * Description: This static class handles spatial  *
 * audio playback for door-related sounds (e.g.,   *
 * open or close effects). It ensures a persistent *
 * AudioSource is available for consistent door    *
 * audio and plays sounds at specified world       *
 * positions with spatial settings.                *
 *                                                 *
 * Features:                                       *
 * - Singleton-style static audio playback         *
 * - Spatial 3D audio with configurable settings   *
 * - Ensures audio source persists across scenes   *
 * - Designed specifically for door interaction FX *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public static class DoorSoundPlayer
    {
        private static AudioSource _source;

        public static void Play(AudioClip clip, Vector3 position, float volume, float pitch, float minDist, float maxDist)
        {
            if (_source is null)
            {
                GameObject obj = new GameObject("DoorSoundPlayer");
                _source = obj.AddComponent<AudioSource>();
                _source.spatialBlend = 1f;
                _source.rolloffMode = AudioRolloffMode.Linear;
                Object.DontDestroyOnLoad(obj);
            }

            _source.transform.position = position;
            _source.volume = volume;
            _source.pitch = pitch;
            _source.minDistance = minDist;
            _source.maxDistance = maxDist;
            _source.clip = clip;
            _source.Play();
        }
    }
}
