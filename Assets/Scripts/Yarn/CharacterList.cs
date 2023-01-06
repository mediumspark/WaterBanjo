using UnityEngine;

public class CharacterList : ScriptableObject
{
    [System.Serializable]
    public struct Character
    {
        public string CharacterName; 
        public GameObject Prefab;
        public string[] Emotions; 
    }

    public Character[] Characters; 

}