// Presionar.cs
using UnityEngine;

public class Presionar : MonoBehaviour
{
    public static string hacer = "nada";
    public string action;

    void OnMouseDown() => hacer = action;
    void OnMouseUp()   => hacer = "nada";
}