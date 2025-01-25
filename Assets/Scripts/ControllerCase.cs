using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ControllerCase : MonoBehaviour
{
    /// <summary>
    /// Позволяет обратится к текущему последнему сектору и соответственно к экземпляру класса ControllerCase
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

    public void SpawnNotSerializationModificationCase(string timeTrackingText)
    {
        Transform caseTransform = Instantiate(_caseTemplate, _containerCase);
        caseTransform.GetComponentInChildren<TextMeshProUGUI>().text = timeTrackingText;

        ControllerSector.Singleton.sectorsViewList[^1].caseViewsList.Add(caseTransform.AddComponent<CaseView>());
    }

    public void DeleteCase(SectorView locationSectorView, CaseView caseForDelete)
    {
        ControllerSector.Singleton.shellDataSectorList
            .DataSectorList[ControllerSector.Singleton.sectorsViewList.IndexOf(locationSectorView)]
            .CaseList.RemoveAt(locationSectorView.caseViewsList.IndexOf(caseForDelete));

        locationSectorView.caseViewsList.Remove(caseForDelete);

        JsonServiceUtility.SaveData(ControllerSector.Singleton.shellDataSectorList);
    }
}
