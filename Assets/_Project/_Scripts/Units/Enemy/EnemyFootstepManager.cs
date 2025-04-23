using UnityEngine;

/****************************************************
 *             ENEMY FOOTSTEP MANAGER              *
 ****************************************************
 * Description: Handles footstep sound playback    *
 * for enemies based on their current movement     *
 * state. Only plays sounds while the enemy is in  *
 * motion (not stalking or idle). Ensures sound    *
 * variation and 3D audio spatialization.          *
 *                                                 *
 * Features:                                       *
 * - Plays random footstep sounds from a list      *
 * - Adjustable cooldown, volume, and pitch        *
 * - Avoids repeating the same clip back-to-back   *
 * - Disables playback during idle/stalk states    *
 * - Spatial 3D audio using AudioSource settings   *
 ****************************************************/

namespace _Project._Scripts.Units.Enemy
{
    public class EnemyFootstepManager : MonoBehaviour
    {
        [Header("Footstep Sound Settings")]
        [SerializeField] private AudioClip[] footstepSounds;
        [SerializeField] private float footstepCooldown = 0.4f;
        [SerializeField] private float footstepVolume = 1f;
        [SerializeField] private float footstepPitch = 1f;

        [Header("Audio Source Setup")]
        [SerializeField] private AudioSource audioSource;

        private float _footstepTimer;
        private int _lastFootstepIndex = -1;
        private Enemy _enemy;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.minDistance = 1f;
            audioSource.maxDistance = 15f;
        }

        private void Update()
        {
            if (_enemy is null || _enemy.currentState == EnemyState.Stalk || _enemy.currentState == EnemyState.Idle)
                return;

            _footstepTimer += Time.deltaTime;

            if (_footstepTimer >= footstepCooldown)
            {
                PlayFootstep();
                _footstepTimer = 0f;
            }
        }

        private void PlayFootstep()
        {
            if (footstepSounds.Length == 0) return;

            int newIndex;
            do
            {
                newIndex = Random.Range(0, footstepSounds.Length);
            } while (footstepSounds.Length > 1 && newIndex == _lastFootstepIndex);

            _lastFootstepIndex = newIndex;
            audioSource.pitch = footstepPitch;
            audioSource.PlayOneShot(footstepSounds[newIndex], footstepVolume);
        }

        public void SetFootstepVolume(float volume)
        {
            footstepVolume = Mathf.Clamp01(volume);
        }

        public void SetFootstepPitch(float pitch)
        {
            footstepPitch = Mathf.Clamp(pitch, 0.5f, 2f);
        }
    }
}
