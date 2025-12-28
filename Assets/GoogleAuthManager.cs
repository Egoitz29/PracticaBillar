using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;

public class GoogleAuthManager : MonoBehaviour
{
    private FirebaseAuth auth;

    [Header("Paneles a mostrar tras login")]
    public GameObject panel1;
    public GameObject panel2;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        // Ocultar los paneles al inicio
        if (panel1 != null) panel1.SetActive(false);
        if (panel2 != null) panel2.SetActive(false);
    }

    // Método llamado desde el botón
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
        Debug.Log("Simulación en Editor: botón funciona correctamente");
        OnLoginSuccess(); // Para probar en Editor
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

        var credential = GoogleAuthProvider.GetCredential(idToken, null);

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

            var user = task.Result;
            Debug.Log("Usuario logueado: " + user.DisplayName + " (" + user.Email + ")");

            // Mostrar paneles tras login exitoso
            OnLoginSuccess();
        });
    }

    private void OnLoginSuccess()
    {
        if (panel1 != null) panel1.SetActive(true);
        if (panel2 != null) panel2.SetActive(true);
        Debug.Log("Paneles activados tras login");
    }
}
