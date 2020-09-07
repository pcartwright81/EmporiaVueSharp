using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using JetBrains.Annotations;
using Xamarin.Forms.Platform.Android;

namespace EmporiaVue.CurrentBill.Android
{
	[Activity (Label = "Current Electric Bill", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [UsedImplicitly]
    public class MainActivity : FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Xamarin.Forms.Forms.Init (this, bundle);

			LoadApplication (new App ());
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}

