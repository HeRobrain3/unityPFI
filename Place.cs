using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Place : MonoBehaviour
{
    public static Place placeInstance;

    [SerializeField] GameObject wall;

    public List<Personnage> enn;

    MeshFilter[] mesh = new MeshFilter[2];

    // Start is called before the first frame update
    void Start()
    {
        placeInstance = this;
        mesh[0] = GetComponent<MeshFilter>();
        mesh[1] = GameObject.FindGameObjectWithTag("Nearby").GetComponent<MeshFilter>();
    }

    public void placer(Vector3 posMouse) //READ THIS: if you want to see if a point is inside (only x and z axis not y) a rectangular prism or cube, do: if(IsPointInPolygon(GetColliderVertexPositions(meshOfCube), point)) {...}
    {
       //Useless code for the example deleted
    }


    public bool IsPointInPolygon(Vector3[] polygon, Vector3 point) // <<<<<<<<<<¬¬¬¬¬¬¬¬¬¬¬¬ This code verify if a point is inside the polygon. Check only x and z axis. Output true if the point is inside the polygon. CAUTION: Verifie if the polygon[] points created the wanted shaped.
    {
        int polygonLength = polygon.Length, i = 0;
        bool inside = false;
        // x, y for tested point.
        float pointX = point.x, pointY = point.z;
        // start / end point for the current polygon segment.
        float startX, startY, endX, endY;
        Vector3 endPoint = polygon[polygonLength - 1];
        endX = endPoint.x;
        endY = endPoint.z;
        while (i < polygonLength)
        {
            startX = endX; startY = endY;
            endPoint = polygon[i++];
            endX = endPoint.x; endY = endPoint.z;
            //
            inside ^= (endY > pointY ^ startY > pointY) /* ? pointY inside [startY;endY] segment ? */
                      && /* if so, test if it is under the segment */
                      ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
        }
        return inside;
    }

    public Vector3[] GetColliderVertexPositions(MeshFilter mesh) //<<<<<<<<<<¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬ Output the upper 4 corner of a rectancular prism or cube mesh
    {
        Vector3[] vertices = mesh.mesh.vertices;
        Vector3[] output = new Vector3[4];
        output[0] = transform.TransformPoint(vertices[4]);
        output[1] = transform.TransformPoint(vertices[5]);
        output[2] = transform.TransformPoint(vertices[3]);
        output[3] = transform.TransformPoint(vertices[2]);

        return output;
    }
}
