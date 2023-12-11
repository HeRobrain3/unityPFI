using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class Place : MonoBehaviour
{
    public static Place placeInstance;

    [SerializeField] GameObject RaycastAgent; //Receive the big box in Unity

    public List<Personnage> enn;

    
    public MeshFilter mesh2;



    // Start is called before the first frame update
    void Start()
    {
        placeInstance = this;
         //it "jumps" the parent and gives you the child component :D
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void placer(Vector3 posMouse)
    {

           //unrelated code before removed...
            MeshFilter meshRaycast;
            meshRaycast = RaycastAgent.GetComponent<MeshFilter>();
            GetColliderVertexPositions(meshRaycast)
          //unrelated code after removed...         

    }


   

    public Vector3[] GetColliderVertexPositions(MeshFilter mesh)
    {
        Vector3[] vertices = mesh.mesh.vertices;

        foreach (Vector3 i in vertices)
        {
            Debug.DrawLine(transform.TransformPoint(i), transform.TransformPoint(i) + new Vector3(0, 1, 0), Color.red, 100f);
        }

        Vector3[] output = new Vector3[4];
        output[0] = transform.TransformPoint(vertices[4]);
        output[1] = transform.TransformPoint(vertices[5]);
        output[2] = transform.TransformPoint(vertices[3]);
        output[3] = transform.TransformPoint(vertices[2]);
        return output;
    }
}
