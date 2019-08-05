using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    Transform StartPos;
    Transform EndPos;
    void Update()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        transform.position = (EndPos.position + StartPos.position) / 2;
        if((StartPos.position - EndPos.position).magnitude>0)
        {
            transform.forward = StartPos.position - EndPos.position;
            transform.localScale = new Vector3(0.2f, 1, Vector3.Distance(EndPos.position, StartPos.position) / 10);
        }
    }

    public void RefreshArrow(Transform Start, Transform End)
    {
        StartPos = Start;
        EndPos = End;
        UpdateState();
    }
}
