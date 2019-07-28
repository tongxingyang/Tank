using System.Collections;
using System.Collections.Generic;

using SpringGUI;

using UnityEngine;

public class TankRadarMap : MonoBehaviour
{

    public RadarMap map;

    

	// Use this for initialization
	void Start () {
		
	}

    public void InjectMap(float speed , float view , float project )
    {
        this.map.Inject(new List<float>()
                            {
                            speed , view , project
                            });
    }

    [ContextMenu("Test")]
    public void Test()
    {
        this.InjectMap(0.5f , 0.5f , 0.5f);
    }
}
