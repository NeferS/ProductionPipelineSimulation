using System.Collections.Generic;
using UnityEngine;

public class Assembler : PipelineElement
{
    private GameObject[] inputBlocks;

    void Start()
    {
        inputBlocks = new GameObject[inputTypes.Length];
    }

    void Update()
    {
        if (MainSceneManager.Instance.TimeScale > 0)
        {
            bool produce = true;
            for(int i=0; i<inputBlocks.Length; i++)
                if(inputBlocks[i] == null)
                {
                    produce = false;
                    break;
                }

            if (produce)
                ComputeProductionStep(null);
        }
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
                inputBlocks[doorIndex] = block.gameObject;
                obj.gameObject.SetActive(false);
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

    public override GameObject TriggerOutput(int door, GameObject block)
    {
        block = base.TriggerOutput(door, null);
        block.transform.position = block.transform.position + new Vector3(0, block.GetComponent<Collider>().bounds.extents.y, 0);

        ComponentBlock[] childrenBlocks = block.GetComponentsInChildren<ComponentBlock>();
        ComponentBlock parentBlock = childrenBlocks[0];
        List<GameObject> seenChildren = new List<GameObject>();
        for (int i = 0; i < inputBlocks.Length; i++)
        {
            int index;
            if (inputBlocks[i].GetComponent<CompositeBlock>() == null)
            {
                index = AssembleBase(childrenBlocks, inputBlocks[i], seenChildren);
                parentBlock.AddBlock(childrenBlocks[index]);
            }
            else
            {
                ComponentBlock[] nestedBlocks = inputBlocks[i].GetComponentsInChildren<ComponentBlock>();
                for (int k = 1; k < nestedBlocks.Length; k++)
                {
                    index = AssembleBase(childrenBlocks, nestedBlocks[k].gameObject, seenChildren);
                    parentBlock.AddBlock(childrenBlocks[index]);
                }
            }

            inputDoors[i].Trigger();
            MainSceneManager.Instance.BlockObs.Unregister(inputBlocks[i].GetComponent<ComponentBlock>());
            Destroy(inputBlocks[i]);
            inputBlocks[i] = null;
        }
        return block;
    }

    public override void ResetSelf()
    {
        base.ResetSelf();
        for (int i = 0; i < inputBlocks.Length; i++)
            inputBlocks[i] = null;
    }

    private int AssembleBase(ComponentBlock[] childrenBlocks, GameObject inputBlock, List<GameObject> seenChildren)
    {
        int j;
        for (j = 1; j < childrenBlocks.Length; j++)
            if (inputBlock.name.Equals(childrenBlocks[j].gameObject.name) &&
                !seenChildren.Contains(childrenBlocks[j].gameObject))
            {
                seenChildren.Add(childrenBlocks[j].gameObject);
                childrenBlocks[j].Id = inputBlock.GetComponent<ComponentBlock>().Id;
                childrenBlocks[j].BlockColor = inputBlock.GetComponent<ComponentBlock>().BlockColor;
                childrenBlocks[j].Value = inputBlock.GetComponent<ComponentBlock>().Value;
                break;
            }
        return j;
    }
}
