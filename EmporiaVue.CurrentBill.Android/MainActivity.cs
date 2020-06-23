using System.Diagnostics.CodeAnalysis;
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace EmporiaVue.CurrentBill.Android
{
	[Activity (Label = "Current Electric Bill", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Xamarin.Forms.Forms.Init (this, bundle);

			LoadApplication (new App ());
		}
	}
}

