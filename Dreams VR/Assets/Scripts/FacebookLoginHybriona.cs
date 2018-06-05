using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Hybriona.Facebook;
using System.Collections.Generic;
using System;

public class FacebookLoginHybriona : MonoBehaviour {

   public String firstName = "";
   public String lastName = "";
   public String email = "";
   public Texture profilePic = null;
   public List<Texture> taggedPhotos = new List<Texture>(25);
   private int nextPhotoIndex = 0;
   private Texture tempTexture;

   public bool readyToUse = false;
   public String status = "";

   #region Singleton Set up
   
   private static FacebookLoginHybriona instance;
   public static FacebookLoginHybriona Instance
   {
      get
      {
         if(instance == null)
         {
            GameObject o = new GameObject("FacebookLoginHybriona");
            instance = o.AddComponent<FacebookLoginHybriona>();
            DontDestroyOnLoad(o);
         }
         return instance;
      }
   }

   #endregion


	public void Login()
	{
		if(!HybFacebook.Instance.IsLoggedIn())
		{
			HybFacebook.Instance.Login ();
			status = "Logging Process..";
		}
	}

	public Renderer profilePicRenderer;

	void Start () {
      // http://localhost:8000/fbapi/FBProcess.php
		HybFacebookConstants.SetApiURL("http://localhost:8000/fbapi/FBProcess.php","rjproz_secret_temp");
		HybFacebook.Instance.Init ("563027720744270","7e81ce67f67b22c7aebbe04be5f6d66c","email");
		HybFacebook.Instance.Init ("563027720744270","7e81ce67f67b22c7aebbe04be5f6d66c","user_photos");


		HybFacebook.Instance.onLoginFailedOrCancelled += delegate(string reason) {
			
			status = "Login Failed!\n"+reason;
		};

		HybFacebook.Instance.onValidatingUser += delegate() {
			status = "On Validating User..";
		};



		HybFacebook.Instance.onLoggedInSuccessfully += delegate() {

			status = "Login Successful!";
         GetPlayerInfo();
         GetProfilePicture();
         GetUploadedPhotos();
			//ShareVideo();
			//StartCoroutine(ShareURL());
			//StartCoroutine(LoadUploadedPhotos());
			//StartCoroutine( UploadPhoto() );
			//GetTaggableFriends();

//			string extraurl = "attached_media%5B0%5D=%7B%22media_fbid%22%3A%1358855657515083%22%7D";//attached_media={\"media_fbid\":\"1358855657515083\"}";
//			HybFacebook.Instance.ShareDialog("http://hybriona.com","Hello caption",HybFacebook.DialogType.popup,extraurl, delegate(string response) {
//			
//				Debug.Log(response);
//			});


			//PostFBFanPageSchedule();
		};
	}

   
   // I created this to try to avoid the time wait in Director.cs, will work with it later.
   // This function is called after every API call in the callback function to check that all the fields are populated
   private void checkAllParameters() {
      if (firstName != "" && profilePic != null && taggedPhotos.Count == 25) {
         Debug.Log("MAKING READY TRUE");
         readyToUse = true;
      }
   }

   #region Get Player's Info
	void GetPlayerInfo()
	{
		HybFacebook.Instance.API ("me?fields=first_name,last_name,email", HybFacebook.HTTPMethod.GET, delegate(FacebookResponse response) {
			
			if(response.error == null)
			{
				Debug.Log(response.text);
				Dictionary<string,object> res = (Dictionary<string,object> ) Hybriona.MiniJSON.Json.Deserialize( response.text );
				firstName = res["first_name"].ToString();
				lastName = res["last_name"].ToString();
            email = res["email"].ToString();
			}
			else
			{
				Debug.Log(response.error);
			}
         checkAllParameters();
		}, null);
	}
	#endregion

	#region Get Player's Profile Image
	void GetProfilePicture()
	{
		HybFacebook.Instance.API ("me/picture?type=square&height=128&width=128", HybFacebook.HTTPMethod.GET, delegate(FacebookResponse response) {
			if(response.error == null)
			{
            profilePic = response.texture;
			}
			else
			{
				Debug.Log(response.error);
			}
         checkAllParameters();
		}, null);
	}
	#endregion

	#region Get Player's uploaded pictures and name
   void GetUploadedPhotos()
	{
		HybFacebook.Instance.API ("me/photos?type=uploaded&fields=images", HybFacebook.HTTPMethod.GET, 
      delegate(FacebookResponse response) {
			if(response.error == null)
			{
            // data : [{images: [blah blah ]}, {images: [blah blah blah]}]
            List<object> data = (List<object>)((Dictionary<string, object>)Hybriona.MiniJSON.Json.Deserialize(response.text))["data"];
            for (int i = 0; i < data.Count; i++) {
               // Basically we have to go through the FB Result Dictionary and cast the right type at every level to access the next level
               Dictionary<string, object> images = (Dictionary<string, object>)data[i];
               List<object> imageArray = (List<object>)images["images"];
               Dictionary<string, object> smallestImg = (Dictionary<string, object>)imageArray[imageArray.Count - 1];
               string url = (string)smallestImg["source"];
               // Now that we have the Image url we want, we hand that to a function which pulls it and waits to get it back ~ then attaches it to a new Image
               StartCoroutine(getFBImage(url, setTextureCallback));
            }
			}
			else
			{
				Debug.Log(response.error);
			}
		}, null);
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
      setImageCallback(www.texture);
   }
	#endregion

   #region Public Accessor Functions
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

   public String getFirstName() {
      return firstName;
   }
   
   public String getLastName() {
      return lastName;
   }

   public String getEmail() {
      return email;
   }

   public Texture getProfilePicture() {
      return profilePic;
   }
   #endregion

}
