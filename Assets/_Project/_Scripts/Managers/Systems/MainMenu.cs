using UnityEngine;
using UnityEngine.SceneManagement;

/****************************************************
 *                  MAIN MENU                      *
 ****************************************************
 * Description: This class manages the main menu  *
 * functionality, allowing players to start a new *
 * game or exit the application. It handles scene *
 * transitions and application quitting.          *
 *                                                 *
 * Features:                                       *
 * - Loads the game scene when "New Game" is      *
 *   selected                                     *
 * - Plays a sound effect when New Game is clicked*
 * - Exits the application when "Exit" is chosen  *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Sound Settings")]
        [SerializeField] private AudioClip newGameClip;
        [SerializeField] private float volume = 1f;
        [SerializeField] private float pitch = 1f;

        public void Start()
        {
            Cursor.visible = true;
        }
        
        public void NewGame() 
        {
            if (newGameClip != null)
            {
                var source = new GameObject("NewGameAudio").AddComponent<AudioSource>();
                source.volume = volume;
                source.pitch = pitch;
                source.playOnAwake = false;
                source.spatialBlend = 0f; // 2D UI sound
                source.clip = newGameClip;

                DontDestroyOnLoad(source.gameObject);
                source.Play();
                Destroy(source.gameObject, newGameClip.length);
            }

            SceneManager.LoadScene("IntroScene");
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}