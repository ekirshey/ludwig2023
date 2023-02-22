using UnityEngine;

namespace SadBrains
{
    [CreateAssetMenu(fileName = "CootsType", menuName = "CootsType", order = 0)]
    public class CootsType : ScriptableObject
    {
        public Sprite sprite;
        public CootsOutput output;
        public CootsInput input;
    }
}