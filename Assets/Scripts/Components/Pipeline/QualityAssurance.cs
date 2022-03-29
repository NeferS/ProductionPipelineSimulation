using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(QAConstraint))]
public class QualityAssurance : PipelineElement
{
    private QAConstraint constraint;

    [Serializable]
    protected struct DoorMapping
    {
        public QAConstraint.Outcome outcome;
        public int outputDoor;
    }
    [SerializeField] private DoorMapping[] doorMappings;
    private Dictionary<QAConstraint.Outcome, int> outcomeIndices;

    void Start()
    {
        constraint = GetComponent<QAConstraint>();
        outcomeIndices = new Dictionary<QAConstraint.Outcome, int>();
        foreach (DoorMapping mapping in doorMappings)
            outcomeIndices.Add(mapping.outcome, mapping.outputDoor);
    }

    public override int TriggerInput(DoorController door, Collider obj)
    {
        int doorIndex = base.TriggerInput(door, obj);
        ComponentBlock block = obj.GetComponent<ComponentBlock>();
        if (block != null)
        {
            if (!door.GetState().Equals(DoorController.DoorState.Open))
            {
                MainSceneManager.Instance.BlockObs.Unregister(block);
                Destroy(obj.gameObject);
                discardedBlocks++;
                if (outlineController != null)
                    outlineController.EventuallyUpdate();
            }
            else if (inputTypes[doorIndex].name.Equals(block.gameObject.name))
            {
                obj.gameObject.SetActive(false);
                QAConstraint.Outcome outcome = constraint.Evaluate(obj.gameObject);
                int outputDoorIndex = outcomeIndices[outcome];
                outputDoors[outputDoorIndex].Trigger();
                StartCoroutine(LateTriggerOutput(outputDoorIndex, obj.gameObject));
                StartCoroutine(ReOpenDoor(door, discardTime));
            }
            else
            {
                MainSceneManager.Instance.BlockObs.Unregister(block);
                Destroy(obj.gameObject);
                StartCoroutine(ReOpenDoor(door, discardTime));
                discardedBlocks++;
                if (outlineController != null)
                    outlineController.EventuallyUpdate();
            }
        }
        return doorIndex;
    }
}
