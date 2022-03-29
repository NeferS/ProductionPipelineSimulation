using UnityEngine;
using UnityEngine.UI;

public class AssemblerOutline : PipelineOutline
{
    [SerializeField] private RectTransform integerInput;

    public override void TriggerConfiguration() 
    {
        base.TriggerConfiguration();
        RectTransform _integer = Instantiate(integerInput, MainSceneManager.Instance.elementPanes.pipelineConfigContent.transform);
        _integer.GetComponentInChildren<Text>().text = "Craft time (s):";
        _integer.GetComponentInChildren<InputField>().text = "" + selfElement.productionTimes[0];
        _integer.GetComponentInChildren<InputField>().onEndEdit.AddListener(delegate
        {
            selfElement.productionTimes[0] = int.Parse(_integer.GetComponentInChildren<InputField>().text);
        });
    }

    public override void TriggerInfo()
    {
        base.TriggerInfo();
        RectTransform specific = MainSceneManager.Instance.elementPanes.content.GetComponentsInChildren<RectTransform>()
                                    [MainSceneManager.Instance.elementPanes.content.GetComponentsInChildren<RectTransform>().Length - 1];
        RectTransform row = Instantiate(MainSceneManager.Instance.elementPanes.pipelineInfoRow, specific.transform);
        Text[] textBoxes = row.GetComponentsInChildren<Text>();
        textBoxes[0].text = "Craft time (s):";
        textBoxes[1].text = "" + selfElement.productionTimes[0];
    }
}
