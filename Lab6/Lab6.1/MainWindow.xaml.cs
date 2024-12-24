using System.Windows;

namespace Lab6;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    // Метод заглушка для выбора хеш-таблицы с цепочками
    private void OnChainedHashTableSelected(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Вы выбрали хеш-таблицу с цепочками.");
    }

    // Метод заглушка для выбора хеш-таблицы с открытой адресацией
    private void OnOpenAddressingHashTableSelected(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Вы выбрали хеш-таблицу с открытой адресацией.");
    }

    // Метод заглушка для выбора метода хеширования из ComboBox
    private void OnHashMethodSelected(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Вы выбрали метод хеширования.");
    }
    
    // Заглушка для обработки клика по кнопке "Сгенерировать таблицу"
    private void OnGenerateTableClicked(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Здесь будет логика генерации таблицы.");
    }
    
    // Заглушка для кнопки "Тестирование без вывода таблицы"
    private void OnTestWithoutTableClicked(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Здесь будет логика тестирования без вывода таблицы.");
    }
    
    private void AppendToOutput(string text)
    {
        OutputTextBox.Text += $"{text}\n";
        OutputTextBox.ScrollToEnd(); // Автоматическая прокрутка к последней строке
    }
}