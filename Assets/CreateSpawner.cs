using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnerUI;
    [SerializeField] private GameObject spawnerPrefab;
    //private GameObject placingObject;
    
    public bool isPlacing { get; private set; }
    private Vector3 mousePos;
    void Awake()
    {
        spawnerUI.SetActive(false);
    }

    public void StartPlacing()
    {
        //if (!isPlacing)
        //{


            Placing();
            spawnerUI.SetActive(true);
       // }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlacing)
        {
            Ray camray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(camray, out RaycastHit hitInfo, 100f))
            {
                
                 mousePos = new Vector3 (hitInfo.point.x, 0.5f, hitInfo.point.z);
                spawnerUI.transform.position = mousePos;
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Instantiate(spawnerPrefab, mousePos, Quaternion.identity);
                //placingObject = null;
                Placing();
                spawnerUI.transform.position = Vector3.zero;
                spawnerUI.SetActive(false);
            }
        }
        
    }

    private void Placing()
    {
        isPlacing = !isPlacing;
    }
}
