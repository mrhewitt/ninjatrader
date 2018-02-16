// 
// Copyright (C) 2006, NinjaTrader LLC <www.ninjatrader.com>.
// NinjaTrader reserves the right to modify or overwrite this NinjaScript component with each release.
//

/** 
 * Converted February 2018 to support NinjaTrader 8 
 * @author Mark Hewitt <https://github.com/mrhewitt>
 */

#region Using declarations
using System;
using System.Diagnostics;
using System.Windows.Media;
using System.ComponentModel;
using System.Xml.Serialization;
using NinjaTrader.Data;
using System.ComponentModel.DataAnnotations;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
#endregion

// This namespace holds all indicators and is required. Do not change it.
namespace NinjaTrader.NinjaScript.Indicators
{
	/// <summary>
	/// Keltner Channel. The Keltner Channel is a similar indicator to Bollinger Bands. Here the midline is a standard moving average with the upper and lower bands offset by the SMA of the difference between the high and low of the previous bars. The offset multiplier as well as the SMA period is configurable.
	/// </summary>
	[Description("The Keltner Channel is a similar indicator to Bollinger Bands. Here the midline is a standard moving average with the upper and lower bands offset by the SMA of the difference between the high and low of the previous bars. The offset multiplier as well as the SMA period is configurable.")]
	public class KeltnerChannelEMA : Indicator
	{
		#region Variables
		private	int					period				= 10;
		private double				offsetMultiplier	= 1.5;
		private Series<double>		diff;
		#endregion

		/// <summary>
		/// This method is used to configure the indicator and is called once before any bar data is loaded.
		/// </summary>
		protected override void OnStateChange()
		{
			if (State == State.Configure) {
				//Add(new Plot(Color.DarkGray, "Midline"));
				//Add(new Plot(Color.Blue,     "Upper"));
				//Add(new Plot(Color.Blue,     "Lower"));
				AddPlot(Brushes.DarkGray,		NinjaTrader.Custom.Resource.KeltnerChannelMidline);
				AddPlot(Brushes.DodgerBlue,		NinjaTrader.Custom.Resource.NinjaScriptIndicatorUpper);
				AddPlot(Brushes.DodgerBlue,		NinjaTrader.Custom.Resource.NinjaScriptIndicatorLower);
				
				diff				= new Series<double>(this);

				IsOverlay				= true;
				//PriceTypeSupported	= false;
			}	
		}

		/// <summary>
		/// Called on each bar update event (incoming tick).
		/// </summary>
		protected override void OnBarUpdate()
		{
			diff[0]			= High[0] - Low[0];

			double middle	= EMA(Typical, Period)[0];
			double offset	= EMA(diff, Period)[0] * offsetMultiplier;

			double upper	= middle + offset;
			double lower	= middle - offset;

			Midline[0] = middle;
			Upper[0] = upper;
			Lower[0] = lower;
		}

		#region Properties
		/// <summary>
		/// </summary>
		[Description("Numbers of bars used for calculations")]
		[Category("Parameters")]
		public int Period
		{
			get { return period; }
			set { period = Math.Max(1, value); }
		}

		/// <summary>
		/// </summary>
		[Range(0.01, int.MaxValue), NinjaScriptProperty]
		[Display(ResourceType = typeof(Custom.Resource), Name = "OffsetMultiplier", GroupName = "NinjaScriptParameters", Order = 0)]
		public double OffsetMultiplier
		{
			get { return offsetMultiplier; }
			set { offsetMultiplier = Math.Max(0.01, value); }
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[XmlIgnore()]
		public Series<double> Midline
		{
			get { return Values[0]; }
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[XmlIgnore()]
		public Series<double> Upper
		{
			get { return Values[1]; }
		}
		
		/// <summary>
		/// </summary>
		[Browsable(false)]
		[XmlIgnore()]
		public Series<double> Lower
		{
			get { return Values[2]; }
		}
        #endregion
	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private KeltnerChannelEMA[] cacheKeltnerChannelEMA;
		public KeltnerChannelEMA KeltnerChannelEMA(double offsetMultiplier)
		{
			return KeltnerChannelEMA(Input, offsetMultiplier);
		}

		public KeltnerChannelEMA KeltnerChannelEMA(ISeries<double> input, double offsetMultiplier)
		{
			if (cacheKeltnerChannelEMA != null)
				for (int idx = 0; idx < cacheKeltnerChannelEMA.Length; idx++)
					if (cacheKeltnerChannelEMA[idx] != null && cacheKeltnerChannelEMA[idx].OffsetMultiplier == offsetMultiplier && cacheKeltnerChannelEMA[idx].EqualsInput(input))
						return cacheKeltnerChannelEMA[idx];
			return CacheIndicator<KeltnerChannelEMA>(new KeltnerChannelEMA(){ OffsetMultiplier = offsetMultiplier }, input, ref cacheKeltnerChannelEMA);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.KeltnerChannelEMA KeltnerChannelEMA(double offsetMultiplier)
		{
			return indicator.KeltnerChannelEMA(Input, offsetMultiplier);
		}

		public Indicators.KeltnerChannelEMA KeltnerChannelEMA(ISeries<double> input , double offsetMultiplier)
		{
			return indicator.KeltnerChannelEMA(input, offsetMultiplier);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.KeltnerChannelEMA KeltnerChannelEMA(double offsetMultiplier)
		{
			return indicator.KeltnerChannelEMA(Input, offsetMultiplier);
		}

		public Indicators.KeltnerChannelEMA KeltnerChannelEMA(ISeries<double> input , double offsetMultiplier)
		{
			return indicator.KeltnerChannelEMA(input, offsetMultiplier);
		}
	}
}

#endregion
