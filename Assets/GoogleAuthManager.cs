using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;

public class GoogleAuthManager : MonoBehaviour
{
    private FirebaseAuth auth;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    // Llamar a este método desde un botón
    public void LoginWithGoogle()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            using (AndroidJavaClass bridge = new AndroidJavaClass("com.euneiz.googlesignin.GoogleSignInBridge"))
            {
                bridge.CallStatic("startSignIn", activity);
            }
        }
#else
        Debug.Log("Login con Google solo funciona en Android real");
#endif
    }

    // ESTE método lo llama el puente Android
    public void OnGoogleSignInSuccess(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Login con Google cancelado");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("Error en login con Google: " + task.Exception);
                return;
            }

            FirebaseUser user = task.Result;
            Debug.Log("Usuario logueado: " + user.Email);
        });

    }
}
