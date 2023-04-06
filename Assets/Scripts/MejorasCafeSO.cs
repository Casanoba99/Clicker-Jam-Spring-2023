using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MejorasCafe", menuName = "Cafe/MejorasPasivas")]
public class MejorasCafeSO : ScriptableObject
{
    public new string name;
    public int precioInicial;
    public int cafeSegundo;
    public int incrementoPrecio;
}
