using UnityEngine;

/****************************************************
 *               ENEMY SOUND PROFILE               *
 ****************************************************
 * Description: This ScriptableObject defines the  *
 * audio clips and playback settings for enemy     *
 * sound effects, specifically related to player   *
 * death events. It includes volume and spatial    *
 * parameters for more immersive sound playback.   *
 *                                                 *
 * Features:                                       *
 * - Stores death scream and player death clips    *
 * - Customizable volumes for each clip            *
 * - Global pitch and spatial audio settings       *
 * - Easily assignable to enemy-related scripts    *
 ****************************************************/

namespace _Project._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemySoundProfile", menuName = "ScriptableObjects/EnemySoundProfile", order = 2)]
    public class EnemySoundProfile : ScriptableObject
    {
        [Header("Death Sounds")]
        public AudioClip deathScreamClip;
        public AudioClip playerDeathClip;
        public float deathScreamVolume = 1f;
        public float playerDeathVolume = 1f;

        [Header("Common Settings")]
        public float pitch = 1f;
        public float minDistance = 1f;
        public float maxDistance = 20f;
    }
}