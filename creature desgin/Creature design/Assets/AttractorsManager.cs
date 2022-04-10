using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class AttractorsManager : MonoBehaviour
{

    public List<Attractor> attractors;
    public List<bool> applyChanges;


    void Start()
    {
        for (int i = 0; i < attractors.Count; i++)
        {
            if(applyChanges[i]) ApplyChanges(attractors[i]);
        }

    }


    private void ApplyChanges(Attractor attractor)
    {
        int option = attractor.OptionNumber;
        switch(option)
        {
            case 0: 
                break;
            case 1: 
                attractor.ChangeMassInXTime(attractor.MaximumMass, attractor.TimePerChange);
                break;
            case 2:
                attractor.ChangeMassOverXTime(attractor.MaximumMass, attractor.TimePerChange);
                break;
            case 3: 
                attractor.ChangeMassInstantly(attractor.MaximumMass);
                break;
            case 4:
                attractor.ChangeMassBackForth(attractor.MinimumMass, attractor.MaximumMass, attractor.TimePerChange);
                break;
            default:
                break;
        }
    }

    
}