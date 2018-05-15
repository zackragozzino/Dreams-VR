using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class FacebookLogin : MonoBehaviour {

   public GameObject DialogLoggedIn;
   public GameObject DialogUsername;
   public GameObject DialogProfilePic;
   public GameObject DialogOtherPic;
   public GameObject DialogLoggedOut;

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
      toggleFBMenus(FB.IsLoggedIn);
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
         toggleFBMenus(FB.IsLoggedIn);
      }
   }

   void toggleFBMenus(bool isLoggedIn) {
      if (isLoggedIn) {
         DialogLoggedIn.SetActive(true);
         DialogLoggedOut.SetActive(false);
         FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUserName);
         FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
         FB.API("/1386208068111389?fields=picture", HttpMethod.GET, DisplayOtherPic);
      }
      else {
         DialogLoggedOut.SetActive(true);
         DialogLoggedIn.SetActive(false);
      }
   }

   void DisplayUserName(IResult result) {
      Text Username = DialogUsername.GetComponent<Text>();
      if (result.Error == null) {
         Username.text = "Hi there, " + result.ResultDictionary["first_name"] + "!";
      }
      else {
         Debug.Log(result.Error);
      }
   }

   void DisplayProfilePic(IGraphResult result) {
      if (result.Texture != null) {
         Image ProfilePic = DialogProfilePic.GetComponent<Image>();
         ProfilePic.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
      }
      else {
         Debug.Log(result.Error);
      }
   }

   void DisplayOtherPic(IGraphResult result) {
      if (result.Texture != null) {
         Image OtherPic = DialogOtherPic.GetComponent<Image>();
         OtherPic.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
      }
      else {
         Debug.Log(result.Error);
      }
   }
}
 