using System.Collections;
using UnityEngine;

public class Stream : MonoBehaviour
{
    LineRenderer lineRenderer = null;
    ParticleSystem splashParticle = null;

    Coroutine pourRoutine = null;
    Vector3 targetPosition = Vector3.zero;

    public Transform endPosition = null;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        splashParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {

        MoveToPosition(0, transform.position);
        MoveToPosition(1, transform.position);
        // MoveToPosition(0, endPosition.position);
        //MoveToPosition(1, endPosition.position);
    }

    public void Begin()
    {
        StartCoroutine(UpdateParticle());
        pourRoutine = StartCoroutine(BeginPour());
    }

    private IEnumerator BeginPour()
    {
        while (gameObject.activeInHierarchy)
        {
            targetPosition = FindEndPoint();

            MoveToPosition(0, transform.position);
            //MoveToPosition(0, endPosition.position);
            AnimateToPosition(1, targetPosition);
            // AnimateToPosition(1, endPosition.position);

            yield return null;
        }
    }

    public void End()
    {
        if (pourRoutine != null) StopCoroutine(pourRoutine);
        pourRoutine = StartCoroutine(EndPour());
    }

    IEnumerator EndPour()
    {
        while (!HasReachedPosition(0, targetPosition))
        {
            AnimateToPosition(0, transform.position);
            AnimateToPosition(1, transform.position);
            //AnimateToPosition(0, endPosition.position);
            // AnimateToPosition(1, endPosition.position);
            yield return null;
        }
        //gameObject.SetActive(false);
        // Destroy(gameObject);
    }

    Vector3 FindEndPoint()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        float distance = Mathf.Abs(endPosition.position.y - transform.position.y);
        Physics.Raycast(ray, out hit, distance);
        Vector3 endPoint = hit.collider ? hit.point : ray.GetPoint(distance);

        return endPoint;
    }
    private void MoveToPosition(int index, Vector3 targetPosition)
    {
        lineRenderer.SetPosition(index, targetPosition);
    }

    void AnimateToPosition(int index, Vector3 targetPosition)
    {
        Vector3 currentPosition = lineRenderer.GetPosition(index);
        Vector3 newPosition = Vector3.MoveTowards(currentPosition, targetPosition, Time.deltaTime * 1.75f);
        lineRenderer.SetPosition(index, newPosition);
    }
    bool HasReachedPosition(int index, Vector3 targetPosition)
    {
        Vector3 currentPosition = lineRenderer.GetPosition(index);
        return currentPosition == targetPosition;
    }

    IEnumerator UpdateParticle()
    {
        while (gameObject.activeSelf)
        {
            splashParticle.transform.position = targetPosition;
            bool isHitting = HasReachedPosition(1, targetPosition);
            splashParticle.gameObject.SetActive(isHitting);

            yield return null;
        }
    }

}
