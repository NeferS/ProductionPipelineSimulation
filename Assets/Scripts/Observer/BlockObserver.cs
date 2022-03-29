using UnityEngine;

public abstract class BlockObserver : MonoBehaviour
{
    public abstract void Register(ComponentBlock block);
    public abstract void Unregister(ComponentBlock block);
    public abstract void Action();
}
