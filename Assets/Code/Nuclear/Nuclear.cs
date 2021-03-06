using System.Collections.Generic;
using System.ComponentModel;

public class Nuclear : IMachineType, INotifyPropertyChanged
{
	private bool _isOverloaded;
	private float _controlRodDepth;
	public string Name { get; set; }
	public int Cost { get; set; }
	public bool IsPoweredOn { get; private set; }
	public bool IsOverloaded
	{
		get { return _isOverloaded; }
		set
		{
			_isOverloaded = value;
			OnPropertyChanged("IsOverloaded");
			OnPropertyChanged("CanAdjustRequestedOutput");
		}
	}

	public float MaxTemperature { get; set; }
	public float MaxOutputPerSecond { get; private set; }
	public bool CanAdjustRequestedOutput { get { return !IsOverloaded; } }
	public float Output { get; set; }
	public float MinOutput { get; private set; }
	public float OverloadOutput { get; private set; }
	public double OutsideLimitAccumulated { get; set; }

	public float ControlRodDepth
	{
		get { return _controlRodDepth; }
		set
		{
			_controlRodDepth = value;
			OnPropertyChanged("ControlRodDepth");
		}
	}
	public float ControlRodEffect { get; set; }

	public List<FuelRod> FuelRods { get; set; }
	public float Temperature { get; set; }
	public Range NoReactionUnit { get; set; }
	public Range OverHeatUnit { get; set; }
	public Range TemperatureUnit { get { return new Range(0, Temperature/MaxTemperature); } }
	public bool IsOutsideSafeLimit { get { return OutsideLimitAccumulated > 0.0001f; } }

	public Nuclear()
	{
		Name = "Nuclear";
		FuelRods = new List<FuelRod>
		{
			new FuelRod(),
			new FuelRod(),
			new FuelRod(),
			new FuelRod(),
			new FuelRod(),
			new FuelRod(),
			new FuelRod(),
			new FuelRod(),
			new FuelRod()
		};
		NoReactionUnit = new Range(0f, 0.3f);
		OverHeatUnit = new Range(0.7f, 1f);
		MaxTemperature = FuelRod.BaseTemperature*FuelRods.Count;
		MaxOutputPerSecond = FuelRod.MaxRodOutput*FuelRods.Count;
		ControlRodDepth = 0.5f;
		TogglePower();
	}

	public void TogglePower()
	{
		IsPoweredOn = !IsPoweredOn;
		ControlRodEffect = 0;
	}

	public void Repair()
	{
		throw new System.NotImplementedException();
	}

	public void PowerOff()
	{
		IsPoweredOn = false;
		ControlRodEffect = 0;
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName)
	{
		if(PropertyChanged != null)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

public class FuelRod
{
	public float Degradation { get; set; }
	public float Output { get; set; }
	public float Temperature { get; set; }
	public const float BaseTemperature = 1;
	public const float MaxRodOutput = 5.5f;
}