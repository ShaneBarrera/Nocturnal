using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

/****************************************************
 *               INTRO SCENE CONTROLLER            *
 ****************************************************
 * Description: This script controls a short       *
 * introductory scene that shows backstory text    *
 * and automatically transitions to the main game. *
 * Includes fade-in and fade-out support.          *
 *                                                 *
 * Features:                                       *
 * - Displays text UI at scene start               *
 * - Waits for a set duration                      *
 * - Plays fade-in and fade-out animations         *
 * - Loads next scene with transition              *
 * - Hides mouse cursor                            *
 * - Plays intro sound at scene start              *
 ****************************************************/

namespace _Project._Scripts.Managers.Systems
{
    public class IntroSceneController : MonoBehaviour
    {
        [Header("Intro UI")]
        [SerializeField] private GameObject textBox;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [TextArea] [SerializeField] private string introDialogue = "You awaken in the dark...\nYou don’t remember how you got here.";

        [Header("Scene Settings")]
        [SerializeField] private string nextSceneName = "DungeonRoom";
        [SerializeField] private float textDisplayTime = 6f;

        [Header("Fade Panels")]
        [SerializeField] private GameObject fadeInPanel;
        [SerializeField] private GameObject fadeOutPanel;

        [Header("Intro Audio")]
        [SerializeField] private AudioClip introClip;
        [SerializeField] private float introVolume = 1f;
        [SerializeField] private float introPitch = 1f;

        private void Start()
        {
            Cursor.visible = false;

            if (fadeInPanel)
            {
                var fade = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity);
                Destroy(fade, 1.5f);
            }

            if (textBox != null) textBox.SetActive(true);
            if (dialogueText != null) dialogueText.text = introDialogue;
            
            if (introClip != null && SoundManager.Instance != null)
            {
                if (Camera.main != null)
                    SoundManager.Instance.PlaySound(
                        introClip,
                        Camera.main.transform.position,
                        introVolume,
                        introPitch,
                        1f,
                        30f
                    );
            }

            StartCoroutine(LoadNextSceneWithFade());
        }

        private IEnumerator LoadNextSceneWithFade()
        {
            yield return new WaitForSeconds(textDisplayTime);

            if (fadeOutPanel)
            {
                Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
            }

            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
