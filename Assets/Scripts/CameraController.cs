using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public int Speed = 20;

    bool _moveForward = false;
    bool _moveBack = false;
    bool _moveLeft = false;
    bool _moveRight = false;

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
		var t = new Vector3(Input.GetAxis("Horizontal"), 0 , Input.GetAxis("Vertical")) * Time.deltaTime * Speed;
		
		transform.Translate(t, Space.World);
		
		var z = Camera.main.orthographicSize;
		
    }
}
