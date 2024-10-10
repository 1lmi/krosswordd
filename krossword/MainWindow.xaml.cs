using System;
using System.Collections.Generic;
using System.Data.Common;
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

namespace krossword
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            // Создаем 30 строк и 30 столбцов
            for (int i = 0; i < 30; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Заполняем сетку TextBox'ами
            for (int row = 0; row < 30; row++)
            {
                for (int col = 0; col < 30; col++)
                {
                    TextBox textBox = new TextBox
                    {
                        Text = "",
                        Margin = new Thickness(1),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,

                        // Устанавливаем невидимость
                        Visibility = Visibility.Collapsed
                    };

                    // Указываем позицию TextBox в сетке
                    Grid.SetRow(textBox, row);
                    Grid.SetColumn(textBox, col);

                    // Добавляем TextBox в Grid
                    grid.Children.Add(textBox);
                }
            }
        }


        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();

            int min_row = 1, max_row = 15, min_col = 1, max_col = 15;
            bool vertikal_mode = true;

            for (int i = 0; i < Convert.ToInt32(WordsCount.Text); i++)
            {
                // Определяем случайные позиции для начала слова
                int randomRow = random.Next(min_row, max_row);
                int randomColumn = random.Next(min_col, max_col);

                int charsInWord = random.Next(3, 7); // Количество символов в слове

                // Для каждого символа в слове
                for (int j = 0; j < charsInWord; j++)
                {
                    if (vertikal_mode)
                    {
                        // Проверяем, что не выходим за границы
                        if (randomRow + charsInWord < 30)
                        {
                            TextBox tb = GetTextBox(randomRow + j, randomColumn);
                            tb.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            TextBox tb = GetTextBox(randomRow - j, randomColumn);
                            tb.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        // Проверяем, что не выходим за границы
                        if (randomColumn + charsInWord < 30)
                        {
                            TextBox tb = GetTextBox(randomRow, randomColumn + j);
                            tb.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            TextBox tb = GetTextBox(randomRow, randomColumn - j);
                            tb.Visibility = Visibility.Visible;
                        }
                    }
                }

                // Обновляем границы для следующего слова
                if (vertikal_mode)
                {
                    // Для вертикального слова мы обновляем границы для пересечения
                    min_row = randomRow + charsInWord - random.Next(1,charsInWord-1); // Обновляем строку по окончанию слова
                    max_row = min_row; // Ограничиваем следующую строку пересечением
                    min_col = randomColumn; // Колонка остаётся фиксированной
                    max_col = randomColumn; // Колонка не изменяется
                }
                else
                {
                    // Для горизонтального слова аналогично обновляем границы для пересечения
                    min_col = randomColumn + charsInWord - random.Next(1, charsInWord-1); // Обновляем колонку по окончанию слова
                    max_col = min_col; // Ограничиваем следующую колонку пересечением
                    min_row = randomRow; // Строка остаётся фиксированной
                    max_row = randomRow; // Строка не изменяется
                }

                // Переключаем направление для следующего слова
                vertikal_mode = !vertikal_mode;
            }
        }

        private TextBox GetTextBox(int row, int column)
        {
            if (row < 0 || row > 31 || column < 0 || column > 31)
            {
                return null;
            }

            foreach (var element in grid.Children)
            {
                if (element is TextBox textBox)
                {
                    int elementRow = Grid.GetRow(textBox);
                    int elementColumn = Grid.GetColumn(textBox);

                    if (elementRow == row && elementColumn == column)
                    {
                        return textBox;
                    }
                }
            }

            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var element in grid.Children)
            {
                if (element is TextBox textBox)
                {
                    textBox.Visibility = Visibility.Collapsed;
                    textBox.Text = "";
                }
            }
        }
    }
}
