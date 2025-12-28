using UnityEngine;
using Firebase;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    [Header("Estado Firebase")]
    public bool firebaseReady = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeFirebase();
    }

    void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                firebaseReady = true;
                Debug.Log(" Firebase inicializado correctamente");
            }
            else
            {
                firebaseReady = false;
                Debug.LogError(" Firebase no pudo inicializarse: " + task.Result);
            }
        });
    }
}
