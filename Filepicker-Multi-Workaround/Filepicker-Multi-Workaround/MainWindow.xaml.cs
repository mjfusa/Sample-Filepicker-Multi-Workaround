using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Filepicker_Multi_Workaround
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            string[] files = LibWrap.ShowOpenFileDialog("Select Files", Environment.GetFolderPath(Environment.SpecialFolder.Personal), "All files\0 *.*\0", true, true);

            if (files != null)
            {
                FPS.Text = "";
                foreach (var file in files)
                {
                    Debug.WriteLine("Selected file with full path: {0}", file);

                    FPS.Text += file + "\r\n";
                }
            }
        }
    }
}

