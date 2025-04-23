using UnityEngine;

/****************************************************
 *               UI SOUND PROFILE SO               *
 ****************************************************
 * Description: Stores collections of audio clips  *
 * for UI hover and click events, as well as       *
 * playback settings like volume and pitch.        *
 *                                                 *
 * Features:                                       *
 * - Holds multiple clips for hover/click sounds   *
 * - Adjustable volume and pitch per profile       *
 * - Easily assigned to UI elements via SO         *
 * - Designed for use with UISoundPlayer script    *
 ****************************************************/

namespace _Project._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "UISoundProfile", menuName = "ScriptableObjects/UISoundProfile", order = 2)]
    public class UISoundProfile : ScriptableObject
    {
        public AudioClip[] hoverClips;
        public AudioClip[] clickClips;

        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.5f, 2f)] public float pitch = 1f;
    }
}