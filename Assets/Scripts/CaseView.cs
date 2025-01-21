using UnityEngine;
using UnityEngine.UI;

public class CaseView : MonoBehaviour
{
    [HideInInspector] [SerializeField] private Button _deleteCaseButton;

    private void Start()
    {
        _deleteCaseButton = GetComponentInChildren<Button>();

        _deleteCaseButton.onClick.AddListener(() =>
        {
            SectorView sectorView = GetComponentInParent<SectorView>();

            ControllerSector.Instance.shelldataSectorList
            .DataSectorList[ControllerSector.Instance.sectorsViewList.IndexOf(sectorView)]
            .CaseList.RemoveAt(sectorView.caseViewsList.IndexOf(this));

            JsonServiceUtility.SaveData(ControllerSector.Instance.shelldataSectorList);

            Destroy(this.gameObject);
        });
    }
}
