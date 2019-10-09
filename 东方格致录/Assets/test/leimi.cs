using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leimi : MonoBehaviour
{
    [Range(0, 1)]
    public float time;
    Vector3 start;
     Vector3 end;
    // Start is called before the first frame update
    void Start()
    {
       

    }

    private void RefreshPos()
    {
         start = transform.position;
         end = Random.onUnitSphere*10;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > 1)
        {
            time = 0;
            RefreshPos();
        }
      transform.position=  Vector3.Lerp(start, end, time);
    }
}
