using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using AndroidX.Preference;
using System;

namespace com.companyname.SplashScreen1
{
    [Application]
    public class SplashScreen1 : Application
    {
        #region Ctor
        public SplashScreen1(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
           
        }
        #endregion

        public override void OnCreate()
        {
            base.OnCreate();

            // UiNightMode.Auto = 0
            // UiNightMode.No = 1
            // UiNightMode.Yes = 2
            // UiNightMode.Custom = 3
            
            //ISharedPreferences sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            //UiNightMode uiNightMode = (UiNightMode)sharedPreferences.GetInt("night_mode", (int)UiNightMode.Auto);


            ////if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
            ////    AppCompatDelegate.DefaultNightMode = uiNightMode == UiNightMode.Yes ? AppCompatDelegate.ModeNightYes : AppCompatDelegate.ModeNightNo;

            //switch (uiNightMode)
            //{
            //    case UiNightMode.Yes:
            //        AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightYes;
            //        break;
            //    case UiNightMode.No:
            //        AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
            //        break;
            //    case UiNightMode.Auto:
            //    case UiNightMode.Custom:
            //        AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightFollowSystem;
            //        break;
            //    default:
            //        break;
            //}
        }

    }
}
    
