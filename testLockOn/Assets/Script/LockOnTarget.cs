using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnTarget : MonoBehaviour
{
    StarterAssetsInputs input;
    public Collider[] targets;
    public float radius = 20f;
    [SerializeField] List<Enemy> targetEnemy = new List<Enemy>();
    [SerializeField] GameObject MainCamera;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] Transform lockOnImage;
    [SerializeField] float lookAtSmoothing;
    float minAngle = -70;
    float maxAngle = 70;
    public Vector3 currentTargetPosition;

    public Enemy currentTarget;

    public bool isLockOn = false;
 
    void Start()
    {
        input = GetComponent<StarterAssetsInputs>();
        lockOnImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(isLockOn)
        {
            if(isTargetRange())
            {
                LookAtTarget();
            }
            else
            {
                ResetTarget();
            }
        }

        if (input.lockOn)
        {
            searchTarget();
            input.lockOn = false;
        }
    }

    public void searchTarget()
    {
        Collider [] findTarget = Physics.OverlapSphere(transform.position, radius, targetLayer);

        if (findTarget.Length <= 0)
            return;

        for (int i = 0; i < findTarget.Length; i++)
        {
            Enemy target = findTarget[i].GetComponent<Enemy>();

            if (target != null)
            {
                Vector3 targetVector = (target.transform.position - transform.position);
                float viewAngle = Vector3.Angle(targetVector, MainCamera.transform.forward);

                if (viewAngle > minAngle && viewAngle < maxAngle)
                {
                    RaycastHit hit;

                    if (Physics.Linecast(transform.position, target.lockOnTarget.transform.position, out hit, targetLayer))
                    {
                        targetEnemy.Add(target);
                    }
                }
                else
                {
                    ResetTarget();
                }
            }
        }

        LockOn();
    }

    private void LockOn()
    {
        float shortDistance = Mathf.Infinity;

        for(int i = 0; i < targetEnemy.Count; i++)
        {
            if (targetEnemy[i] != null)
            {
                float distanceFromTarget = Vector3.Distance(targetEnemy[i].transform.position, transform.position);

                if (distanceFromTarget < shortDistance)
                {
                    shortDistance = distanceFromTarget;
                    currentTarget = targetEnemy[i];
                }
            }
            else
            {
                ResetTarget();
            }
        }

        if(currentTarget != null)
        {
            FindTarget();
        }
    }

    private void LookAtTarget()
    {
        if(currentTarget == null)
        {
            ResetTarget();
            return;
        }

        currentTargetPosition = currentTarget.lockOnTarget.transform.position;
        lockOnImage.position = Camera.main.WorldToScreenPoint(currentTargetPosition);

        Vector3 dir = (currentTargetPosition - transform.position).normalized;
        dir.y = transform.position.y;

        transform.forward = Vector3.Lerp(transform.forward, dir, lookAtSmoothing * Time.deltaTime);
    }

    private bool isTargetRange()
    {
        float distance = (transform.position - currentTargetPosition).magnitude;

        if (distance > radius)
            return false;
        else
            return true;
    }

    private void FindTarget()
    {
        isLockOn = true;
        lockOnImage.gameObject.SetActive(true);
    }    

    private void ResetTarget()
    {
        isLockOn = false;
        targetEnemy.Clear();

        lockOnImage.gameObject.SetActive(false);
    }
}
