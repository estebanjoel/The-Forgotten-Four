using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectOption : MonoBehaviour
{
    public List<Transform> positions = new List<Transform>();
    private Vector3 currentPosition;
    public bool horizontalMovement, verticalMovement;
    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (verticalMovement && !horizontalMovement)
        {
            moveVertically();
        }

        else if(!verticalMovement && horizontalMovement)
        {
            moveHorizontally();
        }

        else if(verticalMovement && horizontalMovement)
        {
            freeMovement();
        }
    }

    public void moveVertically()
    {
        Vector3 positionInRealTime = Input.mousePosition;
        if (positionInRealTime.y > positions[0].position.y)
        {
            currentPosition = positions[0].position;
        }

        else if (positionInRealTime.y < positions[positions.Count - 1].position.y)
        {
            currentPosition = positions[positions.Count - 1].position;
        }

        else
        {
            for (int i = 0; i < positions.Count; i++)
            {
                if (positionInRealTime.y <= positions[i].position.y)
                {
                    currentPosition = positions[i].position;
                }
            }
        }

        transform.position = currentPosition;
    }

    public void moveHorizontally()
    {

    }

    public void freeMovement()
    {
        Vector3 positionInRealTime = Input.mousePosition;
        if (positionInRealTime.y > positions[0].position.y && positionInRealTime.x < positions[0].position.x)
        {
            currentPosition = positions[0].position;
        }

        else if (positionInRealTime.y < positions[positions.Count - 1].position.y && positionInRealTime.x < positions[positions.Count - 1].position.x)
        {
            currentPosition = positions[positions.Count - 1].position;
        }

        else
        {
            for (int i = 0; i < positions.Count; i++)
            {
                positions[i].TransformPoint(positions[i].position);
                bool checkPositionInX = positionInRealTime.x >= positions[i].position.x - (positions[i].position.x * 0.5f) && positionInRealTime.x <= positions[i].position.x + (positions[i].position.x * 0.5f);
                bool checkPositionInY = positionInRealTime.y >= positions[i].position.y - (positions[i].position.y * 0.5f) && positionInRealTime.y <= positions[i].position.y + (positions[i].position.y * 0.5f);
                Debug.Log(positionInRealTime);
                Debug.Log(positions[i].position);
                if (checkPositionInX && checkPositionInY)
                {
                    currentPosition = positions[i].position;
                }
            }
        }

        transform.position = currentPosition;
    }
}
