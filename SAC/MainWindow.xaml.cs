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


namespace AB
{
    public partial class MainWindow : Window
    {
        string hListType = "short";
        bool initialised = false;
        bool singleL=true;
        string RZHint = " Ом; - cопротивление растеканию тока единичного электрода \n";
        string THint = " лет; - cрок службы анодного заземления\n";
        string IavHint = " А; - средняя сила тока, стекающего с заземления\n";
        string RazHint = " Ом; - сопротивление растеканию заземления, состоящего из N электродов\n";
        string LHint = " м; - длина траншеи для монтажа анодного заземления\n";
        string ImaxHint = " А; - максимальный допустимый ток на анодное заземление\n";
        string resistanceNeed = "Введите удельное сопротивление грунта\n";
        string Rmax1 = "\nСопротивление растеканию анодного заземления больше рекомендуемого нормативно технической документацией";
        string Rmax2 = ".\nНеобходимо увеличить количество электродов.\n";
        string[] bigHeights= new string[] {"3,5", "4,0", "4,5"};
        string[] smallHeights = new string[] { "2,0", "2,5", "3,0"};
        string[] singleLine = new string[] {"2", "4", "6", "8", "12", "16", "20", "24"};
        string[] doubleLine = new string[] {"8", "16", "24", "32", "40", "48"};
        public MainWindow()
        {
            InitializeComponent();
            initialised = true;
            
        }
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void changeHeights(object sender, SelectionChangedEventArgs e)
        {
            if (initialised)
            {
             
                if (CBText(Aligment) == "Вертикальная" && CBText(Placement) == "Двухрядное" && hListType == "short")
                {
                    height.Items.Clear();
                    foreach (string item in bigHeights) { height.Items.Add(item); }
                    hListType = "long";
                    height.SelectedIndex = 0;
                }
                else if (hListType == "long")
                {
                    height.Items.Clear();
                    foreach (string item in smallHeights) { height.Items.Add(item); }
                    hListType = "short";
                    height.SelectedIndex = 0;
                }

                if (CBText(Placement) == "Двухрядное"&&singleL)
                {
                    N.Items.Clear();
                    foreach (string item in doubleLine) { N.Items.Add(item); }
                    singleL = !singleL;
                    N.SelectedIndex = 0;
                }
                if (CBText(Placement) == "Однорядное" && !singleL)
                {
                    N.Items.Clear();
                    foreach (string item in singleLine) { N.Items.Add(item); }
                    singleL = !singleL;
                    N.SelectedIndex = 0;
                }
            }
        }

