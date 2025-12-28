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

    // Este método se llama desde el botón
    public void LoginWithGoogle()
    {
        Debug.Log("BOTÓN PULSADO");

#if UNITY_ANDROID && !UNITY_EDITOR
        string webClientId = "PEGA_AQUÍ_TU_WEB_CLIENT_ID";

        try
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

                using (AndroidJavaClass bridge = new AndroidJavaClass("com.euneiz.googlesignin.GoogleSignInBridge"))
                {
                    bridge.CallStatic("startSignIn", activity, webClientId);
                    Debug.Log("Llamada al puente Android realizada correctamente");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al llamar al puente Android: " + e.Message);
        }
#else
        // Mensaje en editor para confirmar que el botón funciona
        Debug.Log("Simulación en Editor: botón funciona correctamente");
#endif
    }

    // Este método lo llama el puente Android cuando el login es exitoso
    public void OnGoogleSignInSuccess(string idToken)
    {
        if (string.IsNullOrEmpty(idToken))
        {
            Debug.LogError("El idToken recibido está vacío");
            return;
        }

        Debug.Log("idToken recibido: " + idToken);

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
            Debug.Log("Usuario logueado: " + user.DisplayName + " (" + user.Email + ")");
        });
    }
}
