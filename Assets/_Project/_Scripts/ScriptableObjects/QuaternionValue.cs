using UnityEngine;
using _Project._Scripts.Managers.Systems; // for IResettable

/****************************************************
 *               QUATERNION VALUE SO               *
 ****************************************************
 * Description: This ScriptableObject stores and   *
 * manages a Quaternion value that can persist     *
 * across scenes. Useful for tracking player       *
 * rotation, camera angles, or directional data.   *
 *                                                 *
 * Features:                                       *
 * - Stores an initial and default Quaternion      *
 * - Supports runtime resetting via IResettable    *
 * - Resets to default on deserialization          *
 * - Useful for directional persistence like       *
 *   flashlight or camera orientation              *
 ****************************************************/

namespace _Project._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "QuaternionValue", menuName = "ScriptableObjects/Quaternion", order = 1)]
    public class QuaternionValue : ScriptableObject, IResettable, ISerializationCallbackReceiver
    {
        [Header("Quaternion Values")]
        public Quaternion initialValue;
        public Quaternion defaultValue;

        /******************************************
         *       ISerializationCallbackReceiver
         ******************************************/
        public void OnAfterDeserialize()
        {
            initialValue = defaultValue;
        }

        public void OnBeforeSerialize() { }

        /******************************************
         *              IResettable
         ******************************************/
        public void ResetToDefault()
        {
            initialValue = defaultValue;
        }
    }
}