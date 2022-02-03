using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
 
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject triggerObject = other.gameObject;

        if (triggerObject.name.Contains("PIECE"))
        {
            LevelManager.instance.currentPiece = triggerObject;
            LevelManager.instance.currentPieceNumber += 1;

            float pieceLenght = triggerObject.transform.localScale.z;
            LevelManager.instance.distanceLeftOnPiece = pieceLenght;
            LevelManager.instance.currentPieceLenght = pieceLenght;
        }

    }
}
