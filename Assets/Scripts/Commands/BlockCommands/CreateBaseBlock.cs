using UnityEngine;

public class CreateBaseBlock : BlockCommand
{
    private static int baseSequential = 0,
                       bodySequential = 0,
                       detailSequential = 0;
    private static Color[] colors = { Color.white, Color.black, Color.blue, Color.red, Color.green };
    private static string[] bodyValueSet = { "A", "B", "C" };

    public override GameObject Execute(GameObject block)
    {
        ComponentBlock compBlock = block.GetComponent<ComponentBlock>();
        string sequential = "";
        if (compBlock.gameObject.name.Equals("BaseBlock"))
        {
            sequential = "" + baseSequential;
            compBlock.Value = "" + Random.Range(0, 101);
            baseSequential++;
        }
        else if (compBlock.gameObject.name.Equals("BodyBlock"))
        {
            sequential = "" + bodySequential;
            compBlock.Value = bodyValueSet[Random.Range(0, bodyValueSet.Length)];
            bodySequential++;
        }
        else if (compBlock.gameObject.name.Equals("DetailBlock"))
        {
            sequential = "" + detailSequential;
            compBlock.Value = "" + Random.Range(-30, 30);
            detailSequential++;
        }
        compBlock.Id = compBlock.IdPrefix + sequential.PadLeft(4, '0');
        compBlock.BlockColor = colors[Random.Range(0, colors.Length)];
        return block;
    }
}