using UnityEngine;

public class SourceReceiver : PipelineElement
{

    public override int TriggerInput(DoorController door, Collider obj)
    {
        int doorIndex = base.TriggerInput(door, obj);

        GameObject block = obj.gameObject;
        if (!door.GetState().Equals(DoorController.DoorState.Closed) &&
                inputTypes[doorIndex].name.Equals(block.name))
        {
            outBlocks++;
            if (outlineController != null)
                outlineController.EventuallyUpdate();
        }
        else
        {
            discardedBlocks++;
            if (outlineController != null)
                outlineController.EventuallyUpdate();
        }

        MainSceneManager.Instance.BlockObs.Unregister(block.GetComponent<ComponentBlock>());
        Destroy(block);
        StartCoroutine(ReOpenDoor(door, productionTimes[doorIndex]));

        return doorIndex;
    }
}
