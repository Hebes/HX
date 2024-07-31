using UnityEngine;

public class CameraManager
{
    public GameObject GameObject
    {
        get
        {
            GameObject result;
            if ((result = _gameObject) == null)
                result = _gameObject = Camera.gameObject;

            return result;
        }
    }

    public Transform Transform
    {
        get
        {
            Transform result;
            if ((result = _transform) == null)
                result = _transform = Camera.transform;

            return result;
        }
    }

    public Camera Camera
    {
        get
        {
            Camera result;
            if ((result = _camera) == null)
                result = _camera = Camera.main;
            return result;
        }
    }

    public CameraController Controller => SingletonMono<CameraController>.Instance;

    public T GetComponent<T>() => GameObject.GetComponent<T>();

    public T AddComponent<T>() where T : Component => GameObject.AddComponent<T>();

    public bool IsInView(GameObject go)
    {
        Vector3 vector = Camera.WorldToViewportPoint(go.transform.position);
        return vector.x > 0f && vector.x < 1f;
    }

    private Camera _camera;

    private GameObject _gameObject;

    private Transform _transform;
}