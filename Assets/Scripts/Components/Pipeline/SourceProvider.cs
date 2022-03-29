using UnityEngine;

public class SourceProvider : PipelineElement
{
    private BlockCommand blockCommand;

    void Start()
    {
        blockCommand = GetComponent<BlockCommand>();
    }

    void Update()
    {
        if (MainSceneManager.Instance.TimeScale > 0)
        {
            ComputeProductionStep(null);
        }
    }

    public override int TriggerInput(DoorController door, Collider obj) { return -1; }

    public override GameObject TriggerOutput(int door, GameObject block)
    {
        block = base.TriggerOutput(door, null);
        return blockCommand == null ? block : blockCommand.Execute(block);
    }

}
