using System;
using System.Collections;
using _Project._Scripts.ScriptableObjects;
using _Project._Scripts.Units.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

/****************************************************
 *                 DEATH MANAGER                   *
 ****************************************************
 * Description: Manages the player's death state.  *
 * When the player is killed by an enemy, this     *
 * class activates a UI panel and handles the      *
 * transition back to the main menu.               *
 *                                                 *
 * Features:                                       *
 * - Shows death panel on death                    *
 * - Disables movement & freezes time              *
 * - Resets ScriptableObjects on returning to menu *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class DeathManager : MonoBehaviour
    {
        [Header("UI Components")]
        public GameObject deathPanel;

        [Header("Scene Management")]
        public string mainMenuScene;

        [Header("Sound Settings")]
        public EnemySoundProfile soundProfile;

        private Player _player;

        [Obsolete("Start is obsolete")]
        private void Start()
        {
            if (deathPanel != null)
                deathPanel.SetActive(false);

            _player = FindObjectOfType<Player>();
        }

        public void TriggerDeath()
        {
            if (_player != null)
                _player.enabled = false;

            Cursor.visible = true;

            // Fade out all active AmbientSound3D instances
            foreach (var ambient in AmbientSound3D.ActiveInstances)
            {
                if (ambient != null)
                    ambient.FadeToVolume(0f, 0.5f);
            }

            // Play scream immediately
            if (soundProfile != null && soundProfile.deathScreamClip != null)
            {
                SoundManager.Instance.PlaySound(
                    soundProfile.deathScreamClip,
                    _player.transform.position,
                    soundProfile.deathScreamVolume,
                    soundProfile.pitch,
                    soundProfile.minDistance,
                    soundProfile.maxDistance
                );
            }

            // Delay death clip slightly (e.g., 1.5s after scream)
            if (soundProfile != null && soundProfile.playerDeathClip != null)
            {
                StartCoroutine(PlayDeathClipAfterDelay(1.5f));
            }

            StartCoroutine(ShowDeathScreenAfterDelay());
        }

        private IEnumerator PlayDeathClipAfterDelay(float delay)
        {
            yield return new WaitForSecondsRealtime(delay); // Use unscaled time since timescale may be zero
            if (_player is null || soundProfile is null || soundProfile.playerDeathClip is null) yield break;

            SoundManager.Instance.PlaySound(
                soundProfile.playerDeathClip,
                _player.transform.position,
                soundProfile.playerDeathVolume,
                soundProfile.pitch,
                soundProfile.minDistance,
                soundProfile.maxDistance
            );
        }
        
        private IEnumerator ShowDeathScreenAfterDelay()
        {
            deathPanel?.SetActive(true);
            yield return new WaitForEndOfFrame();
            Time.timeScale = 0f;
        }

        public void ReturnToMainMenu()
        {
            GameSaveManager.GameSave?.ResetAllData();
            Cursor.visible = true;
            Time.timeScale = 1f;
            SceneManager.LoadScene(mainMenuScene);
        }
    }
}
