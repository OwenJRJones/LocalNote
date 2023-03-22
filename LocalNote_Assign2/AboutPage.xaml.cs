using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LocalNote_Assign2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        public AboutPage()
        {
            this.InitializeComponent();

            //Retrieve dynamic app info from package manifest
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;
            String appName = package.DisplayName;
            String publisher = package.PublisherDisplayName;

            aboutApp.Text = appName + " " + String.Format("version {0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision)
                + " © 2022 " + publisher;

            description.Text = "This app was designed and built for PROG2500 focusing on command binding and async functionality using MVVM in UWP.";

            lastUpdate.Text = "2022-04-05";
        }

        //Define methods for page navigation
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            base.OnNavigatedTo(e);
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs backRequestedEventArgs)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
            backRequestedEventArgs.Handled = true;
        }
    }
}
