using UnityEngine;

public abstract class BlockCommand : MonoBehaviour
{
    public abstract GameObject Execute(GameObject block);
}
