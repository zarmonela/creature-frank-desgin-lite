using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Rigidbody))]

public class Attractor : MonoBehaviour
{
    public float G = 0.3674f;
    private Rigidbody rb;
    public List<Rigidbody> attractedObjects;

    public int OptionNumber = 0;
    [SerializeField] private float minimumMass = 0.0f;
    public float MinimumMass
    {   
        get{ return minimumMass;} 
        set{minimumMass = value;}
    }
    [SerializeField]private float maximumMass = 0.0f;
    public float MaximumMass
    {
        get{ return maximumMass;} 
        set{maximumMass = value;}
    }
    [SerializeField]private float timePerChange = 0.0f; 
    public float TimePerChange
    {
        get{ return timePerChange;} 
        set{ timePerChange = value;}
        }

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        foreach (Rigidbody attracted in attractedObjects)
        {
            Attract(attracted);
        }
        
    }

    void Attract(Rigidbody rbToAttract)
    {
        Vector3 direction = rb.position - rbToAttract.position;

        float distance = direction.magnitude;

        float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);

        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }

    public void ChangeMassInstantly(float newMass)
    {
        rb.mass = newMass;
    }

    public void ChangeMassOverXTime(float newMass, float xTime)
    {
        StartCoroutine(LerpMass(newMass, xTime));
    }

    public void ChangeMassInXTime(float newMass, float xTime)
    {
        StartCoroutine(ChangeMassInXTimeRoutine(newMass,xTime));
    }

    private IEnumerator ChangeMassInXTimeRoutine(float newMass, float xTime)
    {
        yield return new WaitForSeconds(xTime);
        ChangeMassInstantly(newMass);
        yield return null;
        
    }

    public void ChangeMassBackForth(float minMass, float maxMass, float timeForOneChange)
    {
        ChangeMassInstantly(minMass);
        StartCoroutine(ChangeMassBackForthRoutine(minMass, maxMass, timeForOneChange));

    } 

    IEnumerator ChangeMassBackForthRoutine(float minMass, float maxMass, float timeForOneChange)
    {
        ChangeMassInstantly(minMass);
        yield return StartCoroutine(LerpMass(maxMass, timeForOneChange));
        yield return StartCoroutine(LerpMass(minMass, timeForOneChange));
    }
    IEnumerator LerpMass(float endValue, float xTime)
    {
        float time = 0;
        float startValue = rb.mass;
        while (time < xTime)
        {
            rb.mass = Mathf.Lerp(startValue, endValue, time / xTime);
            time += Time.deltaTime;
            yield return null;
        }
        rb.mass = endValue;
    }

}

