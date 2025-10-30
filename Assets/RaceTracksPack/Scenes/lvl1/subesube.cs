using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class subesube : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;

    private Spline s;
    // Start is called before the first frame update
    void Start()
    {
        s = splineContainer.Spline;
        Debug.Log(s.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
