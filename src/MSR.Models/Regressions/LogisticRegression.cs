/*
 * MSR Tools - tools for mining software repositories
 * 
 * Copyright (C) 2011  Semyon Kirnosenko
 */

using System;
using System.Collections.Generic;
using Accord.Statistics.Analysis;

namespace MSR.Models.Regressions
{
	public class LogisticRegression : MultipleRegression
	{
		private LogisticRegressionAnalysis regression;
		
		public override void Train()
		{
			regression = new LogisticRegressionAnalysis(
				predictorsList.ToArray(),
				resultList.ToArray()
			);
			regression.Compute();
		}
		public override double Predict(double[] predictors)
		{
			return regression.Regression.Compute(predictors);
		}
	}
}
