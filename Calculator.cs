using System;

namespace BurnFat
{
	/// <summary>
	/// Модель калькулятора для расчета пульса для сжигания жира
	/// </summary>
	public class Calculator
	{
		// Входные данные
		/// <summary>
		/// ЧСС в состоянии покоя
		/// </summary>
		private int _pulse;
		/// <summary>
		/// Возраст в годах
		/// </summary>
		private int _age;
		// ****

		// Выходные данные
		/// <summary>
		/// Нижняя граница пульса
		/// </summary>
		private double _ngp;
		/// <summary>
		/// Верхняя граница пульса
		/// </summary>
		private double _vgp;
		// ****

		// Ограничения для входных данных
		private const int minPulseLim = 40;
		private const int maxPulseLim = 109;
		private const int minAgeLim = 6;
		private const int maxAgeLim = 80;
		// ****

		// Литералы/константы формулы
		/// <summary>
		/// Максимальный пульс
		/// </summary>
		private const int maxPulse = 220;
		/// <summary>
		/// Коэффициент для вычисления нижней границы пульса
		/// </summary>
		private const double ngpRatio = 0.5;
		/// <summary>
		/// Коэффициент для вычисления верхней границы пульса
		/// </summary>
		private const double vgpRatio = 0.8;
		// ****


		/// <summary>
		/// Пульс. При изменении данного производятся вычисления результата
		/// Исключения:
		/// - ArgumentException
		/// </summary>
		public int Pulse
		{
			get => _pulse;
			set
			{
				if (minPulseLim <= value && value <= maxPulseLim)
				{
					_pulse = value;
					Calc();
				}
				else
				{
					throw new ArgumentException("Некорректное значение! Вводимое значение должно быть в диаопозоне 40..109");
				}
			}
		}
		/// <summary>
		/// Возраст. При изменении данного производятся вычисления результата
		/// </summary>
		public int Age
		{
			get => _age;
			set
			{
				if (minAgeLim <= value && value <= maxAgeLim)
				{
					_age = value;
					Calc();
				}
				else
				{
					throw new ArgumentException("Некорректное значение! Вводимое значение должно быть в диаопозоне 6..80");
				}
			}
		}
		/// <summary>
		/// Нижняя граница пульса
		/// </summary>
		public double Ngp
        {
			get => Math.Round(_ngp, 2);
        }
		/// <summary>
		/// Верхняя граница пульса
		/// </summary>
		public double Vgp
        {
			get => Math.Round(_vgp, 2);
        }


		/// <summary>
		/// Устанавливает входные данные по-умолчанию при инициализвации объекта
		/// </summary>
		public Calculator() { }
		/// <summary>
		/// Устанавливает входные данные при инициализвации объекта и сразу производит вычисления
		/// </summary>
		/// <param name="pulse"></param>
		/// <param name="age"></param>
		public Calculator(int pulse, int age)
		{
			_pulse = pulse;
			_age = age;

			Calc();
		}


		/// <summary>
		/// Изменяет входные данные и вызывает процедуру вычислений
		/// </summary>
		public void Change(int pulse, int age)
		{
			Pulse = pulse;
			Age = age;

			Calc();
		}

		/// <summary>
		/// Производит вычисления ngp и vgp на осннове входных данных записанных в полях _age и _pulse
		/// </summary>
		private void Calc()
		{
			if (_pulse != 0 && _age != 0)
			{
				_ngp = (maxPulse - _age - _pulse) * ngpRatio + _pulse;
				_vgp = (maxPulse - _age - _pulse) * vgpRatio + _pulse;
			}
		}
	}
}
