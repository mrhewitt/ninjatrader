// 
// Copyright (C) 2008, NinjaTrader LLC <www.ninjatrader.com>.
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
    /// T3Colored Moving Average
    /// </summary>
    [Description("T3Colored Moving Average")]
    public class T3Colored : Indicator
    {
        #region Variables

            private double 					vFactor = 0.4; // Default setting for VFactor
			private int 					tCount = 3;
			private int 					period = 4;
			private Brush					upColor					= Brushes.SteelBlue;
			private Brush					downColor				= Brushes.Firebrick;
			private int						plot0Width				= 2;
			private PlotStyle				plot0Style				= PlotStyle.Line;
			private DashStyleHelper			dash0Style				= DashStyleHelper.Solid;
			private System.Collections.ArrayList seriesCollection;

        #endregion

        /// <summary>
        /// This method is used to configure the indicator and is called once before any bar data is loaded.
        /// </summary>
        protected override void OnStateChange()
        {
			switch (State) {
				case State.Configure:
		           // Add(new Plot(Color.FromKnownColor(KnownColor.Green), PlotStyle.Line, "T3Colored"));
					AddPlot(new Stroke(Brushes.DarkCyan), PlotStyle.Line, "T3Colored");
		            IsOverlay				= true;
		 			ArePlotsConfigurable	= false;
					break;
				case State.DataLoaded:
					Plots[0].Width 			 = plot0Width;
					Plots[0].PlotStyle		 = plot0Style;
					Plots[0].DashStyleHelper = dash0Style;					
					break;
			}
       }
		
		/// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
			if (TCount == 1)
			{
				CalculateGD(Inputs[0], Values[0]);
				return;
			}
				
			if (seriesCollection == null)
			{
				seriesCollection = new System.Collections.ArrayList();
				for (int i = 0; i < TCount - 1; i++) 
					seriesCollection.Add(new Series<double>(this));
			}
			
			CalculateGD(Inputs[0], (Series<double>) seriesCollection[0]);

			for (int i = 0; i <= seriesCollection.Count - 2; i++)
				CalculateGD((Series<double>) seriesCollection[i], (Series<double>) seriesCollection[i + 1]);

			CalculateGD((Series<double>) seriesCollection[seriesCollection.Count - 1], Values[0]);	
			
			if (IsRising(Values[0]))
				PlotBrushes[0][0] = upColor;
			else 
				PlotBrushes[0][0] = downColor;
				
         }
		
		private void CalculateGD(ISeries<double> input, Series<double> output)
		{
			//output.Set((EMA(input, Period)[0] * (1 + VFactor)) - (EMA(EMA(input, Period), Period)[0] * VFactor));
			output[0] = (EMA(input, Period)[0] * (1 + VFactor)) - (EMA(EMA(input, Period), Period)[0] * VFactor);
		}
		
        #region Properties
		[Range(1, int.MaxValue), NinjaScriptProperty]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Period", GroupName = "NinjaScriptParameters", Order = 0)]
        public int Period
        {
            get { return period; }
            set { period = Math.Max(1, value); }
        }
		
		[Range(1, int.MaxValue), NinjaScriptProperty]
		[Display(ResourceType = typeof(Custom.Resource), Name = "TCount", GroupName = "NinjaScriptParameters", Order = 1)]
        public int TCount
        {
            get { return tCount; }
            set { tCount = Math.Max(1, value); }
        }

		[Range(0, int.MaxValue), NinjaScriptProperty]
		[Display(ResourceType = typeof(Custom.Resource), Name = "VFactor", GroupName = "NinjaScriptParameters", Order = 2)]
        public double VFactor
        {
            get { return vFactor; }
            set { vFactor = Math.Max(0, value); }
        }
 		/// <summary>
		/// </summary>
		[XmlIgnore()]
		[Description("Select color for rising T3")]
		[Category("Plots")]
		[Display(ResourceType = typeof(Custom.Resource), Name = "T3 Rising", GroupName = "NinjaScriptParameters", Order = 3)]
		public Brush UpColor
		{
			get { return upColor; }
			set { upColor = value; }
		}
		
		// Serialize Color object
		[Browsable(false)]
		public string UpColorSerialize
		{
			get { return Serialize.BrushToString(upColor); }
			set { upColor = Serialize.StringToBrush(value); }
		}
		
		/// <summary>
		/// </summary>
		[XmlIgnore()]
		[Description("Select color for falling T3")]
		[Category("Plots")]
		[Display(ResourceType = typeof(Custom.Resource), Name = "T3 Falling", GroupName = "NinjaScriptParameters", Order = 4)]
		public Brush DownColor
		{
			get { return downColor; }
			set { downColor = value; }
		}
		
		// Serialize Color object
		[Browsable(false)]
		public string DownColorSerialize
		{
			get { return Serialize.BrushToString(downColor); }
			set { downColor = Serialize.StringToBrush(value); }
		}
		
 		/// <summary>
		/// </summary>
		[Description("Width for T3P Line.")]
		[Category("Plots")]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Line Width", GroupName = "NinjaScriptParameters", Order = 4)]
		public int Plot0Width
		{
			get { return plot0Width; }
			set { plot0Width = Math.Max(1, value); }
		}

		/// <summary>
		/// </summary>
		[Description("DashStyle for T3 Line")]
		[Category("Plots")]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Plot Style", GroupName = "NinjaScriptParameters", Order = 4)]
		public PlotStyle Plot0Style
		{
			get { return plot0Style; }
			set { plot0Style = value; }
		}
		
		/// <summary>
		/// </summary>
		[Description("DashStyle for T3 Line")]
		[Category("Plots")]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Dash Style", GroupName = "NinjaScriptParameters", Order = 4)]
		public DashStyleHelper Dash0Style
		{
			get { return dash0Style; }
			set { dash0Style = value; }
		}
		
      #endregion
    }
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private T3Colored[] cacheT3Colored;
		public T3Colored T3Colored(int period, int tCount, double vFactor)
		{
			return T3Colored(Input, period, tCount, vFactor);
		}

		public T3Colored T3Colored(ISeries<double> input, int period, int tCount, double vFactor)
		{
			if (cacheT3Colored != null)
				for (int idx = 0; idx < cacheT3Colored.Length; idx++)
					if (cacheT3Colored[idx] != null && cacheT3Colored[idx].Period == period && cacheT3Colored[idx].TCount == tCount && cacheT3Colored[idx].VFactor == vFactor && cacheT3Colored[idx].EqualsInput(input))
						return cacheT3Colored[idx];
			return CacheIndicator<T3Colored>(new T3Colored(){ Period = period, TCount = tCount, VFactor = vFactor }, input, ref cacheT3Colored);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.T3Colored T3Colored(int period, int tCount, double vFactor)
		{
			return indicator.T3Colored(Input, period, tCount, vFactor);
		}

		public Indicators.T3Colored T3Colored(ISeries<double> input , int period, int tCount, double vFactor)
		{
			return indicator.T3Colored(input, period, tCount, vFactor);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.T3Colored T3Colored(int period, int tCount, double vFactor)
		{
			return indicator.T3Colored(Input, period, tCount, vFactor);
		}

		public Indicators.T3Colored T3Colored(ISeries<double> input , int period, int tCount, double vFactor)
		{
			return indicator.T3Colored(input, period, tCount, vFactor);
		}
	}
}

#endregion
