using UnityEngine;

public abstract class ComponentBlock : MonoBehaviour
{

    [SerializeField] protected string valueName;
    [SerializeField] protected string idPrefix;

    public abstract string Value { get; set; }

    public string ValueName
    {
        get => valueName;
        private set { }
    }

    protected string id;
    public string Id { get => id; set => id = value; }

    public string IdPrefix { get => idPrefix; private set { } }

    public virtual Color BlockColor
    {
        get => GetComponent<Renderer>().material.color;
        set => GetComponent<Renderer>().material.SetColor("_Color", value);
    }

    public abstract void AddBlock(ComponentBlock block);
    public abstract void RemoveBlock(ComponentBlock block);
    public abstract ComponentBlock GetChildAt(int index);
    public abstract int GetChildrenCount();
}
