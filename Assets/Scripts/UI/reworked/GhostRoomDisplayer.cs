using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRoomDisplayer : MonoBehaviour
{
    [SerializeField]
    private Material canBuild_overlay;
    [SerializeField]
    private Material cantBuild_overlay;

    [SerializeField]
    private GameObject[] ghostPrefabs;
    private CellSelectProto cellSelectProto;
    private Transform selectionTransform;
    private GameObject currentRoom;
    public static GhostRoomDisplayer instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }

        cellSelectProto = GetComponent<CellSelectProto>();
    }
    private void Update()
    {
        selectionTransform = cellSelectProto.GetFrameTransform();

        if(currentRoom != null)
        {
            currentRoom.transform.position = selectionTransform.position;
        }
    }

    public void DisplayGhostRoom(int roomID)
    {
        if(currentRoom == null)
        {
           currentRoom = Instantiate(ghostPrefabs[roomID], selectionTransform, false);

            cellSelectProto.SetBuildRoomState();
        }
        else
        {
            currentRoom.transform.position = selectionTransform.position;
        }
    }
    public void Display_CanBuildHere()
    {
        if(currentRoom != null)
        {
            currentRoom.GetComponent<MeshRenderer>().material = canBuild_overlay;
        }
    }
    public void Display_CantBuildHere()
    {
        if (currentRoom != null)
        {
            currentRoom.GetComponent<MeshRenderer>().material = cantBuild_overlay;
        }
    }
    public void HideGhostRoom()
    {
        if (currentRoom != null)
        {
            Destroy(currentRoom);
            currentRoom = null;
        }
    }
}
