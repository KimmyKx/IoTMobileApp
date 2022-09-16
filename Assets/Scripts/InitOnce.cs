using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitOnce : MonoBehaviour
{
    private void Awake()
    {
        Invoke(nameof(HideLoading), 3f);
    }

    private void HideLoading() => UIManager.Instance.InitOnce();
}
