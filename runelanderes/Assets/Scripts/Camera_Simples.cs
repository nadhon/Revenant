using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Camera_Simples : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] Camera MainCamera;
    [SerializeField] GameObject Player;
    private float initialZ;

    void Start()
    {
        MainCamera = Camera.main;
        initialZ = MainCamera.transform.position.z;
    }
    void LateUpdate()
    {
        if(PlayerPrefs.GetInt("map_active") == 0)
        {
            Vector3Int cellPosition = grid.WorldToCell(Player.transform.position);
            MainCamera.transform.position = grid.GetCellCenterWorld(cellPosition);
            Vector3 newPosition = MainCamera.transform.position;
            newPosition.z = initialZ;

            
            MainCamera.transform.position = newPosition;
        }
    }
    
}