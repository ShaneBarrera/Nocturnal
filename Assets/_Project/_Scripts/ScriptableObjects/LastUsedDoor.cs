using UnityEngine;

namespace _Project._Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LastUsedDoor", menuName = "ScriptableObjects/LastUsedDoor")]
    public class LastUsedDoor : ScriptableObject
    {
        public string lastDoorID = "";

        public void SetDoorID(string id)
        {
            lastDoorID = id;
        }

        public void ResetDoorID()
        {
            lastDoorID = "";
        }
    }
}