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

    [SerializeField] GameObject wall;
    [SerializeField] GameObject lava;
    [SerializeField] GameObject gunTurret;
    [SerializeField] GameObject RaycastAgent;

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

        if (GameManager.gameManagerInstance.points != null && GameManager.gameManagerInstance.FindMouseObject("Ground") && Input.GetMouseButtonDown(0))
        {
           // MeshFilter meshWall;
            MeshFilter meshRaycast;

            GameObject tmp = ObjectPool.instance.GetPooledObject(wall);
            tmp.transform.position = posMouse;
            tmp.transform.rotation = gameObject.transform.rotation;
            RaycastAgent.transform.position = posMouse;
            RaycastAgent.transform.rotation = gameObject.transform.rotation;
            tmp.SetActive(true);
            //meshWall = tmp.GetComponent<MeshFilter>();
            meshRaycast = RaycastAgent.GetComponent<MeshFilter>();

            for (int i = 0; i < GameManager.gameManagerInstance.points.GetLength(0); i++)
            {
                for (int j = 0; j < GameManager.gameManagerInstance.points.GetLength(0) - 1; j++)
                {

                    if (IsPointInPolygon(GetColliderVertexPositions(meshRaycast), GameManager.gameManagerInstance.points[i,j].transform.position))
                    {
                        GameManager.gameManagerInstance.points[i, j].GetComponent<MeshRenderer>().material = GameManager.gameManagerInstance.mat[1];
                        GameManager.gameManagerInstance.points[i, j].Isolation();

                    }else if (IsPointInPolygon(GetColliderVertexPositions(meshRaycast), GameManager.gameManagerInstance.points[i, j].transform.position))
                    {
                        print("nein!");
                        GameManager.gameManagerInstance.points[i, j].RaycastCheck();
                    }
                }
            }
            enn.Sort((p1, p2) => p1.GetComponent<Personnage>().Dist(tmp.transform.position).CompareTo(p2.GetComponent<Personnage>().Dist(tmp.transform.position)));
            foreach (Personnage p in enn)
            {
                p.genPath();
            }

        }

    }


    public bool IsPointInPolygon(Vector3[] polygon, Vector3 point)
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
