using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.CoordinatorLayout.Widget;
using AndroidX.Core.View;
using AndroidX.Preference;
using Google.Android.Material.AppBar;
using System;

namespace com.companyname.SplashScreen1
{
    [Activity(Label = "@string/app_name",  MainLauncher = true) /* No theme set here */]
    public class MainActivity : AppCompatActivity, IOnApplyWindowInsetsListener
    {
        private ISharedPreferences sharedPreferences;
        private RadioGroup themeRadioGroup;
        
        private UiNightMode uiNightMode;
        private UiNightMode? previousUiNightMode = null;

        #region OnCreate
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AndroidX.Core.SplashScreen.SplashScreen.InstallSplashScreen(this);

            base.OnCreate(savedInstanceState);

            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            AppBarLayout appBar = FindViewById<AppBarLayout>(Resource.Id.app_bar);
            themeRadioGroup = FindViewById<RadioGroup>(Resource.Id.theme);
            
            SetSupportActionBar(toolbar);
            WindowCompat.SetDecorFitsSystemWindows(this.Window, false);
            ViewCompat.SetOnApplyWindowInsetsListener(appBar, this);

            sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(this);
            uiNightMode = (UiNightMode)sharedPreferences.GetInt("night_mode", (int)UiNightMode.Auto);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                themeRadioGroup.CheckedChange += ThemeRadioGroup_CheckedChange;

                // Check the correct radio button (System Default, Light or Dark) of the themeRadioGroup based on the preference setting of nightMode
                // There would be no radioButton checked without this check
                // We want it normally to default to System Default.
                int radioButtonId = GetCheckedRadioButtonId(uiNightMode);

                if (themeRadioGroup.CheckedRadioButtonId != radioButtonId)
                    themeRadioGroup.Check(radioButtonId);
            }
            else
                themeRadioGroup.Visibility = ViewStates.Gone;

        } 
        #endregion

        #region ThemeRadioGroup_CheckedChange
        private void ThemeRadioGroup_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            uiNightMode = GetNightModeOfCheckedRadioButton(e.CheckedId);
            if (previousUiNightMode == null)
                previousUiNightMode = uiNightMode;

            if (previousUiNightMode != uiNightMode)
            {
                //Recreate();   // Don't appear to need this. Works with or without.
                UpdateNightMode(uiNightMode);
            }
        }
        #endregion

        #region GetCheckedRadioButtonID
        private int GetCheckedRadioButtonId(UiNightMode nightMode)
        {

            //UiNightMode.Auto    0
            //UiNightMode.No      1
            //UiNightMode.Yes     2
            //UiNightMode.Custom  3;

            return nightMode switch
            {
                UiNightMode.Auto => Resource.Id.theme_system,
                UiNightMode.No => Resource.Id.theme_light,
                UiNightMode.Yes => Resource.Id.theme_dark,
                _ => Resource.Id.theme_system,
            };
        }
        #endregion

        #region GetNightModeOfCheckedRadioButton
        private UiNightMode GetNightModeOfCheckedRadioButton(int checkedRadioButtonId)
        {
            return checkedRadioButtonId switch
            {
                Resource.Id.theme_system => UiNightMode.Auto,
                Resource.Id.theme_light => UiNightMode.No,
                Resource.Id.theme_dark => UiNightMode.Yes,
                _ => throw new Exception("Unknown view Id: " + checkedRadioButtonId.ToString()),
            };
        }
        #endregion

        #region UpdateNightMode
        private void UpdateNightMode(UiNightMode uiNightMode)
        {
            UiModeManager uiModeManager = GetSystemService("uimode") as UiModeManager;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
                uiModeManager?.SetApplicationNightMode((int)uiNightMode);  // Only avaialable in Android 12 and above.
            
            ISharedPreferencesEditor editor = sharedPreferences.Edit();
            editor.PutInt("night_mode", (int)uiNightMode).Apply();
            editor.Commit();
        }
        #endregion

        #region OnApplyWindowInsets
        public WindowInsetsCompat OnApplyWindowInsets(View v, WindowInsetsCompat insets)
        {
            if (v is AppBarLayout)
            {
                AndroidX.Core.Graphics.Insets statusBarsInsets = insets.GetInsets(WindowInsetsCompat.Type.StatusBars());
                v.SetPadding(v.PaddingLeft, v.PaddingTop + statusBarsInsets.Top, v.PaddingRight, v.PaddingBottom);
            }
            return insets;
        }
        #endregion

        #region OnBackPressed
        public override void OnBackPressed()
        {
            base.OnBackPressed();

            // Convenience for testing. Since this is a demo app, we kill the app when it is closed to ensure we see a cold start for every launch
            // REMOVE for a normal app.
            FinishAndRemoveTask();
            Process.KillProcess(Process.MyPid());
        }
        #endregion

        
    }
}