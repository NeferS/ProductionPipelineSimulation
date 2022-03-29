public class BaseBlock : ComponentBlock
{

    public override string Value { get; set; }

    public override void AddBlock(ComponentBlock block) { }

    public override void RemoveBlock(ComponentBlock block) { }

    public override ComponentBlock GetChildAt(int index) { return null; }

    public override int GetChildrenCount() { return 0; }
}
