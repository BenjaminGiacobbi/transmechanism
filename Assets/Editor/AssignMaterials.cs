using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssignMaterial : ScriptableWizard
{
    public Material _material;
    GameObject[] gos;

    void OnWizardUpdate()
    {
        helpString = "Select Game Obects";
        isValid = (_material != null);
    }

    void OnWizardCreate()
    {

        gos = Selection.gameObjects;
        foreach (var go in gos)
        {
            go.GetComponent<Renderer>().material = _material;
        }
    }

    [MenuItem("Custom/Assign Material", false, 4)]
    static void SetMaterial()
    {
        DisplayWizard("Assign Material", typeof(AssignMaterial), "Assign");
    }
}
