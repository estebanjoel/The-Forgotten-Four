using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform target;
    private float leftBound, rightBound, topBound, bottomBound;
    private LevelBounds myBounds;
    private Vector3 pos;
    
    // Start is called before the first frame update
    void Start()
    {
        #region Singleton
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        #endregion
        
        UpdateCameraBounds();
        UpdateTarget();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        if(!GameManager.instance.battleActive)
        {
            pos = new Vector3(target.position.x, target.position.y+ transform.up.y*3, transform.position.z);
            pos.x = Mathf.Clamp(pos.x, leftBound, rightBound);
            pos.y = Mathf.Clamp(pos.y, bottomBound, topBound);
            transform.position = pos;
        }
    }

    public void UpdateCameraBounds()
    {
        myBounds = GameObject.FindObjectOfType<LevelBounds>();
        leftBound = myBounds.LeftBound;
        rightBound = myBounds.RightBound;
        topBound = myBounds.TopBound;
        bottomBound = myBounds.BottomBound;
    }

    public void UpdateTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
