using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRoomDisplayer : MonoBehaviour
{

    private CellSelectProto cellSelectProto;
    private Vector3 selectionPosition;
    private void Awake()
    {
        cellSelectProto = GetComponent<CellSelectProto>();
    }
    private void Update()
    {
        selectionPosition = cellSelectProto.GetFramePosition();


    }
}
