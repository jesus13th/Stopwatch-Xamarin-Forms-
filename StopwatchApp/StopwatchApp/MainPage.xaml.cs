using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace StopwatchApp {
    public partial class MainPage : ContentPage {
        public ObservableCollection<Lap> observableLaps { get; } = new ObservableCollection<Lap>();
        private Stopwatch stopwatch;
        private bool isRunning;

        public MainPage() {
            InitializeComponent();
            stopwatch = new Stopwatch();
            isRunning = false;
            lapList.BindingContext = this;
        }
        private async void primary_Clicked(object sender, EventArgs e) {
            if (!isRunning) {
                primaryButton.Source = "pause.png";
                secondaryButton.IsVisible = true;
                secondaryButton.Source = "flag.png";
                stopwatch.Start();
                isRunning = true;
                await UpdateTimer();
            } else {
                stopwatch.Stop();
                isRunning = false;
                primaryButton.Source = "play.png";
                secondaryButton.Source = "stop.pg";
            }
        }
        private TimeSpan lastTime = TimeSpan.Zero;
        private void secondary_Clicked(object sender, EventArgs e) {
            if (!isRunning) {
                timerLabel.Text = "00:00.00";
                stopwatch.Restart();
                stopwatch.Stop();
                observableLaps.Clear();
                lastTime = TimeSpan.Zero;
                lapList.IsVisible = secondaryButton.IsVisible = false;
            } else {
                if (!lapList.IsVisible)
                    lapList.IsVisible = true;

                observableLaps.Add(
                    new Lap {
                        Index = $"{observableLaps.Count + 1:00}",
                        Time = stopwatch.Elapsed.ToString(@"mm\:ss\.ff"),
                        Delta = $"+{stopwatch.Elapsed - lastTime:mm\\:ss\\.ff}"
                    }
                );
                lastTime = stopwatch.Elapsed;
            }
        }
        private async Task UpdateTimer() {
            while (isRunning) {
                timerLabel.Text = stopwatch.Elapsed.ToString(@"mm\:ss\.ff");
                await Task.Delay(50);
            }
        }
    }
    public class Lap {
        public string Index { get; set; }
        public string Delta { get; set; }
        public string Time { get; set; }
    }
}