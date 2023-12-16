﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Series;

namespace lab5 {
    internal class MainViewModel {
        public MainViewModel() {
            this.MyModel = new PlotModel { Title = "Example 1" };
            this.MyModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
            this.MyModel.InvalidatePlot(true);
        }

        public PlotModel MyModel { get; private set; }

    }
}
