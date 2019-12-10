using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace for easy value mapping
namespace MapValues
{

    public static class MapValuesExtension
    {

        public static float Map(this float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }


    }

}



