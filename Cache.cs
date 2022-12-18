namespace BurnFat
{
	/// <summary>
	/// Предназначен для работы с кэшем программы. Например: данные, оставшиеся в полях ввода после закрытия программы.
	/// </summary>
	internal class Cache
	{
		/* Кэш сохранятся в виде текстовых данных в текстовом файле
		 * Данные хранятся в виде: строка - данные (определенные данные хранятся на определенной строке)
		 * Организация доступа к данным осуществляется по ключам
		 */



		/// <summary>
		/// Имя файла с кэшированными данными
		/// </summary>
		private const string fileName = "Cache.txt";
		/// <summary>
		/// Кол-во данных для сохранения
		/// </summary>
		private const int argCount = 2;

		/// <summary>
		/// Возвращает или сохраняет данные по ключу. Если данных нет или неизвестный ключ возвращает null.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		internal string? this[string key]
		{
			get
			{
				string[]? data = FileManager.GetData(fileName);
				if (data is null || data.Length < argCount) return null;
				switch (key)
				{
					case "pulse": return data[0];
					case "age": return data[1];
					default: return null;
				}
			}
			set
			{
				if (value is null) return;
				// извлечение существующих данных
				string[]? prevData = FileManager.GetData(fileName);
				string[] data = new string[argCount];
				for (int i = 0; i < argCount; i++)
					data[i] = string.Empty;
				if (prevData is not null)
					for (int i = 0; i < prevData.Length; i++)
						data[i] = prevData[i];
				// изменение данных
				switch (key)
				{
					case "pulse":
						data[0] = value;
						break;
					case "age":
						data[1] = value;
						break;
				}
				// сохранение измененных данных
				FileManager.SaveData(fileName, data);
			}
		}
	}
}
