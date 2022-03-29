using System.Collections.Generic;

public class Destroyer : BlockObserver
{
    private List<ComponentBlock> blocks;

    void Start()
    {
        blocks = new List<ComponentBlock>();
    }

    public override void Register(ComponentBlock block)
    {
        blocks.Add(block);
    }

    public override void Unregister(ComponentBlock block)
    {
        blocks.Remove(block);
    }

    public override void Action()
    {
        foreach (ComponentBlock block in blocks)
            Destroy(block.gameObject);
        blocks.Clear();
    }
}
