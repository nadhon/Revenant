using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Camera_Simples : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] GameObject MainCamera;
    [SerializeField] GameObject Player;
    private float cameraTargetPosition;

    void Start()
    {

    }
    void Update()
    {
        if (PlayerPrefs.GetInt("map_active") == 0)
        {
            Vector3Int cellPosition = grid.WorldToCell(Player.transform.position);
            MainCamera.transform.position = grid.GetCellCenterWorld(cellPosition);
            cameraTargetPosition = MainCamera.transform.position.z;
            Vector3 newPosition = grid.GetCellCenterWorld(cellPosition);
            newPosition.z = cameraTargetPosition;
            MainCamera.transform.position = newPosition;
        }
    }
    
}