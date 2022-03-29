using UnityEngine;

public abstract class QAConstraint : MonoBehaviour
{
    public enum Outcome { Pass, Discard, ReElaborate };

    public abstract Outcome Evaluate(GameObject block);
}
