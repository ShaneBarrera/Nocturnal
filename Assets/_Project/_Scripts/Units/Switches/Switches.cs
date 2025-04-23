using _Project._Scripts.Managers.Systems;
using _Project._Scripts.ScriptableObjects;
using UnityEngine;

/****************************************************
 *                  SWITCH CLASS                    *
 ****************************************************
 * Description: This class handles the interaction *
 * with switch in the game. When the player is in  *
 * range and presses 'E', the switch opens, grants  *
 * a door, and sends a signal to notify other     *
 * systems. The switch cannot be reopened once it   *
 * has been interacted with.                       *
 *                                                  *
 * Features:                                        *
 * - Opens when the player presses 'E'             *
 * - Sends signals upon interaction                *
 ****************************************************/

namespace _Project._Scripts.Units.Switches
{
    public class Switches : Interactable
    {
        /****************************************************
         *                  ANIMATION                     *
         ****************************************************/
        [Header("Animation")]
        private static readonly int SwitchOn = Animator.StringToHash("SwitchOn");
        private Animator _animator;

        /****************************************************
         *                   AUDIO                          *  
         ****************************************************/
        public InteractableSoundProfile switchOnSoundProfile;

        /****************************************************
         *              STATE TRACKING                     *
         ****************************************************/
        private bool _hasBeenUsed = false;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            if (_animator == null)
                Debug.LogError($"Animator component is missing on Switch '{name}'.");
        }

        private void Update()
        {
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                Open();
            }
        }

        private void Open()
        {
            if (_hasBeenUsed) return;

            _hasBeenUsed = true;
            _animator.SetBool(SwitchOn, true);

            if (!switchOnSoundProfile || switchOnSoundProfile.interactionSounds.Length <= 0) return;

            AudioClip soundToPlay = switchOnSoundProfile.interactionSounds[0];
            SoundManager.Instance?.PlaySound(
                soundToPlay,
                transform.position,
                switchOnSoundProfile.volume,
                switchOnSoundProfile.pitch,
                switchOnSoundProfile.minDistance,
                switchOnSoundProfile.maxDistance
            );
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                playerInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || other.isTrigger) return;
            playerInRange = false;
        }
    }
}
