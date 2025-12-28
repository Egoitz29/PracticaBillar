using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;

public class FirebaseDataManager : MonoBehaviour
{
    public static FirebaseDataManager Instance;

    private DatabaseReference dbRef;
    private FirebaseAuth auth;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        auth = FirebaseAuth.DefaultInstance;
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    //  Guarda la puntuación de un juego
    public void SaveGameScore(int gameId, int score)
    {
        if (auth.CurrentUser == null)
        {
            Debug.LogWarning(" Usuario no autenticado, no se puede guardar");
            return;
        }

        string userId = auth.CurrentUser.UserId;

        dbRef
            .Child("users")
            .Child(userId)
            .Child("games")
            .Child("game" + gameId)
            .SetValueAsync(score);

        Debug.Log($" Guardado  Juego {gameId}: {score}");
    }

    // Leer todas las puntuaciones del usuario
    public void LoadUserScores(System.Action<int, int, int> onLoaded)
    {
        if (auth.CurrentUser == null)
        {
            Debug.LogWarning("Usuario no autenticado");
            return;
        }

        string userId = auth.CurrentUser.UserId;

        dbRef
            .Child("users")
            .Child(userId)
            .Child("games")
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (!task.IsCompleted || task.Result == null)
                {
                    onLoaded?.Invoke(0, 0, 0);
                    return;
                }

                DataSnapshot snap = task.Result;

                int g1 = snap.Child("Juego1").Exists ? int.Parse(snap.Child("Juego1").Value.ToString()) : 0;
                int g2 = snap.Child("Juego2").Exists ? int.Parse(snap.Child("Juego2").Value.ToString()) : 0;
                int g3 = snap.Child("Juego3").Exists ? int.Parse(snap.Child("Juego3").Value.ToString()) : 0;

                onLoaded?.Invoke(g1, g2, g3);
            });
    }
}
