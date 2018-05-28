using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class FacebookAccount : MonoBehaviour {

   // Awake function from Unity's MonoBehavior
   void Awake ()
   {
      if (!FB.IsInitialized) {
         // Initialize the Facebook SDK
         FB.Init(InitCallback, OnHideUnity);
      } else {
         // Already initialized, signal an app activation App Event
         FB.ActivateApp();
      }
   }

   private void InitCallback ()
   {
      if (FB.IsLoggedIn) {
         // Signal an app activation App Event
         Debug.Log("Is logged in.");
         FB.ActivateApp();
         // Continue with Facebook SDK
         // ...
      } else {
         Debug.Log("Is not logged in.");
      }
   }

   private void OnHideUnity (bool isGameShown)
   {
      if (!isGameShown) {
         // Pause the game - we will need to hide
         Time.timeScale = 0;
      } else {
         // Resume the game - we're getting focus again
         Time.timeScale = 1;
      }
   }

   public void FBLogin() {
      List<string> permissions = new List<string>();
      permissions.Add("public_profile");
      permissions.Add("user_photos");
      FB.LogInWithReadPermissions(permissions, AuthCallback);
   }


   private void AuthCallback (ILoginResult result) {
      if (result.Error != null) {
         Debug.Log(result.Error);
      }
      else {
         if (FB.IsLoggedIn) {
            // Signal an app activation App Event
            Debug.Log("Is logged in.");
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
         } else {
            Debug.Log("Is not logged in.");
         }
      }
   }
}
 