/*
 * MSR Tools - tools for mining software repositories
 * 
 * Copyright (C) 2011  Semyon Kirnosenko
 */

using System;
using System.Collections.Generic;

namespace MSR.Models.Regressions
{
	public abstract class MultipleRegression
	{
		protected List<double[]> predictorsList = new List<double[]>();
		protected List<double> resultList = new List<double>();
		
		public void AddTrainingData(double[] predictors, double result)
		{
			predictorsList.Add(predictors);
			resultList.Add(result);
		}
		public abstract void Train();
		public abstract double Predict(double[] predictors);
	}
}
