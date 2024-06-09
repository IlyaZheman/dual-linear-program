using System.Windows;
using DualLinearProgram.ViewModel;

namespace DualLinearProgram;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}