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
    [SerializeField] GameObject lava;
    [SerializeField] GameObject gunTurret;

    public List<Personnage> enn;

    MeshFilter[] mesh = new MeshFilter[2];

    public int index = 1;

    [SerializeField] GameObject box1;
    [SerializeField] GameObject box2;
    [SerializeField] GameObject box3;
    [SerializeField] GameObject box4;


    // Start is called before the first frame update
    void Start()
    {
        placeInstance = this;
        mesh[0] = GetComponent<MeshFilter>();
        mesh[1] = GameObject.FindGameObjectWithTag("Nearby").GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameManagerInstance.points != null)
        {
            Vector3[] man = GetColliderVertexPositions(mesh[index]);
            box1.transform.position = man[0];
            box2.transform.position = man[1];
            box3.transform.position = man[2];
            box4.transform.position = man[3];
        }
        
    }

    public void placer(Vector3 posMouse)
    {
        if (GameManager.gameManagerInstance.points != null && GameManager.gameManagerInstance.FindMouseObject("Ground") && Input.GetMouseButtonDown(0))
        {
            GameObject tmp = ObjectPool.instance.GetPooledObject(wall);
            tmp.transform.position = posMouse;
            tmp.transform.rotation = gameObject.transform.rotation;
            tmp.SetActive(true);

            int j = 0;
            int i = 0;
            for (i = 0; i < GameManager.gameManagerInstance.points.GetLength(0); i++)
            {
                for (j = 0; j < GameManager.gameManagerInstance.points.GetLength(0) - 1; j++)
                {

                    if (IsPointInPolygon(GetColliderVertexPositions(mesh[0]), GameManager.gameManagerInstance.points[i,j].transform.position))
                    {
                        GameManager.gameManagerInstance.points[i, j].GetComponent<MeshRenderer>().material = GameManager.gameManagerInstance.mat[1];
                        GameManager.gameManagerInstance.points[i, j].Isolation();
                    }else if (IsPointInPolygon(GetColliderVertexPositions(mesh[1]), GameManager.gameManagerInstance.points[i, j].transform.position))
                    {
                        GameManager.gameManagerInstance.points[i, j].GetComponent<MeshRenderer>().material = GameManager.gameManagerInstance.mat[0];
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


    public bool IsPointInPolygon(Vector3[] polygon, Vector3 point) // <<<<<<<<<<¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬ This code verify if a point is inside the polygon. Check only x and z axis
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

    public Vector3[] GetColliderVertexPositions(MeshFilter mesh) //<<<<<<<<<<¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬ Get 4 corner of the mesh
    {
        Vector3[] vertices = mesh.mesh.vertices;
        Vector3[] output = new Vector3[4];
        output[0] = transform.TransformPoint(vertices[2]);
        output[1] = transform.TransformPoint(vertices[3]);
        output[2] = transform.TransformPoint(vertices[4]);
        output[3] = transform.TransformPoint(vertices[5]);

        return output;
    }
}
