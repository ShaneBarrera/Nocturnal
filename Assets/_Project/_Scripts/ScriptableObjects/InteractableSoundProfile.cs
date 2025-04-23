using UnityEngine;

/****************************************************
 *         INTERACTABLE SOUND PROFILE              *
 ****************************************************
 * Description: This ScriptableObject defines the  *
 * audio settings for interactable objects such as *
 * doors, switches, or chests. It allows for       *
 * multiple audio clips with adjustable playback   *
 * properties including volume, pitch, and 3D      *
 * spatial settings.                               *
 *                                                 *
 * Features:                                       *
 * - Holds an array of interaction sound clips     *
 * - Adjustable volume and pitch per interaction   *
 * - Supports 3D spatial sound configuration       *
 * - Easily pluggable into interactable prefabs    *
 ****************************************************/

namespace _Project._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewInteractableSoundProfile", menuName = "Audio/Interactable Sound Profile")]
    public class InteractableSoundProfile : ScriptableObject
    {
        public AudioClip[] interactionSounds; 
        public float volume = 1f;
        public float pitch = 1f;
        public float minDistance = 1f;
        public float maxDistance = 50f;
    }
}