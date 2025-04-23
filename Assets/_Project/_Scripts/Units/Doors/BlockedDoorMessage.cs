using _Project._Scripts.Managers.Systems;
using TMPro;
using UnityEngine;
using _Project._Scripts.ScriptableObjects;

/****************************************************
 *              BLOCKED DOOR MESSAGE               *
 ****************************************************
 * Description: This script displays a message     *
 * and plays a sound when the player approaches a  *
 * non-functional door (i.e., a blocked door).     *
 *                                                 *
 * Features:                                       *
 * - Plays "budge" sound on interaction            *
 * - Displays a message when in range              *
 * - No scene transition or key logic              *
 ****************************************************/

namespace _Project._Scripts.Units.Doors
{
    public class BlockedDoorMessage : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject textBox;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [TextArea]
        [SerializeField] private string blockedMessage = "The door won't budge...";

        [Header("Sound Settings")]
        [SerializeField] private InteractableSoundProfile budgeSoundProfile;

        private bool _playerInRange;

        private void Start()
        {
            if (textBox != null)
                textBox.SetActive(false);
        }

        private void Update()
        {
            if (!_playerInRange || !Input.GetKeyDown(KeyCode.E)) return;
            ShowBlockedMessage();
            PlayBudgeSound();
        }

        private void ShowBlockedMessage()
        {
            if (textBox is null || dialogueText is null) return;
            textBox.SetActive(true);
            dialogueText.text = blockedMessage;
        }

        private void PlayBudgeSound()
        {
            if (budgeSoundProfile is null || budgeSoundProfile.interactionSounds.Length <= 0) return;
            AudioClip clip = budgeSoundProfile.interactionSounds[0];

            SoundManager.Instance.PlaySound(
                clip,
                transform.position,
                budgeSoundProfile.volume,
                budgeSoundProfile.pitch,
                budgeSoundProfile.minDistance,
                budgeSoundProfile.maxDistance
            );
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                _playerInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            _playerInRange = false;

            if (textBox != null)
                textBox.SetActive(false);
        }
    }
}
