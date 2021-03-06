/*
 * MSR Tools - tools for mining software repositories
 * 
 * Copyright (C) 2010-2011  Semyon Kirnosenko
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MSR.Tools.Visualizer
{
	public class VisualizerPresenter
	{
		private IVisualizerModel model;
		private IVisualizerView view;

		public VisualizerPresenter(IVisualizerModel model, IVisualizerView view)
		{
			this.model = model;
			this.view = view;
			model.OnTitleUpdated += x => view.Title = x;
			view.OnOpenConfigFile += OpenConfigFile;
			view.OnVisualizationActivate += UseVisualization;
			view.OnChangeCleanUpOption += x => model.AutomaticallyCleanUp = x;
		}
		public void Run(string[] args)
		{
			if (args.Length > 0)
			{
				ShowGraphFromFile(args[0]);
			}
			view.Show();
		}
		private void ShowGraphFromFile(string filename)
		{
			if (! File.Exists(filename))
			{
				return;
			}
			string line = null;
			string legend = null;
			List<double> x = null;
			List<double> y = null;
			char[] separator = new char[] { ' ' };
			
			using (TextReader r = new StreamReader(filename))
			{
				while ((line = r.ReadLine()) != null)
				{
					if (line[0] == 'l')
					{
						if (x != null)
						{
							view.Graph.ShowLineWithPoints(legend, x.ToArray(), y.ToArray());
						}
						
						legend = line.Length > 1 ? line.Remove(0, 2) : null;
						x = new List<double>();
						y = new List<double>();
					}
					else
					{
						string[] elem = line.Split(separator);
						x.Add(Convert.ToDouble(elem[0]));
						y.Add(Convert.ToDouble(elem[1]));
					}
				}
				if (x != null)
				{
					view.Graph.ShowLineWithPoints(legend, x.ToArray(), y.ToArray());
				}
			}
		}
		private void ReadOptions()
		{
			Dictionary<string,string[]> visualizations = new Dictionary<string,string[]>();
			foreach (var m in model.Visualizations)
			{
				visualizations.Add(
					m.Title,
					m.GetType().Namespace
						.Replace("MSR.Tools.Visualizer.Visualizations", "")
						.Split(new char[] { '.' })
				);
			}
			
			view.SetVisualizationList(visualizations);
			view.AutomaticallyCleanUp = model.AutomaticallyCleanUp;
		}
		private void OpenConfigFile(string fileName)
		{
			try
			{
				model.OpenConfig(fileName);
				ReadOptions();
			}
			catch (Exception e)
			{
				view.ShowError(e.Message);
			}
		}
		private void UseVisualization(int number)
		{
			IVisualization visualization = model.Visualizations[number];
			
			if (! visualization.Initialized)
			{
				model.InitVisualization(visualization);
			}
			if (! visualization.Configurable || view.ConfigureVisualization(visualization))
			{
				if (model.AutomaticallyCleanUp && visualization.AllowCleanUp)
				{
					view.Graph.CleanUp();
				}

				model.CalcVisualization(visualization);
				visualization.Draw(view.Graph);
				view.Status = model.LastVisualizationProfiling;
			}
		}
	}
}
