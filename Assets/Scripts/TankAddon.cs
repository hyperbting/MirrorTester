using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAddon : MonoBehaviour
{
    public void Init()
    {

        //ProjectileMount
        var pm = new GameObject("ProjectileMount");
        pm.transform.SetParent(this.transform);
        pm.transform.localPosition = new Vector3(0,0.412f,1.054f);

        //SpotLight
        var sl = new GameObject("SpotLight");
        sl.transform.SetParent(this.transform);
        sl.transform.localPosition = new Vector3(0.07f, 0.46f, 0.126f);

        var li = sl.AddComponent<Light>();
        li.type = LightType.Spot;
        li.range = 15;
        li.spotAngle = 80;

        //var ta = AddComponent<Tank>();
        //ta.agent = 
        //ta.animator =
        // projectileMount = 
    }
}
