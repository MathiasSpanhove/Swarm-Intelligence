using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;


namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void window_KeyDown(object sender, KeyEventArgs e)
        {
            // Disable fullscreen with F11 (if already in fullscreen)
            if (e.Key == Key.F11 && this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
            }
            // Enable fullscreen with F11
            else if(e.Key == Key.F11)
            {
                this.WindowState = WindowState.Maximized;
                this.WindowStyle = WindowStyle.None;
            }
            // Deselect Selected Boid in the simulation with Escape
            else if(e.Key == Key.Escape)
            {
                simulation.SelectedIndex = -1;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(sender);

            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DoubleValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(sender);

            Regex regex = new Regex("[^0-9]+\\.[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
