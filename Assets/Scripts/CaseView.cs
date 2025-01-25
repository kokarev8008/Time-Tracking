using UnityEngine;
using UnityEngine.UI;

public class CaseView : MonoBehaviour
{
    private void Start()
    {
        this.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            SectorView sectorView = GetComponentInParent<SectorView>(); 
            
            this.GetComponentInParent<ControllerCase>().DeleteCase(sectorView, this);

            Destroy(this.gameObject);
        });
    }
}
