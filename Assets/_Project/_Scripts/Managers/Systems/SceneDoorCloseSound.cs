using _Project._Scripts.ScriptableObjects;
using UnityEngine;

/****************************************************
 *            SCENE DOOR CLOSE SOUND MANAGER       *
 ****************************************************
 * Description: Plays a door closing sound when the*
 * player transitions into a new scene via a door. *
 * Waits for proper initialization and confirms the*
 * transition was door-based before playing sound. *
 *                                                 *
 * Features:                                       *
 * - Checks if last scene transition used a door   *
 * - Plays door closing sound after a small delay  *
 * - Handles initialization timing for large scenes*
 * - Waits for SoundManager to initialize if needed*
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class SceneDoorCloseSound : MonoBehaviour
    {
        [Header("Sound Settings")]
        public InteractableSoundProfile doorCloseSoundProfile;

        [Tooltip("Delay before playing the door close sound (to simulate door shutting behind).")]
        public float delay = 0.2f; 

        [Tooltip("How long to wait after scene loads to check for door transition")]
        public float initializationDelay = 0.1f;

        private void Start()
        {
            StartCoroutine(DelayedCheck());
        }

        private System.Collections.IEnumerator DelayedCheck()
        {
            // Wait real time to ensure all systems initialize properly (even in large scenes)
            yield return new WaitForSecondsRealtime(initializationDelay);

            if (!SceneTransitionTracker.WasDoorTransition)
                yield break;

            SceneTransitionTracker.WasDoorTransition = false;

            if (doorCloseSoundProfile && doorCloseSoundProfile.interactionSounds.Length > 0)
            {
                AudioClip clip = doorCloseSoundProfile.interactionSounds[0];
                StartCoroutine(PlayDoorCloseAfterDelay(clip));
            }
        }

        private System.Collections.IEnumerator PlayDoorCloseAfterDelay(AudioClip clip)
        {
            // Wait optionally before actually playing the door sound
            yield return new WaitForSeconds(delay);
            
            float maxWaitTime = 1f;
            float elapsed = 0f;
            while (SoundManager.Instance is null && elapsed < maxWaitTime)
            {
                yield return null;
                elapsed += Time.deltaTime;
            }

            if (SoundManager.Instance is not null)
            {
                SoundManager.Instance.PlaySound(
                    clip,
                    transform.position,
                    doorCloseSoundProfile.volume,
                    doorCloseSoundProfile.pitch,
                    doorCloseSoundProfile.minDistance,
                    doorCloseSoundProfile.maxDistance
                );
            }
        }
    }
}
