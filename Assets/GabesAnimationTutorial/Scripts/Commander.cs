using UnityEngine;
using System.Collections.Generic;
public class Commander : MonoBehaviour
{

    [SerializeField] private List<Monster> controlledMonsters = new List<Monster>() ;
    private List<Monster> selectedMonsters = new List<Monster>();


    private float _rotationDirection;
    private Vector3 _moveDirection;
    private Vector3 lastDestination;
    [SerializeField] private CreateSpawner create;
    private Monster selectedTarget;
    private bool isFocused = false;
    // Start is called before the first frame update
    void Start()
    {
        InputManager.Initialize(this);
        selectedMonsters.Clear();
    }

    private void Update()
    {
        Transform myTransCached  = transform;
        myTransCached.Rotate(Vector3.up,Time.deltaTime *   Settings.Instance.MouseRotateSens * _rotationDirection );
        myTransCached.position += transform.rotation * (Time.deltaTime * Settings.Instance.MouseMoveSense * _moveDirection);
    }

    public void Attack(Ray camToWorldRay)
    {
        Debug.DrawRay(camToWorldRay.origin, camToWorldRay.direction * 100, Color.red,1);
    }

    

    public void Interact(Ray camToWorldRay)
    {
        if (!create.isPlacing)
        {
            Debug.DrawRay(camToWorldRay.origin, camToWorldRay.direction * 100, Color.blue, 1);

            /* if (Physics.Raycast(camToWorldRay, out RaycastHit hit, 100, StaticUtilities.PlayerLayer))
             {

                 Debug.Log("PlayerValid");
                 selectedTarget = hit.collider.gameObject.GetComponent<Monster>();
                 selectedTarget.ChangeAuraState();
                 isFocused = true;



             }
             else if(Physics.Raycast(camToWorldRay, out RaycastHit bop, 100, StaticUtilities.MoveLayerMask))
             {
                 if (isFocused && selectedTarget  != null)
                 {
                     selectedTarget.MoveToTarget(bop.point);
                     selectedTarget.ChangeAuraState();
                     isFocused = false;
                 }
                else if (!isFocused)
                 {
                     MoveTo(bop.point);
                 }
                 Debug.Log("groundLayer");
             }*/

            if (Physics.Raycast(camToWorldRay, out RaycastHit hit, 100))
            {
                Debug.Log("hitSomething");
                if(((1<<hit.collider.gameObject.layer) & StaticUtilities.PlayerLayer) != 0)
                {
                    Debug.Log("PlayerValid");
                    selectedTarget = hit.collider.gameObject.GetComponent<Monster>();

                    if (selectedTarget.isSelected) return;

                    selectedTarget.isSelected = true;
                    selectedMonsters.Add(selectedTarget);
                    selectedTarget.ChangeAuraState();
                    isFocused = true;

                } else if(((1<<hit.collider.gameObject.layer) & StaticUtilities.MoveLayerMask) !=0)
                {
                    if (isFocused && selectedTarget != null)
                    {
                        MoveTo(hit.point, selectedMonsters);
                        foreach(Monster I in selectedMonsters)
                        {
                            I.ChangeAuraState();
                            I.isSelected = false;
                        }
                        selectedMonsters.Clear();
                        isFocused = false;
                    }
                    else if (!isFocused)
                    {
                        MoveTo(hit.point, controlledMonsters);
                    }
                }
            }
        }
    }
    public void MoveTo(Vector3 point, List<Monster> list)
    {
      //  if (!create.isPlacing)
        //{
           // Debug.DrawRay(camToWorldRay.origin, camToWorldRay.direction * 100, Color.blue, 1);

           // if (!Physics.Raycast(camToWorldRay, out RaycastHit hit, 100, StaticUtilities.MoveLayerMask))
                //return;

            lastDestination = point;

            foreach (Monster monster in list)
            {
                monster.MoveToTarget(point);
            }
       // }
    }

    public void AddMinion(Monster newMinion)
    {
        controlledMonsters.Add(newMinion);
        if(lastDestination != Vector3.zero)
        {
            Debug.Log(lastDestination);
            newMinion.MoveToTarget(lastDestination);
        }
       
    }
    

    
    public void SetRotationDirection(float direction)
    {
        _rotationDirection = direction;
    }

    public void SetMoveDirection(Vector2 direction)
    {
        _moveDirection.x = direction.x;
        _moveDirection.z = direction.y;
    }
}
