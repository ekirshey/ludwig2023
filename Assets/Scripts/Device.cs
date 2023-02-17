

namespace SadBrains
{
    public abstract class Device : Placeable
    {
        public abstract bool CanReceiveSignal(DeviceSignal signal);
        
        public abstract void ReceiveSignal(DeviceSignal signal);

        public abstract int GetCurrentSignalState(DeviceSignal signal);
        
        public override bool Place()
        {
            return !CollisionChecker.IsColliding(gameObject, Collider2D.bounds.center, Collider2D.size);
        }
    }
}