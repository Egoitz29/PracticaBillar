using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    [Header("Estado Firebase")]
    public bool firebaseReady = false;

    public FirebaseDatabase database;

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
                Debug.Log("Firebase inicializado correctamente");

                // Inicializamos la base de datos con la URL de tu proyecto
                string databaseUrl = "https://TU-PROYECTO.firebaseio.com";
                database = FirebaseDatabase.GetInstance(databaseUrl);
            }
            else
            {
                firebaseReady = false;
                Debug.LogError("Firebase no pudo inicializarse: " + task.Result);
            }
        });
    }
}
