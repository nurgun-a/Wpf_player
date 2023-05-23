using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Wpf_player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
   
    public partial class MainWindow : Window
    {

        DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            slider_vol.Value =100;
            mediaPlayer.Volume = slider_vol.Value;
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            slider_play.Value = mediaPlayer.Position.TotalSeconds;
            if (mediaPlayer.Source!=null && mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                statusText.Text = $"{mediaPlayer.Position:hh\\:mm\\:ss} / {mediaPlayer.NaturalDuration.TimeSpan:hh\\:mm\\:ss}";                
            }
            else
            {
                statusText.Text = "...";
            }
        }

        private void Click_Stop(object sender, RoutedEventArgs e)
        {
            slider_play.Value = 0;
            mediaPlayer.Stop();
            bt_Play.Content = "Play";
            timer.Stop();
        }

        private void Click_Clouse(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Click_Play(object sender, RoutedEventArgs e)
        {
            timer.Start();
            if (bt_Play.Content.ToString() == "Play")
            {
                mediaPlayer.Play();
                bt_Play.Content = "Pause";
                bt_Play.ToolTip = "Pause";
            }
            else if (bt_Play.Content.ToString() == "Pause")
            {
                mediaPlayer.Pause();
                bt_Play.Content = "Play";
                bt_Play.ToolTip = "Play";
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ((Slider)sender).SelectionEnd = e.NewValue;
            mediaPlayer.Volume = slider_vol.Value / 100;

        }

        private void Window_Player_MediaOpened(object sender, RoutedEventArgs e)
        {
            slider_play.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
        }

        private void Window_Player_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show("Incorrect format media file", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{

        //}

        private void slider_play_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Position = TimeSpan.FromSeconds(slider_play.Value);
        }
        private void Click_Open(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Video files (*.mp4)|*.mp4|All files (*.*)|*.*";
            if (openFile.ShowDialog() == true)
            {
                mediaPlayer.Source = new Uri(openFile.FileName);
                mediaPlayer.Play();
            }
        }
    }
}
