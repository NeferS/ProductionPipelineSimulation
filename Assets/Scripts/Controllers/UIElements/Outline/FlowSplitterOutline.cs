using UnityEngine;
using UnityEngine.UI;

public class FlowSplitterOutline : PipelineOutline
{
    [SerializeField] private RectTransform decimalInput;

    public override void TriggerConfiguration() 
    {
        base.TriggerConfiguration();
        RectTransform[] decimals = new RectTransform[((FlowSplitter)selfElement).doorWeights.Length];
        for (int i = 0; i < ((FlowSplitter)selfElement).doorWeights.Length; i++)
        {
            RectTransform _decimal = Instantiate(decimalInput, MainSceneManager.Instance.elementPanes.pipelineConfigContent.transform);
            _decimal.transform.position = _decimal.transform.position - new Vector3(0, _decimal.rect.height * i, 0);
            _decimal.GetComponentInChildren<Text>().text = "Weight " + i + ":";
            _decimal.GetComponentInChildren<InputField>().text = "" + ((FlowSplitter)selfElement).doorWeights[i];
            decimals[i] = _decimal;
        }

        foreach(RectTransform _decimal in decimals)
            _decimal.GetComponentInChildren<InputField>().onEndEdit.AddListener(delegate
            {
                int index = int.Parse(_decimal.GetComponentInChildren<Text>().text.Substring(7, 1));
                ((FlowSplitter)selfElement).doorWeights[index] = float.Parse(_decimal.GetComponentInChildren<InputField>().text);
            });
    }

    public override void TriggerInfo()
    {
        base.TriggerInfo();
        RectTransform specific = MainSceneManager.Instance.elementPanes.content.GetComponentsInChildren<RectTransform>()
                                    [MainSceneManager.Instance.elementPanes.content.GetComponentsInChildren<RectTransform>().Length - 1];
        ((FlowSplitter)selfElement).Normalize(((FlowSplitter)selfElement).doorWeights);
        float[] doorWeights = ((FlowSplitter)selfElement).doorWeights;
        for (int i=0; i<selfElement.outputDoors.Length; i++)
        {
            RectTransform row = Instantiate(MainSceneManager.Instance.elementPanes.pipelineInfoRow, specific.transform);
            row.position = row.position - new Vector3(0, MainSceneManager.Instance.elementPanes.pipelineInfoRow.rect.height * i, 0);
            Text[] textBoxes = row.GetComponentsInChildren<Text>();
            textBoxes[0].text = "Weight " + (i + 1) + ":";
            textBoxes[1].text = "" + doorWeights[i];
        }
    }
}
