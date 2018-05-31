using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using Facebook.MiniJSON;
using System;

// The app's test user access token (Dorothy):
// EAAIAEhtLLU4BADBGHfCK8mZC2w8oZBxwr2p6zb7ZBeAJTx3o3kdWRudoZBukdXMZBYQaRr3woEeB0WUj1NVY7Jn3vZCEVwD07xZARIaSvHYW5ACiXkEVPzwfz6Vcb3E1ZAretVckI7kyydejJ7ey9hMmwEwBQlhVPP7nlHzyWZCZAkLW76QAqx5Tl54K9OFsiVB74OQkgQAnPqnOb0aSi42V8cIuasGfVJsZCpcCNNqm6hGmvWLNvhE75ve
public class FacebookLogin : Singleton<FacebookLogin> {
   
   public String firstName = "";
   public String lastName = "";
   public Texture profilePic = null;
   public List<Texture> taggedPhotos = new List<Texture>(25);
   private int nextPhotoIndex = 0;
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

   // This will prompt the user to login, using the Facebook SDK's built in dialog
   // Here you can add the desired permissions
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
         // This makes API calls to populate this singleton object with user info
         FB.API("/me?fields=first_name", HttpMethod.GET, setUserFirstNameCallback);
         FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, setUserProfilePicCallback);
         FB.API("/me/photos?type=uploaded&fields=images", HttpMethod.GET, setUserPhotosCallback);
      }
   }

   // I created this to try to avoid the time wait in Director.cs, will work with it later.
   // This function is called after every API call in the callback function to check that all the fields are populated
   private void checkAllParameters() {
      if (firstName != "" && profilePic != null && taggedPhotos.Count == 25) {
         Debug.Log("MAKING READY TRUE");
         ready = true;
      }
   }

   /***** Facebook API Callback functions! *****/
   private void setUserFirstNameCallback(IResult result) {
      if (result.Error == null) {
         firstName = (String)result.ResultDictionary["first_name"];
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

   // This looks scary but just goes through each image in the result dictionary, downloads it, and adds it to the taggedPhotos list.
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
            StartCoroutine(getFBImage(url, setTextureCallback));
         }
      }
   }

   // sets the image sprite to the new downloaded FB image sprite
   void setTextureCallback(Texture tex) {
      Debug.Log("Adding a photo...");
      taggedPhotos.Add(tex);
      checkAllParameters();
   }

   // This will download the image URL into a sprite and then hand the callback the sprite once the sprite is ready
   IEnumerator getFBImage(string url, Action<Texture> setImageCallback) {
      WWW www = new WWW(url);
      yield return www;
      setImageCallback(www.texture); //www.texture.width, www.texture.height
   }

   // returns status of logged in
   public bool getLoggedIn() {
      return FB.IsLoggedIn;
   }

   public Texture getNextUserPhoto() {
      Texture photo;
      // This just cycles through the same 25 photos
      if (nextPhotoIndex >= taggedPhotos.Count) {
         nextPhotoIndex = 0;
         // taggedPhotos.Clear();
         // nextPhotoIndex = 0;
         // FB.API("/me/photos?type=tagged&fields=images", HttpMethod.GET, setUserPhotosCallback);
         // StartCoroutine(Wait(10));
      }    
      photo = taggedPhotos[nextPhotoIndex];
      nextPhotoIndex++;
      return photo;
   }

   IEnumerator Wait(int seconds) {
      yield return new WaitForSeconds(seconds);
   }
}
 