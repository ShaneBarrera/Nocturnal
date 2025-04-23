using _Project._Scripts.Managers.Systems;
using _Project._Scripts.ScriptableObjects;
using UnityEngine;
using System.Collections;

/****************************************************
 *                  LOCKED DOOR CLASS              *
 ****************************************************
 * Description: Controls the behavior of locked    *
 * doors in the game. The player must possess the  *
 * correct key in their Backpack to open the door. *
 * Otherwise, a locked sound will play.            *
 *                                                 *
 * Features:                                       *
 * - Checks player inventory for door key          *
 * - Plays different sounds for open/locked        *
 * - Locks both player and enemy movement on open  *
 * - Triggers door open animation and transition   *
 * - Supports state persistence via BoolValue      *
 ****************************************************/

namespace _Project._Scripts.Units.Doors
{
    public enum DoorType
    {
        Standard,
        Silver,
        Bronze,
        Bloody
    }

    public class LockedDoors : MonoBehaviour
    {
        private static readonly int OpenDoor = Animator.StringToHash("Open");

        [Header("References")]
        private Animator _animator;
        private static Enemy.Enemy _enemy;
        private static Rigidbody2D _enemyRb;
        private Player.Player _player;
        public BoolValue storedOpen;

        [Header("State Management")]
        private bool _playerInRange;
        private bool _hasInteracted;

        [Header("Serialized Fields")]
        [SerializeField] private DoorType doorType;
        [SerializeField] private Backpack backpack;
        [SerializeField] private Thing thing;

        [Header("Sound")]
        public InteractableSoundProfile doorOpenSoundProfile;
        public InteractableSoundProfile doorLockedSoundProfile;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _hasInteracted = storedOpen.runtimeValue;

            _enemy = FindFirstObjectByType<Enemy.Enemy>();
            if (_enemy != null && _enemy.gameObject != null)
                _enemyRb = _enemy.GetComponent<Rigidbody2D>(); 

            _player = FindFirstObjectByType<Player.Player>();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
                _playerInRange = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                _playerInRange = false;
        }

        private void Update()
        {
            if (_playerInRange && !_hasInteracted && Input.GetKeyDown(KeyCode.E))
            {
                if (HasRequiredKey())
                {
                    StartCoroutine(OpenDoorSequence());
                }
                else
                {
                    PlayLockedDoorSound();
                }
            }
        }

        private bool HasRequiredKey()
        {
            return doorType switch
            {
                DoorType.Standard => backpack.hasStandardKey,
                DoorType.Silver => backpack.hasSilverKey,
                DoorType.Bronze => backpack.hasBronzeKey,
                DoorType.Bloody => backpack.hasBloodyKey,
                _ => false
            };
        }

        private IEnumerator OpenDoorSequence()
        {
            _hasInteracted = true;
            storedOpen.runtimeValue = true;

            _animator.SetBool(OpenDoor, true);

            if (_player is not null)
                _player.LockMovement();

            if (_enemy is not null && _enemyRb is not null)
            {
                _enemy.LockMovement(true);
            }
            
            if (doorOpenSoundProfile && doorOpenSoundProfile.interactionSounds.Length > 0)
            {
                var clip = doorOpenSoundProfile.interactionSounds[0];
                DoorSoundPlayer.Play(
                    clip,
                    transform.position,
                    doorOpenSoundProfile.volume,
                    doorOpenSoundProfile.pitch,
                    doorOpenSoundProfile.minDistance,
                    doorOpenSoundProfile.maxDistance
                );
            }
            
            foreach (var ambient in AmbientSound3D.ActiveInstances)
            {
                if (ambient is not null)
                    ambient.FadeOutAndDestroy();
            }
            
            yield return new WaitForSeconds(1.0f);

            SceneTransitionTracker.WasDoorTransition = true;
            if (FindFirstObjectByType<SceneTransition>() is { } transition)
                transition.ForceTransition();
        }
        
        private void PlayLockedDoorSound()
        {
            if (!doorLockedSoundProfile || doorLockedSoundProfile.interactionSounds.Length <= 0) return;
            AudioClip clip = doorLockedSoundProfile.interactionSounds[0];
            SoundManager.Instance.PlaySound(
                clip,
                transform.position,
                doorLockedSoundProfile.volume,
                doorLockedSoundProfile.pitch,
                doorLockedSoundProfile.minDistance,
                doorLockedSoundProfile.maxDistance
            );
        }

    }
}
