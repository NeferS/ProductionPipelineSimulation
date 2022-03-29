using PathCreation;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathCreator))]
public class Conveyor : MonoBehaviour
{
    [SerializeField] public float speed = 5;
    [SerializeField] private EndOfPathInstruction endOfPathInstruction = EndOfPathInstruction.Stop;
    private PathCreator pathCreator;
    private List<GameObject> travellingBlocks;
    private List<float> distancesTravelled;

    void Start()
    {
        pathCreator = GetComponent<PathCreator>();
        travellingBlocks = new List<GameObject>();
        distancesTravelled = new List<float>();
    }

    void Update()
    {
        if(MainSceneManager.Instance.TimeScale > 0f)
        {
            for (int i = 0; i < travellingBlocks.Count; i++)
            {
                distancesTravelled[i] += speed * Time.deltaTime * MainSceneManager.Instance.TimeMultiplier;
                travellingBlocks[i].transform.position = pathCreator.path.GetPointAtDistance(distancesTravelled[i], endOfPathInstruction);
                travellingBlocks[i].transform.rotation = pathCreator.path.GetRotationAtDistance(distancesTravelled[i], endOfPathInstruction);
            }
        }
    }

    public void PutBlock(GameObject block)
    {
        travellingBlocks.Add(block);
        distancesTravelled.Add(0f);
    }

    public void RemoveBlock()
    {
        travellingBlocks.RemoveAt(0);
        distancesTravelled.RemoveAt(0);
    }

    public void ResetSelf()
    {
        travellingBlocks.Clear();
        distancesTravelled.Clear();
    }
}
