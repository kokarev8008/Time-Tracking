using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Данный класс не является компонентом игрового объекта по умолчанию
/// </summary>
public class SectorView : MonoBehaviour
{
    [HideInInspector] public List<CaseView> caseViewsList = new();
}