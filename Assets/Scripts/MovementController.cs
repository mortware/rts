using UnityEngine;
using System.Collections;
using Pathfinding;


[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    public GameObject Target; 

    public float MoveSpeed = 100.0f;                    // Default movement speed
    public float RotateSpeed = 10.0f;                   // Default rotate speed

    private GameObject _target;
    private CharacterController _characterController;
    private Transform _compass;
    private const float WaypointDistance = 0.2f;        // Distance to waypoint before moving to next one
    
    private const float TargetActiveRange = 1.0f;       // The minimum range to a target before it can be interacted with

    private bool _walking = false;
    private Seeker _seeker;

    private Path _path;
    private int _currentWaypoint;

    void Start()
    {
        _seeker = GetComponent<Seeker>();
        _characterController = GetComponent<CharacterController>();

        _compass = transform.Find("Compass");
    }

    // Update is called once per frame
    void Update()
    {
        // If target has changed since last update, calculate a new path
        if (this.Target != _target)
        {
            _target = this.Target;
            _path = null;
            _seeker.StartPath(transform.position, _target.collider.ClosestPointOnBounds(transform.position), OnPathCompleted);
        }
    }

    private void FixedUpdate()
    {
        var waypoint = _path.vectorPath[_currentWaypoint];
        var direction = (waypoint - transform.position).normalized;

        direction *= MoveSpeed * Time.fixedDeltaTime;

        Debug.Log(direction.magnitude);
        _characterController.SimpleMove(direction);

        _compass.LookAt(new Vector3(waypoint.x, transform.position.y, waypoint.z), Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, _compass.rotation, RotateSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypoint) < WaypointDistance)
            _currentWaypoint++;
    }

    private void OnPathCompleted(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
        else
        {
            Debug.Log(p.errorLog);
        }
    }
}
