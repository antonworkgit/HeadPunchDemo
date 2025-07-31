using UnityEngine;

public class CopyTransformation : MonoBehaviour
{
    [SerializeField]
    private Transform _originTransform;

    private void LateUpdate()
    {
        transform.SetPositionAndRotation(_originTransform.position, _originTransform.rotation);
        //transform.localScale = _originTransform.lossyScale;
    }
}