using UnityEngine;
public static class TransformExtensions
{

    public static void DestroyChildren(this Transform transform)
    {
        for (int ix = transform.childCount - 1; ix >= 0; ix--)
        {
            Transform child = transform.GetChild(ix);
            GameObject.Destroy(child.gameObject);
        }
    }

}