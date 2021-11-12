using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MoverCoche : MonoBehaviour
{
    Vector3[] points; // Geometry
    Vector3 ecuacionParam;
    int[] tris;       // Topology
    float angle = 0;
    float timer = 0;
    bool expected = true;

    public int car = 1;
    Vector3 initialNode;
    Vector3 nextNode;
    /*public List<Transform> nodos=new List<Transform>();
    private Transform targetNodoPoint;
    private Transform originNodoPoint;
    private Transform startingNodoPoint;
    private float speed;
    private int lastNodoIndex;
    private int targetNodoIndex;
    private Vector3 currentPosition;
    private Vector3 nextPosition;
    private float minDistance=1.0f;*/



    void TransformCar()
    {
        int n = points.Length;
        int i;
        Vector4[] vs = new Vector4[n];
        Vector3[] final = new Vector3[n];
        for(i=0; i < n; i++)
        {
            vs[i] = points[i];
            vs[i].w = 1.0f;
        }
        /*
        Vector4 v0 = points[0];
        Vector4 v1 = points[1];
        Vector4 v2 = points[2];
        v0.w = 1.0f;
        v1.w = 1.0f;
        v2.w = 1.0f;
        */
        // 1. �C�mo traslado el tri�ngulo 3.2 unidades a la derecha y 2.4 unidades abajo?
        //Matrix4x4 transform1 = Transformations.TranslateM(3.2f, -2.4f, 0.0f);
        Matrix4x4 transform2 = Transformations.RotateM(angle, Transformations.AXIS.AX_Z);
        /*
        Vector4 temp1 = new Vector4(points[0].x, points[0].y, points[0].z, 1);
        Vector4 temp2 = new Vector4(points[1].x, points[1].y, points[1].z, 1);
        Vector4 temp3 = new Vector4(points[2].x, points[2].y, points[2].z, 1);
        
        v0 = transform1 * transform2 * temp1;
        v1 = transform1 * transform2 * temp2;
        v2 = transform1 * transform2 * temp3;
        */
        //Matrix4x4 A = transform1 * transform2;
        for (i = 0; i < n; i++)
        {
            vs[i] = transform2 * vs[i];
            final[i] = vs[i];
        }
        /*
        v0 = A * v0;
        v1 = A * v1;
        v2 = A * v2;
        */
        //Vector3[] points2 = { v0, v1, v2 };

        GetComponent<MeshFilter>().mesh.vertices = final;

    }


    // Start is called before the first frame update
    void Start()
    {
        /*lastNodoIndex=nodos.Count-1;
        targetNodoPoint=nodos[targetNodoIndex+1];*/
        angle = 0;
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        // Geometry
        points = mesh.vertices;
        //originNodoPoint=nodos[targetNodoPoint];

        if(car == 1){
            initialNode = new Vector3(2.75f,0.0f,0.0f);
            nextNode    = new Vector3(12.75f,0.0f, 0.0f);
        }
        if(car == 2){
            initialNode = new Vector3(12.75f, 0.0f, 0.0f);
            nextNode    = new Vector3(2.75f,0.0f,0.0f);
        }

    }

    float ecuacion(float t){
        return 10 * t;
    }

    bool moverAuto(Vector3 currentPosition, Vector3 nextPosition){
        int n = points.Length;
        int i;
        Vector4[] vs = new Vector4[n];
        Vector3[] final = new Vector3[n];

        for(i=0; i < n; i++)
        {
            vs[i] = points[i];
            vs[i].w = 1.0f;
        }

        float t = (timer += Time.deltaTime);


        // A + (B-A)t   // 12.75 - 2.75    1.43 + (0)t     6.25 + (0)

        float newPositionX = currentPosition.x + ((nextPosition.x - currentPosition.x) * t);
        //if(car == 2) Debug.Log(nextPosition.x);
        if(car == 2) newPositionX = -newPositionX;

        Vector3 np = new Vector3(newPositionX,0.0f,0.0f);

        Matrix4x4 transform2 = Transformations.TranslateM(np.x,np.y,np.z);

        for (i = 0; i < n; i++)
        {
            vs[i] = transform2 * vs[i];
            final[i] = vs[i];
        }

        GetComponent<MeshFilter>().mesh.vertices = final;

        float relativeActual = (newPositionX * n / 10000) / 6.66f;
        float relativeFinal  = Math.Abs(currentPosition.x - nextPosition.x);
        if(car == 1) relativeFinal += 3;
        if(car == 2) relativeFinal += 5;
        

        //if(car == 2) Debug.Log(relativeActual + " vs " + relativeFinal);

        if(relativeActual > relativeFinal) return false;
        else return true;
        
    }

    // Update is called once per frame
    void Update()
    {
        /*angle += 1.0f;
        if (angle > 360)
            angle = 0.0f;
        TransformCar();*/


        //Nodo 4 a Nodo 6
        // Eq. Parametrica  A + (B-A)t
        //
        /*currentPosition=originNodoPoint.position;
        nextPosition=targetNodoPoint.position;
        float currentDistance=Vector3.Distance(transform.position,targetNodoPoint.position);*/

        if(expected){
            expected =  moverAuto(initialNode, nextNode);
        }

        
       
        //checkDistance(currentDistance);
        //transform.Translate(10 * Time.deltaTime, 0f, 0f);


    }

   /* void checkDistance(float currentDistance){
        if(currentDistance<minDistance){
            targetNodoIndex++;
            updateTargetNode();
           
        }
    }

    void updateTargetNode(){
        if(targetNodoPoint>lastNodoIndex){
            targetNodoIndex=0;
        }
    }*/
}
