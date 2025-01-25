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
          
            ControllerSector.Instance.shellDataSectorList
            .DataSectorList[ControllerSector.Instance.sectorsViewList.IndexOf(sectorView)]
            .CaseList.RemoveAt(sectorView.caseViewsList.IndexOf(this));

            sectorView.caseViewsList.Remove(this);   

            JsonServiceUtility.SaveData(ControllerSector.Instance.shellDataSectorList);

            Destroy(this.gameObject);
        });
    }
}
