using UnityEngine;

public class SumValues : QAConstraint
{
    [SerializeField] private int minValue;

    public override Outcome Evaluate(GameObject obj)
    {
        ComponentBlock block = obj.GetComponent<ComponentBlock>();
        int value = 0;
        for (int i = 0; i < block.GetChildrenCount(); i++)
            value += int.Parse(block.GetChildAt(i).Value);
        return value > minValue ? Outcome.Pass : Outcome.Discard;
    }
}
