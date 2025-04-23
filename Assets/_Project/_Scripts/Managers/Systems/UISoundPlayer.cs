using _Project._Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;

/****************************************************
 *                  UI SOUND PLAYER                *
 ****************************************************
 * Description: Plays UI interaction sounds such as*
 * hover and click feedback when a user interacts  *
 * with UI elements like buttons. Uses a shared    *
 * AudioSource to avoid instantiating multiple     *
 * sources and supports playing randomized clips   *
 * from a provided sound profile.                  *
 *                                                 *
 * Features:                                       *
 * - Plays hover and click sounds on UI events     *
 * - Uses a UISoundProfile for flexible control    *
 * - Reuses a persistent shared AudioSource        *
 * - Supports randomized clip selection            *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class UISoundPlayer : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        public UISoundProfile soundProfile;

        private static AudioSource _sharedSource;

        private void Awake()
        {
            if (_sharedSource != null) return;
            GameObject audioObj = new GameObject("UISoundPlayer_AudioSource");
            DontDestroyOnLoad(audioObj);
            _sharedSource = audioObj.AddComponent<AudioSource>();
            _sharedSource.playOnAwake = false;
            _sharedSource.spatialBlend = 0f;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (soundProfile?.hoverClips.Length > 0)
            {
                PlayClip(soundProfile.hoverClips[Random.Range(0, soundProfile.hoverClips.Length)]);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (soundProfile?.clickClips.Length > 0)
            {
                PlayClip(soundProfile.clickClips[Random.Range(0, soundProfile.clickClips.Length)]);
            }
        }

        private void PlayClip(AudioClip clip)
        {
            if (clip == null || _sharedSource == null) return;

            _sharedSource.pitch = soundProfile.pitch;
            _sharedSource.PlayOneShot(clip, soundProfile.volume);
        }
    }
}