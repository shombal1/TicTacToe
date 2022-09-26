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
using System.Windows.Shapes;

namespace TicTacToe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum Cell
        { 
            NULL=0,
            Player1=1,
            Player2=2,
        }

        public Cell[,] hideMap=new Cell[3,3];
        public int playerNumber = 1;
        public Line line = new Line();
        public List<Shape> shapes = new List<Shape>();


        public void DrawEllipse(int y,int x)
        {
            shapes.Add(new Ellipse());
            int last = shapes.Count - 1;
            shapes[last].Stroke = Brushes.Red;
            shapes[last].StrokeThickness = 18;
            shapes[last].Margin = new Thickness(6, 6, 6, 6);
            Grid.SetRow(shapes[last], y);
            Grid.SetColumn(shapes[last], x);
            Grid.SetZIndex(shapes[last], 5);
            Map.Children.Add(shapes[last]);
        }
        public void DrawCross(int y, int x)
        {
            shapes.Add(new Line() { X1=0,X2=MainGride.Width/3, Y1 = 0, Y2 = MainGride.Height / 3 });
            int last = shapes.Count - 1;
            shapes[last].Stroke = Brushes.Blue;
            shapes[last].StrokeThickness = 18;
            shapes[last].Margin = new Thickness(6, 6, 6, 6);
            Grid.SetRow(shapes[last], y);
            Grid.SetColumn(shapes[last], x);
            Grid.SetZIndex(shapes[last], 5);
            Map.Children.Add(shapes[last]);
            shapes.Add(new Line() { X1 = MainGride.Width / 3, X2 = 0, Y1 = -11, Y2 = MainGride.Height / 3-11 });
            last = shapes.Count - 1;
            shapes[last].Stroke = Brushes.Blue;
            shapes[last].StrokeThickness = 18;
            shapes[last].Margin = new Thickness(6, 6, 6, 6);
            Grid.SetRow(shapes[last], y);
            Grid.SetColumn(shapes[last], x);
            Grid.SetZIndex(shapes[last], 5);
            Map.Children.Add(shapes[last]);

        }
        public MainWindow()
        {
            InitializeComponent();
            Map.Children.OfType<Button>().ToList().ForEach(button =>
            {
                button.Click += Button_Click1;
            });
            for (int y = 0; y < hideMap.GetLength(0); y++)
            {
                for (int x = 0; x < hideMap.GetLength(1); x++)
                {
                    hideMap[y, x] = Cell.NULL;
                }
            }
            line.Visibility = Visibility.Hidden;
            line.StrokeThickness = 12;
            line.Stroke = Brushes.DarkGray;
            Canvas.SetZIndex(line, 4);
            MainGride.Children.Add(line);
        }

        private bool TestWin(Cell numberPlayer)
        {
            bool value=true;
            for (int y = 0; y < hideMap.GetLength(0); y++)
            {
                for (int x = 0; x < hideMap.GetLength(1); x++)
                {
                    if (hideMap[y, x] != numberPlayer)
                    {
                        value= false;
                    }
                }
                if (value)
                {
                    double b = MainGride.Height / 6.0 + MainGride.Height / 3 * y;
                    line.Y1 = b;
                    line.X1 = 0;
                    line.X2 = MainGride.Width;
                    line.Y2 = b;
                    line.Visibility = Visibility.Visible;
                    return true;
                }
                value = true;
            }
            for (int y = 0; y < hideMap.GetLength(0); y++)
            {
                for (int x = 0; x < hideMap.GetLength(1); x++)
                {
                    if (hideMap[x, y] != numberPlayer)
                    {
                        value =false;
                    }
                }
                if (value)
                {
                    double b = MainGride.Width / 6.0 + MainGride.Width / 3 * y;
                    line.Y1 = 0;
                    line.X1 = b;
                    line.X2 = b;
                    line.Y2 = MainGride.Height;
                    line.Visibility = Visibility.Visible;
                    return true;
                }
                value = true;
            }
            for (int y = 0; y < hideMap.GetLength(0); y++)
            {
                if (hideMap[y, y] != numberPlayer)
                {
                    value= false;
                }

            }
            if (value)
            {
                line.Y1 = 0;
                line.X1 = 0;
                line.X2 = MainGride.Width;
                line.Y2 = MainGride.Height;
                line.Visibility = Visibility.Visible;
                return true;
            }
            value = true;
            for (int y = 0; y < hideMap.GetLength(0); y++)
            {
                if (hideMap[y, hideMap.GetLength(0)-1- y] != numberPlayer)
                {
                    value = false;
                }
            }
            if(value)
            {
                line.Y1 = 0;
                line.X1 = MainGride.Width;
                line.X2 = 0;
                line.Y2 = MainGride.Height;
                line.Visibility = Visibility.Visible;
                return true;
            }
            return value;
        }

        private bool TestEndGame()
        {
            for (int y = 0; y < hideMap.GetLength(0); y++)
            {
                for (int x = 0; x < hideMap.GetLength(1); x++)
                {
                    if (hideMap[y, x] == Cell.NULL)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void RestartLevel()
        {
            for (int y = 0; y < hideMap.GetLength(0); y++)
            {
                for (int x = 0; x < hideMap.GetLength(1); x++)
                {
                    hideMap[y, x] = Cell.NULL;
                }
            }
            Map.Children.OfType<Shape>().ToList().ForEach(shape => {
            Map.Children.Remove(shape);
            });
            shapes.Clear();
            line.Visibility = Visibility.Hidden;
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            Button button=(Button)sender;
            if (hideMap[Grid.GetRow(button), Grid.GetColumn(button)] == Cell.NULL)
            {
                if (playerNumber == 1)
                {
                    hideMap[Grid.GetRow(button), Grid.GetColumn(button)] = Cell.Player1;
                    DrawEllipse(Grid.GetRow(button), Grid.GetColumn(button));
                    if (TestWin(Cell.Player1))
                    {
                        MessageBox.Show($"{playerNumber}");
                        RestartLevel();
                    }
                    else
                    {
                        playerNumber = 2;
                    }
                }
                else
                {
                    DrawCross(Grid.GetRow(button), Grid.GetColumn(button));
                    hideMap[Grid.GetRow(button), Grid.GetColumn(button)] = Cell.Player2;
                    if (TestWin(Cell.Player2))
                    {
                        MessageBox.Show($"{playerNumber}");
                        RestartLevel();
                    }
                    playerNumber = 1;
                }
                if (TestEndGame())
                {
                    playerNumber = 1;
                    MessageBox.Show("Ничья");
                    RestartLevel();
                }
            }
        }

    }
}
