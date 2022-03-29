using UnityEngine;

public class FlowSplitter : PipelineElement
{
    [SerializeField] public float[] doorWeights;

    void Start()
    {
        Normalize(doorWeights);
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

                int outputDoorIndex = -1;
                float weightsSum = 0f;
                float random = Random.value;
                for (int i = 0; i < doorWeights.Length - 1; i++)
                {
                    weightsSum += doorWeights[i];
                    if (random < weightsSum)
                    {
                        outputDoorIndex = i;
                        break;
                    }
                }
                if (outputDoorIndex == -1)
                    outputDoorIndex = outputDoors.Length - 1;

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

    public void Normalize(float[] vector)
    {
        float magnitude = 0;
        for (int i = 0; i < vector.Length; i++)
            magnitude += vector[i];
        if (magnitude > 0)
            for (int i = 0; i < vector.Length; i++)
                vector[i] /= magnitude;
    }
}
