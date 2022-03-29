using System.Collections;
using UnityEngine;

public abstract class PipelineElement : MonoBehaviour
{
    [SerializeField] public GameObject[] inputTypes;
    [SerializeField] public GameObject[] outputTypes;
    [SerializeField] public DoorController[] inputDoors;
    [SerializeField] public DoorController[] outputDoors;
    [SerializeField] public Conveyor[] inputConveyors;
    [SerializeField] public Conveyor[] outputConveyors;
    [SerializeField] public float[] outputDistanceFromDoors;
    [SerializeField] public int[] productionTimes;
    [SerializeField] protected float discardTime;
    protected float[] elapsedTimes;
    protected bool[] outputDoorTrigger;
    protected float threshold = 1f;

    protected OutlineController outlineController;
    public int outBlocks;
    public int discardedBlocks;

    void Awake()
    {
        elapsedTimes = new float[productionTimes.Length];
        outputDoorTrigger = new bool[productionTimes.Length];
        for (int i = 0; i < elapsedTimes.Length; i++)
        {
            elapsedTimes[i] = 0f;
            outputDoorTrigger[i] = false;
        }
        outlineController = GetComponent<OutlineController>();
        outBlocks = 0;
        discardedBlocks = 0;
    }

    public virtual int TriggerInput(DoorController door, Collider obj)
    {
        int doorIndex = MatchDoor(door, true);
        inputConveyors[doorIndex].RemoveBlock();
        return doorIndex;
    }

    public virtual GameObject TriggerOutput(int door, GameObject block)
    {
        if (block == null)
        {

            block = Instantiate(outputTypes[door], Vector3.zero, outputDoors[door].transform.rotation);
            block.name = block.name.Substring(0, block.name.IndexOf('('));
            MainSceneManager.Instance.BlockObs.Register(block.GetComponent<ComponentBlock>());
        }
        else
        {
            block.SetActive(true);
            block.transform.rotation = outputDoors[door].transform.rotation;
        }

        float y = gameObject.GetComponent<Renderer>().bounds.extents.y - block.GetComponent<Renderer>().bounds.extents.y;
        block.transform.position = gameObject.transform.position +
                                   outputDoors[door].transform.forward * outputDistanceFromDoors[door] -
                                   new Vector3(0, y, 0);
        outputConveyors[door].PutBlock(block);
        outputDoors[door].Trigger();
        outBlocks++;
        if (outlineController != null)
            outlineController.EventuallyUpdate();
        return block;
    }

    public virtual void ResetSelf()
    {
        StopAllCoroutines();
        foreach (DoorController door in inputDoors)
            door.ResetSelf();
        foreach (DoorController door in outputDoors)
            door.ResetSelf();
        for (int i = 0; i < elapsedTimes.Length; i++)
        {
            elapsedTimes[i] = 0f;
            outputDoorTrigger[i] = false;
        }
        outBlocks = 0;
        discardedBlocks = 0;
    }

    protected virtual IEnumerator ReOpenDoor(DoorController door, float seconds)
    {
        yield return new WaitForSeconds(seconds / MainSceneManager.Instance.TimeMultiplier);
        door.Trigger();
    }

    protected IEnumerator LateTriggerOutput(int door, GameObject block)
    {
        yield return new WaitForSeconds(threshold / MainSceneManager.Instance.TimeMultiplier);
        TriggerOutput(door, block);
    }

    protected virtual void ComputeProductionStep(GameObject block)
    {
        float deltaTime = Time.deltaTime;
        for (int i = 0; i < outputTypes.Length; i++)
        {
            elapsedTimes[i] += deltaTime;
            if (!outputDoorTrigger[i] && elapsedTimes[i] >= (productionTimes[i] - threshold) / MainSceneManager.Instance.TimeMultiplier)
            {
                outputDoors[i].Trigger();
                outputDoorTrigger[i] = true;
            }

            if (elapsedTimes[i] >= productionTimes[i] / MainSceneManager.Instance.TimeMultiplier)
            {
                TriggerOutput(i, block);
                elapsedTimes[i] = 0f;
                outputDoorTrigger[i] = false;
            }
        }
    }

    protected int MatchDoor(DoorController door, bool input)
    {
        DoorController[] doors = input ? inputDoors : outputDoors;
        int doorIndex = -1;
        for (int i = 0; i < doors.Length; i++)
            if (doors[i] == door)
            {
                doorIndex = i;
                break;
            }
        return doorIndex;
    }
}
