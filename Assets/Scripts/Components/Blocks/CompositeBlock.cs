using System.Collections.Generic;

public class CompositeBlock : ComponentBlock
{
    private List<ComponentBlock> children = new List<ComponentBlock>();

    public override string Value 
    {
        get { return null; }
        set { }
    }

    public override void AddBlock(ComponentBlock block)
    {
        if (block != null)
            children.Add(block);
    }

    public override void RemoveBlock(ComponentBlock block)
    {
        children.Remove(block);
    }

    public override ComponentBlock GetChildAt(int index)
    {
        if (index >= 0 && index < children.Count)
            return children[index];
        return null;
    }

    public override int GetChildrenCount()
    {
        return children.Count;
    }
}
