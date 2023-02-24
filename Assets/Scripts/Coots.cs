using System;

namespace SadBrains
{
    public class Coots : OutputObject
    {
        public static event Action CootsDestroyed;
        
        public CootsType CootsType { get; private set; }

        public void SetType(CootsType type)
        {
            CootsType = type;
            spriteRenderer.sprite = type.sprite;
        }

        public override void Destroy()
        {
            CootsDestroyed?.Invoke();
            base.Destroy();
        }
    }
}