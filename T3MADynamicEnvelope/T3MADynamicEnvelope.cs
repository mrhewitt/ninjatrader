/**
 * T3 Moving Average Dynamic Envelope
 *
 * Provides guideline for containing price and showing potential points of
 * being over-extended or potential exits
 *
 * Conceptually based on MT4 indicator by sjcoinc2000@yahoo.com and mediciforexfactory
 * but is a totally new implementation based on Ninjatrader indicators
 *
 * Note the tFactor has been tweaked to suit my purposes using 3-tick range bars
 * and may not be a generally applicable settings
 *
 * @author Mark Hewitt  <https://github.com/mrhewitt>
 */


#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class T3MADynamicEnvelope : Indicator
	{
       #region Variables
 			private int tCount = 2;
		#endregion
		
		protected override void OnStateChange()
		{
			switch (State) {
				case State.SetDefaults:
					Description									= @"Envelope indicator based on a T3 MA";
					Name										= "T3MADynamicEnvelope";
					Calculate									= Calculate.OnBarClose;
					IsOverlay									= false;
					DisplayInDataBox							= false;
					DrawOnPricePanel							= true;
					DrawHorizontalGridLines						= false;
					DrawVerticalGridLines						= false;
					PaintPriceMarkers							= false;
					ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
					
					//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
					//See Help Guide for additional information.
					IsSuspendedWhileInactive	= true;
					MAPeriod					= 30;
					VFactor						= 0.4;
					ATRPeriod					= 100;
					ATRMultiple					= 4;
					break;
					
				case State.Configure:
					AddPlot(Brushes.Silver, NinjaTrader.Custom.Resource.NinjaScriptIndicatorUpper);
					AddPlot(Brushes.Silver, NinjaTrader.Custom.Resource.NinjaScriptIndicatorLower);
					break;
					
				case State.DataLoaded:
					Plots[0].DashStyleHelper = DashStyleHelper.Dot;
					Plots[1].DashStyleHelper = DashStyleHelper.Dot;
					break;
			}		
		}

		protected override void OnBarUpdate()
		{			
			double atr = ATR(ATRPeriod)[0];
			double t3 = T3(MAPeriod,tCount,VFactor)[0]; 
			Upper[0] = t3 + (atr * ATRMultiple);
			Lower[0] = t3 - (atr * ATRMultiple);				
         }
		
		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="MAPeriod", Description="T3MA Period", Order=1, GroupName="Parameters")]
		public int MAPeriod
		{ get; set; }

		[NinjaScriptProperty]
		[Range(0, double.MaxValue)]
		[Display(Name="VFactor", Description="V Factor for the T3MA", Order=2, GroupName="Parameters")]
		public double VFactor
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="ATRPeriod", Order=3, GroupName="Parameters")]
		public int ATRPeriod
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="ATRMultiple", Order=4, GroupName="Parameters")]
		public int ATRMultiple
		{ get; set; }

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[XmlIgnore()]
		public Series<double> Upper
		{
			get { return Values[0]; }
		}
		
		/// <summary>
		/// </summary>
		[Browsable(false)]
		[XmlIgnore()]
		public Series<double> Lower
		{
			get { return Values[1]; }
		}


	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private T3MADynamicEnvelope[] cacheT3MADynamicEnvelope;
		public T3MADynamicEnvelope T3MADynamicEnvelope(int mAPeriod, double vFactor, int aTRPeriod, int aTRMultiple)
		{
			return T3MADynamicEnvelope(Input, mAPeriod, vFactor, aTRPeriod, aTRMultiple);
		}

		public T3MADynamicEnvelope T3MADynamicEnvelope(ISeries<double> input, int mAPeriod, double vFactor, int aTRPeriod, int aTRMultiple)
		{
			if (cacheT3MADynamicEnvelope != null)
				for (int idx = 0; idx < cacheT3MADynamicEnvelope.Length; idx++)
					if (cacheT3MADynamicEnvelope[idx] != null && cacheT3MADynamicEnvelope[idx].MAPeriod == mAPeriod && cacheT3MADynamicEnvelope[idx].VFactor == vFactor && cacheT3MADynamicEnvelope[idx].ATRPeriod == aTRPeriod && cacheT3MADynamicEnvelope[idx].ATRMultiple == aTRMultiple && cacheT3MADynamicEnvelope[idx].EqualsInput(input))
						return cacheT3MADynamicEnvelope[idx];
			return CacheIndicator<T3MADynamicEnvelope>(new T3MADynamicEnvelope(){ MAPeriod = mAPeriod, VFactor = vFactor, ATRPeriod = aTRPeriod, ATRMultiple = aTRMultiple }, input, ref cacheT3MADynamicEnvelope);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.T3MADynamicEnvelope T3MADynamicEnvelope(int mAPeriod, double vFactor, int aTRPeriod, int aTRMultiple)
		{
			return indicator.T3MADynamicEnvelope(Input, mAPeriod, vFactor, aTRPeriod, aTRMultiple);
		}

		public Indicators.T3MADynamicEnvelope T3MADynamicEnvelope(ISeries<double> input , int mAPeriod, double vFactor, int aTRPeriod, int aTRMultiple)
		{
			return indicator.T3MADynamicEnvelope(input, mAPeriod, vFactor, aTRPeriod, aTRMultiple);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.T3MADynamicEnvelope T3MADynamicEnvelope(int mAPeriod, double vFactor, int aTRPeriod, int aTRMultiple)
		{
			return indicator.T3MADynamicEnvelope(Input, mAPeriod, vFactor, aTRPeriod, aTRMultiple);
		}

		public Indicators.T3MADynamicEnvelope T3MADynamicEnvelope(ISeries<double> input , int mAPeriod, double vFactor, int aTRPeriod, int aTRMultiple)
		{
			return indicator.T3MADynamicEnvelope(input, mAPeriod, vFactor, aTRPeriod, aTRMultiple);
		}
	}
}

#endregion
