using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using _Project._Scripts.Managers.Systems; // ✅ For GameSaveManager access
using _Project._Scripts.Units.Player;

/****************************************************
 *                     CAR                         *
 ****************************************************
 * Description: When the player enters the car's   *
 * trigger zone and presses 'E', the scene fades   *
 * out and transitions to the "EndGame" scene.     *
 *                                                 *
 * Features:                                       *
 * - Trigger-based player interaction              *
 * - Locks movement for player and enemy           *
 * - Fades audio and visuals                       *
 * - Resets game data before exiting               *
 * - Scene loading on input                        *
 ****************************************************/

namespace _Project._Scripts.Units.Car
{
    public class Car : MonoBehaviour
    {
        [Header("Scene Settings")]
        [SerializeField] private string endSceneName = "EndGame";
        [SerializeField] private float fadeDelay = 1f;

        [Header("Fade Panels")]
        [SerializeField] private GameObject fadeOutPanel;

        private bool _playerInRange;
        private bool _hasInteracted;

        private Player.Player _player;
        private Enemy.Enemy _enemy;

        private void Awake()
        {
            _player = FindFirstObjectByType<Player.Player>();
            _enemy = FindFirstObjectByType<Enemy.Enemy>();
        }

        private void Update()
        {
            if (_playerInRange && !_hasInteracted && Input.GetKeyDown(KeyCode.E))
            {
                _hasInteracted = true;

                // Lock movement
                _player?.LockMovement();
                _enemy?.LockMovement(true);

                StartCoroutine(FadeAndExit());
            }
        }

        private IEnumerator FadeAndExit()
        {
            // Fade out ambient audio
            foreach (var ambient in AmbientSound3D.ActiveInstances)
            {
                if (ambient is not null)
                    ambient.FadeOutAndDestroy();
            }

            // Play fade-out visual
            if (fadeOutPanel)
                Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);

            yield return new WaitForSeconds(fadeDelay);
            
            GameSaveManager.GameSave?.ResetAllData();

            SceneManager.LoadScene(endSceneName);
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
    }
}
