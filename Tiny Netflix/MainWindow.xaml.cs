using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Forms;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tiny_Netflix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Window window;
        public static bool hidden = false;
        public static bool playing = false;

        List<Key> pressedKeys = new List<Key>();

        int tempWidth = 320;
        int tempHeight = 180;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            window = this;
            Browser.Address += new NavigatingCancelEventHandler(WebBrowser_Navigating);
            //webBrowser.Navigate("http://www.netflix.com");

            Browser.Address = "http://www.netflix.com";
            this.Width = 800;
            this.Height = 600;

            HotKeyManager.RegisterHotKey(Keys.X, KeyModifiers.Alt);
            HotKeyManager.RegisterHotKey(Keys.Q, KeyModifiers.Alt);
            HotKeyManager.RegisterHotKey(Keys.Space, KeyModifiers.Alt);
            HotKeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>(HotKeyManager_HotKeyPressed);
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == Key.Escape){
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void webBrowser_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                System.Windows.Application.Current.Shutdown();
            }

            if(playing){
                if (pressedKeys.Contains(e.Key)) return;
                pressedKeys.Add(e.Key);
                CheckKeys();
            }

        }

        private void webBrowser_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            pressedKeys.Remove(e.Key);
        }

        private void WebBrowser_Navigating(object sender, NavigatingCancelEventArgs e) {
            Console.WriteLine(e.Uri.ToString());
            if(e.Uri.ToString().Contains("/watch")){
                this.Width = tempWidth;
                this.Height = tempHeight;
                this.Topmost = true;
                this.Left = SystemParameters.PrimaryScreenWidth - this.Width;
                this.Top = (SystemParameters.PrimaryScreenHeight / 2) - (this.Height / 2);
                playing = true;
            }
            else if (e.Uri.ToString().Contains("/browse"))
            {
                this.Width = 400;
                this.Height = 600;
                this.Topmost = false;
                this.Left = (SystemParameters.PrimaryScreenWidth / 2) - (this.Width / 2);
                this.Top = (SystemParameters.PrimaryScreenHeight / 2) - (this.Height / 2);
                playing = false;
            }
        }

        static void HotKeyManager_HotKeyPressed(object sender, HotKeyEventArgs e)
        {
            if(e.Key == System.Windows.Forms.Keys.X && e.Modifiers == KeyModifiers.Alt){
                App.Current.Dispatcher.Invoke(() =>
                {
                    if (hidden)
                    {
                        if (playing)
                        {
                            window.WindowState = WindowState.Normal;
                        }
                        hidden = false;
                    }
                    else
                    {
                        //window.Topmost = false;
                        window.WindowState = WindowState.Minimized;
                        window.MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                        if(playing) hidden = true;
                    }
                });
            }
            else if(e.Key == System.Windows.Forms.Keys.Q && e.Modifiers == KeyModifiers.Alt){
                App.Current.Dispatcher.Invoke(() =>
                {
                    window.WindowState = WindowState.Normal;
                    window.Focus();
                    System.Windows.Application.Current.Shutdown();
                });
            }
            else if (e.Key == System.Windows.Forms.Keys.Space && e.Modifiers == KeyModifiers.Alt)
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    SendKeys.SendWait(" ");
                });
            }

        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if(window.WindowState == WindowState.Minimized){
                hidden = true;
            }
            else if (window.WindowState == WindowState.Normal)
            {
                hidden = false;
            }
        }

        void CheckKeys() { 
            if(pressedKeys.Contains(Key.LeftCtrl) && pressedKeys.Contains(Key.D2))
            {
                int width = (int)this.Width;
                bool switched = false;
                switch (width)
                {
                    case 320:
                        this.Width = tempWidth = 420;
                        this.Height = tempHeight = 236;
                        switched = true;
                        break;
                    case 420:
                        this.Width = tempWidth = 520;
                        this.Height = tempHeight = 293;
                        switched = true;
                        break;
                    case 520:
                        this.Width = tempWidth = 620;
                        this.Height = tempHeight = 349;
                        switched = true;
                        break;
                }
                if (switched)
                {
                    this.Left = SystemParameters.PrimaryScreenWidth - this.Width;
                    this.Top = (SystemParameters.PrimaryScreenHeight / 2) - (this.Height / 2);
                }
            }
            else if (pressedKeys.Contains(Key.LeftCtrl) && pressedKeys.Contains(Key.D1))
            {
                int width = (int)this.Width;
                bool switched = false;
                switch (width)
                {
                    case 420:
                        this.Width = tempWidth = 320;
                        this.Height = tempHeight = 180;
                        switched = true;
                        break;
                    case 520:
                        this.Width = tempWidth = 420;
                        this.Height = tempHeight = 236;
                        switched = true;
                        break;
                    case 620:
                        this.Width = tempWidth = 520;
                        this.Height = tempHeight = 293;
                        switched = true;
                        break;
                }
                if(switched){
                    this.Left = SystemParameters.PrimaryScreenWidth - this.Width;
                    this.Top = (SystemParameters.PrimaryScreenHeight / 2) - (this.Height / 2);
                }

            }
        }
    }
}
