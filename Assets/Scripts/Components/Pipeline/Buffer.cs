using UnityEngine;

public class Buffer : PipelineElement
{
    [SerializeField, Range(1, 100)] private int capacity;
    private GameObject[] blockBuffer;
    private int input, output, size;

    void Start()
    {
        blockBuffer = new GameObject[capacity];
        input = 0;
        output = 0;
        size = 0;
    }

    void Update()
    {
        if (MainSceneManager.Instance.TimeScale > 0 && size > 0)
        {
            ComputeProductionStep(blockBuffer[output]);
        }
    }

    public override int TriggerInput(DoorController door, Collider obj)
    {
        int doorIndex = base.TriggerInput(door, obj);
        ComponentBlock block = obj.GetComponent<ComponentBlock>();
        if (block != null)
        {
            if (!door.GetState().Equals(DoorController.DoorState.Open) || size == capacity)
            {
                MainSceneManager.Instance.BlockObs.Unregister(block);
                Destroy(obj.gameObject);
                discardedBlocks++;
            }
            else if (inputTypes[doorIndex].name.Equals(block.gameObject.name))
            {
                blockBuffer[input] = block.gameObject;
                input = (input + 1) % capacity;
                size++;
                if(size < capacity)
                    StartCoroutine(ReOpenDoor(door, discardTime));
                obj.gameObject.SetActive(false);
            }
            else
            {
                MainSceneManager.Instance.BlockObs.Unregister(block);
                Destroy(obj.gameObject);
                StartCoroutine(ReOpenDoor(door, discardTime));
                discardedBlocks++;
            }
        }
        return doorIndex;
    }

    public override GameObject TriggerOutput(int door, GameObject block)
    {
        block = base.TriggerOutput(door, block);
        if (size == capacity)
            inputDoors[door].Trigger();
        size--;
        blockBuffer[output] = null;
        output = (output + 1) % capacity;
        return block;
    }

    public override void ResetSelf()
    {
        base.ResetSelf();
        blockBuffer = new GameObject[capacity];
        input = 0;
        output = 0;
        size = 0;
    }
}
