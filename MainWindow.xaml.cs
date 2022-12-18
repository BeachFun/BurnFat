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
using System.Windows.Controls.Primitives;

namespace BurnFat
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Calculator calc = new Calculator();
		Cache cache = new Cache();
		Story story = new Story();

		/// <summary>
		/// Счетчик кол-ва вычислений на текущей сессии
		/// </summary>
		private int calcCount = 0;

		public MainWindow()
		{
			InitializeComponent();

			foreach (int age in Enumerable.Range(6, 75))
				ageCB.Items.Add(age);
		}

        /// <summary>
        /// Вычисляет результат на основе данных в полях ввода, записывает его в элемент вывода и записывает в историю
        /// </summary>
        private void Calc()
        {
			int pulse = int.Parse(pulseTB.Text);
			int age = (int)ageCB.SelectedValue;

			calc.Change(pulse, age);
			calcCount++;

			resultTBl.Text = string.Format("{2}. Целевая зона {0} - {1} ударов в минуту\n", calc.Ngp, calc.Vgp, calcCount) + resultTBl.Text;

			story.AddEntry(pulse, age, calc.Ngp, calc.Vgp);
        }

		/// <summary>
		/// Очищает файл содержащий историю вычислений
		/// </summary>
		private void ClearStory()
        {
			MessageBoxResult result = MessageBox.Show("Вы действительно хотите очистить\nисторию вычислений?", "", MessageBoxButton.OKCancel);
			if (result == MessageBoxResult.OK)
			{
				story.Clear();
				StoryPage.Visibility = Visibility.Collapsed;
				StoryPage.Visibility = Visibility.Visible;

				StoryStackPanel.Children.Clear();
				StoryStackPanel.Children.Add(new TextBlock()
				{
					Text = "Нет записей..",
					Style = this.Resources["DescCaption"] as Style
				});
			}
		}

		/// <summary>
		/// Кнопка вычислить.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CalcBtn_Click(object sender, RoutedEventArgs e)
		{
			Calc();
		}

		/// <summary>
		/// Кнопка перехода в раздел истории
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void StoryBtn_Click(object sender, RoutedEventArgs e)
		{
			CalcPage.Visibility = Visibility.Collapsed;
			StoryPage.Visibility = Visibility.Visible;
		}

		/// <summary>
		/// Кнопка перехода в раздел вычислений
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BackBtn_Click(object sender, RoutedEventArgs e)
		{
			StoryPage.Visibility = Visibility.Collapsed;
			CalcPage.Visibility = Visibility.Visible;
		}

		/// <summary>
		/// Загружает кэш в поля ввода
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			ageCB.Text = cache["age"];
			pulseTB.Text = cache["pulse"];
		}

		/// <summary>
		/// Сохраняет данные в кэше при каждом изменений данных в полей ввода возраста
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ageCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			cache["age"] = ageCB.SelectedValue.ToString();
		}

		/// <summary>
		/// Сохраняет данные пульса в кэше при каждом изменении в поле ввода пульса
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pulseTB_KeyUp(object sender, KeyEventArgs e)
		{
			string pulse = pulseTB.Text;

			try
			{
				if (40 <= int.Parse(pulse) && int.Parse(pulse) < 110)
				{
					pulseTB.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
					pulseTip.Visibility = Visibility.Hidden;
					CalcBtn.IsEnabled = true;
					cache["pulse"] = pulse;
				}
				else
				{
					throw new Exception();
				}
			}
			catch
			{
				pulseTB.BorderBrush = Brushes.Tomato;
				pulseTip.Visibility = Visibility.Visible;
				CalcBtn.IsEnabled = false;
			}
		}

		/// <summary>
		/// Позволяет таскать окно по экрану, зажав левую кнопку мыши над верхней частью окна
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		/// <summary>
		/// Кнопка закрытия. Закрывает окно программы.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CloseBtn_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Кнопка очистки истории. Открывает диалоговое окно, чистит историю вычислений и добавляет на панель истории надпись "Нет записей.."
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BinBtn_Click(object sender, RoutedEventArgs e)
		{
			ClearStory();
		}

		/// <summary>
		/// Обновляет содержимое раздела истории вычислений при изменения свойства видимости
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void StoryPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (story.Data == null) return;

			string[] entryes = story.Data;
			var sortedStory = new List<(DateTime, List<string[]>)>();

			// Перевод записей в список записей сгруппированных по датам
			foreach (string entry in entryes)
			{
				string[] data = entry.Split(' ');

				bool flag = true;
				for(int i = 0; i < sortedStory.Count; i++) // проверка на то есть ли дата записи в сгрупированном по датам списке
				{
					if(sortedStory[i].Item1.Date == Convert.ToDateTime($"{data[4]}")) // если дата записи есть, то занесение записи в эту группу
					{
						sortedStory[i].Item2.Add(data);
						flag = false;
						break;
					}
				}

				if (flag) // случай когда записи нет в группе
				{
					sortedStory.Add( (Convert.ToDateTime($"{data[4]}"), new List<string[]>() { data }) );
				}
			}

			// Добавление записей в раздел истории вычислений
			StoryStackPanel.Children.Clear();
			foreach ((DateTime, List<string[]>) group in sortedStory)
			{
				StackPanel groupPanel = new StackPanel();
				groupPanel.Children.Add(new TextBlock()
				{
					Text = String.Format("{0}.{1}.{2}", group.Item1.Day, group.Item1.Month, group.Item1.Year),
					Style = this.Resources["EntryCaption"] as Style
				});

				foreach (string[] entry in group.Item2)
				{
					StatusBar bar = new StatusBar()
					{
						Background = Brushes.Transparent,
						Foreground = Brushes.White,
						HorizontalAlignment = HorizontalAlignment.Center
					};

					bar.Items.Add(new TextBlock()
					{
						Text = String.Format("Возраст: {0}", entry[0].ToString()),
						Style = this.Resources["Entry"] as Style
					});
					bar.Items.Add(new Separator());
					bar.Items.Add(new TextBlock()
					{
						Text = String.Format("Пульс: {0}", entry[0].ToString()),
						Style = this.Resources["Entry"] as Style
					});
					bar.Items.Add(new Separator());
					bar.Items.Add(new TextBlock()
					{
						Text = String.Format("Целевая зона: {0} - {1}", entry[2].ToString(), entry[3].ToString()),
						Style = this.Resources["Entry"] as Style
					});
					bar.Items.Add(new Separator());
					bar.Items.Add(new TextBlock()
					{
						Text = String.Format("{0}", entry[5].ToString()),
						Style = this.Resources["Entry"] as Style
					});

					groupPanel.Children.Add(bar);
				}

				StoryStackPanel.Children.Add(groupPanel);
			}
		}

		/// <summary>
		/// Переводит фокус на кнопку вычислений при нажатии на клавишу "Enter" при вводе данных в поле ввода пульса
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pulseTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
            {
				CalcBtn.Focus();
            }
        }

		/// <summary>
		/// Отлавливает нажатия и комбинации нажатий клавиш. Фокус на каком-либо элементе не имеет значения
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
			if(CalcPage.Visibility == Visibility.Visible)
            {
				switch (e.Key)
				{
					case Key.Escape:
						this.Close();
						break;
					case Key.H:
						if(e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                        {
							CalcPage.Visibility = Visibility.Collapsed;
							StoryPage.Visibility = Visibility.Visible;
						}
						break;

				}
			}
            else if(StoryPage.Visibility == Visibility.Visible)
            {
				switch (e.Key)
				{
					case Key.Escape:
						StoryPage.Visibility = Visibility.Collapsed;
						CalcPage.Visibility = Visibility.Visible;
						break;
					case Key.Delete:
						ClearStory();
						break;
					case Key.Up:
						storyViewer.LineUp();
						break;
					case Key.Down:
						storyViewer.LineDown();
						break;

				}
			}
        }
    }
}