        private void digit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(char.IsDigit(e.Text, e.Text.Length - 1)))
                e.Handled = true;
        }
        private void float_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ",")
            {
                if (!((TextBox)sender).Text.Contains(","))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }
        private void workTime_LostFocus(object sender, RoutedEventArgs e)
        {
            int value;
            if (int.TryParse(workTime.Text, out value))
            {
                if (value < 10)
                    workTime.Text = "10";
            }
        }

        private void resistance_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void solve_Click(object sender, RoutedEventArgs e)
        {
            result.Text = "";          
            double R = findR();
            double Raz=findRaz(R);
            double L= findL();
            double T= findT();
            double Imax = findImax();
            checkResult(T, Raz, Imax);
    

        }

        private void checkResult(double T, double Raz, double Imax)
        {
            if (T<Convert.ToDouble(workTime.Text))   
                result.Inlines.Add(new Bold(new Run("\nОриентировочный срок службы анодного заземления меньше проектного.\nНеобходимо увеличить количество электродов.\n")));         
            
            if (Imax<Convert.ToDouble(ProjectI.Text))
                result.Inlines.Add(new Bold(new Run("\nПроектируемый ток СКЗ больше максимально допустимого тока на анодное заземление.\nНеобходимо увеличить количество электродов.\n")));
            if (Convert.ToDouble(resistance.Text) <= 10 && Raz > 0.5)
               result.Inlines.Add(new Bold(new Run(Rmax1+ " (0.5 Ом)"+Rmax2)));
            else if (Convert.ToDouble(resistance.Text) > 10 && Convert.ToDouble(resistance.Text) <= 50 && Raz > 1)            
                result.Inlines.Add(new Bold(new Run(Rmax1 + " (0.1 Ом)" + Rmax2)));
            else if (Convert.ToDouble(resistance.Text) > 50 && Convert.ToDouble(resistance.Text) <= 100 && Raz > 1.5)
                result.Inlines.Add(new Bold(new Run(Rmax1 + " (1.5 Ом)" + Rmax2)));
            else if (Convert.ToDouble(resistance.Text) > 100 && Convert.ToDouble(resistance.Text) <= 500 && Raz > 3)
               result.Inlines.Add(new Bold(new Run(Rmax1 + " (3 Ом)" + Rmax2)));
            else if (Convert.ToDouble(resistance.Text) > 500&& Raz > 10)
                result.Inlines.Add(new Bold(new Run(Rmax1 + " (10 Ом)" + Rmax2)));
        }

        private double findImax()
        {
            result.Inlines.Add( "5) ");
            double Imax = Formulas.Imax(CBText(ABType), CBText(METype), CBDouble(N));
            result.Inlines.Add( "Imax=" + Imax + ImaxHint);          
            return Imax;
        }

        private double findL()
        {
            result.Inlines.Add( "3) ");
            double fL = Formulas.L(CBText(Aligment), CBText(ABType), CBText(METype), CBDouble(N), CBDouble(L));
            result.Inlines.Add( "L=" + fL + LHint);
            return fL;
        }

        private double findR()
        {
            result.Inlines.Add( "1) ");
            double R;
            if (CBText(ABType).Equals("Комплектный"))
            {
                if (CBText(Aligment).Equals("Вертикальная")) {
                    R = Formulas.R13(CBText(METype), Convert.ToDouble(resistance.Text), CBDouble(height));
                    result.Inlines.Add( "Rрв=" + R + RZHint);
                    if (Double.IsNaN(R)) result.Inlines.Add(new Bold(new Run(resistanceNeed)));
                    return R;
                } else {
                    R = Formulas.R11(CBText(METype), Convert.ToDouble(resistance.Text), CBDouble(height));
                    result.Inlines.Add( "Rрв=" + R + RZHint);
                    if (Double.IsNaN(R)) result.Inlines.Add(new Bold(new Run(resistanceNeed)));
                    return R;
                }
            } else {
                if (Aligment.SelectedValue.ToString().Equals("Вертикальная")) {
                    R = Formulas.R14(CBText(METype), Convert.ToDouble(resistance.Text), CBDouble(height));
                    result.Inlines.Add( "Rрв=" + R + RZHint);
                    if (Double.IsNaN(R)) result.Inlines.Add(new Bold(new Run(resistanceNeed)));
                    return R;
                } else {
                    R = Formulas.R12(CBText(METype), Convert.ToDouble(resistance.Text), CBDouble(height));
                    result.Inlines.Add( "Rрв=" + R + RZHint);
                    if (Double.IsNaN(R)) result.Inlines.Add(new Bold(new Run(resistanceNeed)));
                    return R;
                }
            }
        }

        private double findT()
        {
            result.Inlines.Add( "4) ");
            double Iav = 0.25d * Convert.ToDouble(In.Text) + 0.75d * Convert.ToDouble(Ic.Text);
            double T = Formulas.T(CBText(METype), Iav, CBDouble(N));                
            result.Inlines.Add( "T=" + T + THint);          
            result.Inlines.Add( "    Iср=" + Iav + IavHint);
            if (Double.IsInfinity(T)) result.Inlines.Add(new Bold(new Run("Введите силу тока в начале и конце периода эксплуатации\n")));
            return T;
        }
        private double findRaz(double R)
        {
            result.Inlines.Add( "2) ");         
            double Raz = R / (CBDouble(N) * Formulas.nu(CBText(Aligment), CBText(Placement), CBDouble(N), CBDouble(L)));
            result.Inlines.Add( "Raz=" + Raz + RazHint);
            if (Double.IsNaN(Raz)) result.Inlines.Add(new Bold(new Run(resistanceNeed)));
            return Raz;
        }

        private string CBText(ComboBox cb)
        {                       
            return cb.SelectedItem.ToString().Substring(38);
        }

        private double CBDouble(ComboBox cb)
        {
            return Convert.ToDouble(cb.Text);
        }


    }
}
