using OxyPlot.Series;
using OxyPlot;
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
using org.mariuszgromada.math.mxparser;
using org.mariuszgromada.math.mxparser.mathcollection;
using org.mariuszgromada.math.mxparser.parsertokens;
using org.mariuszgromada.math.mxparser.syntaxchecker;
using Expression = org.mariuszgromada.math.mxparser.Expression;
using OxyPlot.Wpf;
using OxyPlot.Axes;
using System.Linq.Expressions;
using System.Drawing;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace lab5 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private PlotModel OxyPlotModel { get; set; }
        public PlotModel MyModel { get; set; }

        public MainWindow()
        {
            this.MyModel = new PlotModel { Title = "График" };
            MyModel.InvalidatePlot(true);
            //this.MyModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
            //this.MyModel.InvalidatePlot(true);
        }

        private double leftBorder = 0;
        private double rigthBorder = 0;
        private double eps = 0;
        private double number = 0;
        private double h = 0;

        private void btCalculate_Click(object sender, RoutedEventArgs e)
        {
            //Проверка пустых значений
            if (tbInput.Text == string.Empty || tbLeftBorder.Text == string.Empty || tbRigthBorder.Text == string.Empty || tbAccuracy.Text == String.Empty)
            {
                MessageBox.Show("Пожалуйста, введите данные");
                return;
            }

            //чтение данных
            leftBorder = Convert.ToDouble(tbLeftBorder.Text);
            rigthBorder = Convert.ToDouble(tbRigthBorder.Text);
            eps = Convert.ToDouble(tbAccuracy.Text);
            number = Convert.ToDouble(tbRectangle.Text);
            h = (rigthBorder - leftBorder) / number;

            //График
            MyModel.Series.Clear();
            MyModel.InvalidatePlot(true);

            MyModel.Series.Add(GetFunction());
            MyModel.InvalidatePlot(true);

            PlotModel.Model = MyModel;

            //double result = parseIntegral(rigthBorder) - parseIntegral(leftBorder);
            parseIntegral();

            if (cbParabola.IsChecked == true || cbRectangle.IsChecked == true || cbTrapez.IsChecked == true)
            {
                if (cbParabola.IsChecked == true)
                {
                    double diff = 0;
                    int i = 1;
                    double current_numbre = number;
                    do
                    {
                        diff = Math.Abs(Parabola(current_numbre) - Parabola(current_numbre + i));
                        current_numbre++;
                    } while (diff > eps);
                    lbParabol.Content = Parabola(current_numbre + i).ToString();
                }
                if (cbTrapez.IsChecked == true)
                {
                    double diff = 0;
                    double current_numbre = number;
                    int i = 1;
                    do
                    {
                        diff = Math.Abs(Trapez(current_numbre) - Trapez(current_numbre + i));
                        current_numbre++;
                    } while (diff > eps);

                    lbTrapez.Content = Trapez(current_numbre + i).ToString();
                }
                if (cbRectangle.IsChecked == true)
                {
                    double diff = 0;
                    double current_numbre = number;
                    int i = 1;
                    do
                    {
                        
                        //diff = Math.Abs(Rectangle(number * i) - Rectangle(number * (i + 1)));
                        double rect1 = Rectangle(current_numbre);
                        double rect2 = Rectangle(current_numbre + i);
                        diff = Math.Abs(rect2) - Math.Abs(rect1);
                        current_numbre++;
                    } while (diff > eps);

                    lbRectangle.Content = Rectangle(current_numbre + i).ToString();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите метод(ы)");
                return;
            }
        }
        #region Методы
        private double Rectangle(double number)
        {
            double result;
            result = 0;

            for (double i = 1; i < number; i++)
            {
                //x = leftBorder + i * h;
                result += h * parseMath(leftBorder + i * h);
            }

            return result;
        }
        private double Trapez(double number)
        {
            double result = parseMath(leftBorder) + parseMath(rigthBorder);

            for (double i = 1; i < number - 1; i++)
            {
                result += 2 * parseMath(leftBorder + i * h);
            }
            result *= h / 2;

            return result;
        }

        private double Parabola(double number)
        {
            double result = parseMath(leftBorder) + parseMath(rigthBorder);
            double k;

            for (double i = 1; i < number - 1; i++)
            {
                k = 2 + 2 * (i % 2);
                result += k * parseMath(leftBorder + i * h);
            }
            result *= h / 3;

            return result;
        }
        #endregion

        #region парсинг
        public FunctionSeries GetFunction()
        {
            double n = Convert.ToDouble(tbRigthBorder.Text);
            FunctionSeries serie = new FunctionSeries();

            for (double x = Convert.ToDouble(tbLeftBorder.Text); x < n; x += 0.01)
            {
                DataPoint data = new DataPoint(x, parseMath(x));

                serie.Points.Add(data);
            }
            return serie;
        }

        private double parseIntegral()
        {
            Expression expression = new Expression("int(" + tbInput.Text.ToString() + ", x, " + leftBorder.ToString() + ", " + rigthBorder.ToString() + ")");
            return expression.calculate();
        }

        private double parseMath(double point)
        {
            Argument x = new Argument("x");
            x.setArgumentValue(point);
            Expression expression = new Expression(tbInput.Text.ToString(), x);
            return expression.calculate();
        }
        #endregion

    }

}
