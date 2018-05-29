using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using Facebook.MiniJSON;
using System;

// The app's access token: EAAIAEhtLLU4BAN5erI9bAyN8fbm0jcC2SsPmSKdfguOokglV6ItiLGdrMgwinLIYTswPlvf2totzers6GZAjdFT9srRx3XejSh3POsc6mri60b0PppmoFpTsas1yE90s6ZAheH86Vkan8cyUDvxZCS82wAZBfMQt4aZCXyZAetZC4b2x5oM7ZC74ZC0G7N3ILFpMZD
public class FacebookLogin : Singleton<FacebookLogin> {
   
   public GameObject cube;
   public String firstName = "";
   public String lastName = "";
   public Texture profilePic = null;
   public List<Texture> taggedPhotos = null;
   private Texture tempTexture;

   public bool ready = false;

   protected FacebookLogin() {

   }

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

   // activating app and setting up screens
   private void InitCallback ()
   {
      if (FB.IsLoggedIn) {
         // Signal an app activation App Event
         Debug.Log("Is logged in.");
         Debug.Log(Facebook.Unity.AccessToken.CurrentAccessToken);
         FB.ActivateApp();
         // Continue with Facebook SDK
         // ...
      } else {
         Debug.Log("Is not logged in.");
      }
   }

   // Pauses if not shown (a function they said to have, not super sure why)
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
      // create list of permission strings
      // https://developers.facebook.com/docs/facebook-login/permissions/v3.0
      List<string> permissions = new List<string>();
      permissions.Add("public_profile");
      permissions.Add("user_photos");
      // built in function that gives us permissions added above
      FB.LogInWithReadPermissions(permissions, AuthCallback);
   }


   // This will activate the application and set up the screen once authentication is confirmed
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
         FB.API("/me?fields=first_name", HttpMethod.GET, setUserFirstNameCallback);
         FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, setUserProfilePicCallback);
         //FB.API("/me/photos?type=tagged&fields=images", HttpMethod.GET, setUserPhotosCallback);
      }
   }

   private void checkAllParameters() {
      if (firstName != "" && profilePic != null) {
         Debug.Log("MAKING READY TRUE");
         ready = true;
      }
   }

   /***** Facebook API Callback functions! *****/
   private void setUserFirstNameCallback(IResult result) {
      if (result.Error == null) {
         firstName = (String)result.ResultDictionary["first_name"];
         Debug.Log(firstName);
      }
      else {
         Debug.Log(result.Error);
      }
      checkAllParameters();
   }

   private void setUserProfilePicCallback(IGraphResult result) {
      if (result.Error == null) {
         profilePic = result.Texture;
      }
      else {
         Debug.Log(result.Error);
      }
      checkAllParameters();
   }

   void setUserPhotosCallback(IResult result) {
      if (result.ResultDictionary.ContainsKey("error")) {
         Debug.Log("Error returned from API call!");
      }
      else if (result.ResultDictionary.ContainsKey("data")) {
         for (int i = 0; i < ((List<object>)result.ResultDictionary["data"]).Count; i++) {
            // Basically we have to go through the FB Result Dictionary and cast the right type at every level to access the next level
            Dictionary<string, object> images = (Dictionary<string, object>)((List<object>)result.ResultDictionary["data"])[i];
            List<object> imageArray = (List<object>)images["images"];
            Dictionary<string, object> smallestImg = (Dictionary<string, object>)imageArray[imageArray.Count - 1];
            string url = (string)smallestImg["source"];
            // Now that we have the Image url we want, we hand that to a function which pulls it and waits to get it back ~ then attaches it to a new Image
            StartCoroutine(getFBImage(url, setSpriteCallback));
         }
      }
   }

   // sets the image sprite to the new downloaded FB image sprite
   void setSpriteCallback(Texture tex) {
      GameObject newCube = Instantiate(cube, new Vector3(UnityEngine.Random.Range(-80, 80), UnityEngine.Random.Range(-40, 40), 100), cube.transform.rotation);
      Renderer[] ts = newCube.GetComponentsInChildren<Renderer>();
      foreach (Renderer r in ts) {
         //Renderer r = t.GetComponent<Renderer>();
         r.material.mainTexture = tex;
      }
   }

   // This will download the image URL into a sprite and then hand the callback the sprite once the sprite is ready
   IEnumerator getFBImage(string url, Action<Texture> setImageCallback) {
      WWW www = new WWW(url);
      yield return www;
      setSpriteCallback(www.texture); //www.texture.width, www.texture.height
   }

   public bool getLoggedIn() {
      return FB.IsLoggedIn;
   }
}
 