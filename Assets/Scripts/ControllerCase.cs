using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ControllerCase : MonoBehaviour
{
    public static ControllerCase Instance { get; private set; }

    [SerializeField] private Transform _containerCase;
    [SerializeField] private Transform _caseTemplate;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        ControllerSector.Instance.OnAddCase.RemoveAllListeners();

        ControllerSector.Instance.OnAddCase.AddListener((obj) =>
        {
            Transform caseTransform = Instantiate(_caseTemplate, _containerCase);
            caseTransform.GetComponentInChildren<TextMeshProUGUI>().text = obj.CurrentTimeTracking;

            ControllerSector.Instance.sectorsViewList[^1].caseViewsList.Add(caseTransform.AddComponent<CaseView>());

            ControllerSector.Instance.shelldataSectorList.DataSectorList[^1].CaseList.Add(new DataSector.DataCase() 
            {
                TimeTrackingText = obj.CurrentTimeTracking,
            });

            JsonServiceUtility.SaveData(ControllerSector.Instance.shelldataSectorList);
        });
    }

    public void SpawnModificationCase(string timeTrackingText)
    {
        Transform caseTransform = Instantiate(_caseTemplate, _containerCase);
        caseTransform.GetComponentInChildren<TextMeshProUGUI>().text = timeTrackingText;

        ControllerSector.Instance.sectorsViewList[^1].caseViewsList.Add(caseTransform.AddComponent<CaseView>());
    }
}
