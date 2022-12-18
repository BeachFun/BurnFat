using System;

namespace BurnFat
{
	/// <summary>
	/// Модель истории для хранения и работы с историей вычислений
	/// </summary>
	internal class Story
	{
		/* История хранится в текстовом файле в виде текстовых данных
		 * Данные хранятся в виде: строка - данные
		 * Данные в строке имеют структуру: пульс возраст НГП ВГП ДатаВремя
		 */



		/// <summary>
		/// Имя файла содержащий историю вычислений
		/// </summary>
		private const string fileName = "CalculationStory.txt";


		/// <summary>
		/// Данные истории вычислений
		/// </summary>
		internal string[]? Data
		{
			get
			{
				string[]? data = FileManager.GetData(fileName);

				if(data == null) return null;
				if(data.Length == 0) return null;

				return data;
			}
		}


		/// <summary>
		/// Очищает историю вычислений
		/// </summary>
		internal void Clear()
		{
			FileManager.ClearFile(fileName);
		}

		/// <summary>
		/// Добавляет данные/запись в историю вычислений
		/// </summary>
		internal void AddEntry(int pulse, int age, double ngp, double vgp)
		{
			string[]? data = FileManager.GetData(fileName);
			string[] newData = new string[1];

			if (data is not null)
			{
				newData = new string[data.Length + 1];
				for (int i = 0; i < data.Length; i++)
				{
					newData[i] = data[i];
				}
			}

			newData[newData.Length - 1] = string.Format("{0} {1} {2} {3} {4}", pulse, age, ngp, vgp, DateTime.Now);

			FileManager.SaveData(fileName, newData);
		}
	}
}
