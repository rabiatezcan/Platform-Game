using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class enemyControl : MonoBehaviour
{
    GameObject[] visitPoints;
    bool isGetDistance = true;
    Vector3 distance;
    int distanceCounter = 0;
    bool isForward = true;
    GameObject character;
    RaycastHit2D ray;
    public LayerMask layerMask;
    int enemyVel = 5;
    maceControl maceFace; 
    // Start is called before the first frame update
    void Start()
    {
        visitPoints = new GameObject[transform.childCount];
        character = GameObject.FindGameObjectWithTag("Player");
        maceFace = GetComponent<maceControl>();
        for (int i = 0; i < visitPoints.Length; i++)
        {
            visitPoints[i] = transform.GetChild(0).gameObject;
            visitPoints[i].transform.SetParent(transform.parent); 
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameObject.tag == "saw") { 
            transform.Rotate(0, 0, Random.Range(2,8));
        }
        if (gameObject.tag == "mace")
        {
            isSawPlayer(); 
            if (ray.collider.tag == "Player")
            {
                enemyVel = 8;
                maceFace.lookFront();
                maceFace.fire();

            }
            else
            {
                enemyVel = 4;
                maceFace.lookBack();
            }
        }

        enemyRoad();
    }
    void isSawPlayer()
    {
        Vector3 rayDirection = character.transform.position - transform.position;
        ray = Physics2D.Raycast(transform.position, rayDirection, 1000, layerMask);
        Debug.DrawLine(transform.position, ray.point, Color.magenta);
    }
    
    public Vector2 getDirection()
    {
        return (character.transform.position - gameObject.transform.position).normalized; 
    }
    void enemyRoad()
    {
        if (isGetDistance)
        {
            distance = (visitPoints[distanceCounter].transform.position - transform.position).normalized;
            isGetDistance = false;
        }
        float eachDistance = Vector3.Distance(transform.position, visitPoints[distanceCounter].transform.position); 
        transform.position += distance * Time.deltaTime * enemyVel; 
       
        if(eachDistance < 0.5f)
        {
            isGetDistance = true;

            if(distanceCounter  == visitPoints.Length - 1)
            {
                isForward = false;
            }
            else if(distanceCounter == 0)
            {
                isForward = true; 
            }

            if (isForward)
            {
                distanceCounter++;
            }
            else
            {
                distanceCounter--;
            }
           
        }
       
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
             Gizmos.color = Color.red;
             Gizmos.DrawWireSphere(transform.GetChild(i).transform.position,1);
        }
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i + 1).transform.position);
        }
       
    }
#endif

}

#if UNITY_EDITOR
[CustomEditor(typeof(enemyControl))]
[System.Serializable]

class sawEditor : Editor
{
    public override void OnInspectorGUI()
    {
        enemyControl script = (enemyControl)target;
        if (GUILayout.Button("produce"))
        {
            GameObject newObject = new GameObject();
            newObject.transform.parent = script.transform;
            newObject.transform.position = script.transform.position;
            newObject.name = script.transform.childCount.ToString();
        }
        EditorGUILayout.Space();
        // Bir degiskeni disariya acmak için gerekli olan kodlar
        EditorGUILayout.PropertyField(serializedObject.FindProperty("layerMask"));
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();

    }
    
}
#endif
