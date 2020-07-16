using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ARAM.Utils
{
    public class ComponentWatcher : MonoBehaviour
    {
        private void Start()
        {
            DebugExtensions.DebugShowList(transform.GetAllChildren<SkinnedMeshRenderer>().Select(x => x.gameObject));
        }
    }
}
