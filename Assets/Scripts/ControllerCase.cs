using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ControllerCase : MonoBehaviour
{
    /// <summary>
    /// ��������� ��������� � �������� ���������� ������� � �������������� � ���������� ������ ControllerCase
    /// </summary>
    public static ControllerCase Singleton { get; private set; }

    [SerializeField] private Transform _containerCase;
    [SerializeField] private Transform _caseTemplate;

    private void Awake()
    {        
        Singleton = this;

        ControllerSector.Singleton.OnAddCase.RemoveAllListeners();

        ControllerSector.Singleton.OnAddCase.AddListener((obj) =>
        {
            Transform caseTransform = Instantiate(_caseTemplate, _containerCase);
            caseTransform.GetComponentInChildren<TextMeshProUGUI>().text = obj.CurrentTimeTracking;

            ControllerSector.Singleton.sectorsViewList[^1].caseViewsList.Add(caseTransform.AddComponent<CaseView>());

            ControllerSector.Singleton.shellDataSectorList.DataSectorList[^1].CaseList.Add(new DataSector.DataCase() 
            {
                TimeTrackingText = obj.CurrentTimeTracking,
            });

            JsonServiceUtility.SaveData(ControllerSector.Singleton.shellDataSectorList);
        });
    }

    /// <summary>
    /// ������� ��������������� CaseView � ��������� ������� �������
    /// </summary>
    /// <param name="timeTrackingText"></param>
    /// <param name="infoText"></param>
    public void SpawnNotSerializationModificationCase(string timeTrackingText, string infoText = "")
    {
        Transform caseTransform = Instantiate(_caseTemplate, _containerCase);
        caseTransform.GetComponentInChildren<TextMeshProUGUI>().text = timeTrackingText;

        ControllerSector.Singleton.sectorsViewList[^1].caseViewsList.Add(caseTransform.AddComponent<CaseView>());
        caseTransform.GetComponent<CaseView>().GetComponentInChildren<TMP_InputField>().text = infoText;
    }

    /// <summary>
    /// ����������� � JSON ���������� � ��������� ����� CaseView �� ������ SectorView � ������ CaseView
    /// </summary>
    /// <param name="infoText"></param>
    /// <param name="locationSectorView"></param>
    /// <param name="caseForLocationText"></param>
    public void SaveInfoTextInCase(string infoText, SectorView locationSectorView, CaseView caseForLocationText)
    {
        ControllerSector.Singleton.shellDataSectorList
            .DataSectorList[ControllerSector.Singleton.sectorsViewList.IndexOf(locationSectorView)]
            .CaseList[locationSectorView.caseViewsList.IndexOf(caseForLocationText)].InfoText = infoText;

        JsonServiceUtility.SaveData(ControllerSector.Singleton.shellDataSectorList);
    }

    /// <summary>
    /// ������� �� ����� JSON � ���������� ��������� CaseView � SectorView
    /// </summary>
    /// <param name="locationSectorView"></param>
    /// <param name="caseForDelete"></param>
    public void DeleteCase(SectorView locationSectorView, CaseView caseForDelete)
    {
        ControllerSector.Singleton.shellDataSectorList
            .DataSectorList[ControllerSector.Singleton.sectorsViewList.IndexOf(locationSectorView)]
            .CaseList.RemoveAt(locationSectorView.caseViewsList.IndexOf(caseForDelete));

        locationSectorView.caseViewsList.Remove(caseForDelete);

        JsonServiceUtility.SaveData(ControllerSector.Singleton.shellDataSectorList);
    }
}
