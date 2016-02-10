using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Gov.Bls.Codes
{
    public class IpMeasure 
    {
        public string DisplayLevel { get; set; }
        public string MeasureCode { get; set; }
        public string MeasureText { get; set; }
        public string Selectable { get; set; }
        public string SortSequence { get; set; }
        private static List<IpMeasure> _values;
        public static List<IpMeasure> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<IpMeasure>
                           {
                           
                           new IpMeasure
                           {
                               MeasureText = "Capital productivity (2007=100)",
                               DisplayLevel = "0",
                               MeasureCode = "C00",
                               SortSequence = "8",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Capital (2007=100)",
                               DisplayLevel = "0",
                               MeasureCode = "C01",
                               SortSequence = "5",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Capital cost (million $)",
                               DisplayLevel = "0",
                               MeasureCode = "C02",
                               SortSequence = "22",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Capital cost share (percent)",
                               DisplayLevel = "0",
                               MeasureCode = "C03",
                               SortSequence = "25",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Capital to hours ratio (2007=100)",
                               DisplayLevel = "0",
                               MeasureCode = "C06",
                               SortSequence = "14",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Contribution of capital intensity (2007=100)",
                               DisplayLevel = "0",
                               MeasureCode = "C07",
                               SortSequence = "15",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Labor productivity (2007=100)",
                               DisplayLevel = "0",
                               MeasureCode = "L00",
                               SortSequence = "2",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Hours (2007=100)",
                               DisplayLevel = "0",
                               MeasureCode = "L01",
                               SortSequence = "6",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Labor compensation cost (million $)",
                               DisplayLevel = "0",
                               MeasureCode = "L02",
                               SortSequence = "23",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Labor cost share (percent)",
                               DisplayLevel = "0",
                               MeasureCode = "L03",
                               SortSequence = "26",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Number of hours (millions)",
                               DisplayLevel = "0",
                               MeasureCode = "L20",
                               SortSequence = "28",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Multifactor productivity (2007=100)",
                               DisplayLevel = "0",
                               MeasureCode = "M00",
                               SortSequence = "1",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Combined inputs (2007=100)",
                               DisplayLevel = "0",
                               MeasureCode = "M01",
                               SortSequence = "4",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Combined inputs cost (million $)",
                               DisplayLevel = "0",
                               MeasureCode = "M02",
                               SortSequence = "21",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Implicit combined inputs deflator (2007=100)",
                               DisplayLevel = "0",
                               MeasureCode = "M05",
                               SortSequence = "19",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Intermediate purchases productivity (2007=100)",
                               DisplayLevel = "0",
                               MeasureCode = "P00",
                               SortSequence = "9",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Intermediate purchases (2007=100)",
                               DisplayLevel = "0",
                               MeasureCode = "P01",
                               SortSequence = "7",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Intermediate purchases cost (million $)",
                               DisplayLevel = "0",
                               MeasureCode = "P02",
                               SortSequence = "24",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Intermediate purchases cost share (percent)",
                               DisplayLevel = "0",
                               MeasureCode = "P03",
                               SortSequence = "27",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Intermediate purchases to hours ratio (2007=100)",
                               DisplayLevel = "0",
                               MeasureCode = "P06",
                               SortSequence = "16",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Contribution of intermediate purchases intensity (2007=100)",
                               DisplayLevel = "0",
                               MeasureCode = "P07",
                               SortSequence = "17",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Output (2007=100)",
                               DisplayLevel = "0",
                               MeasureCode = "T01",
                               SortSequence = "3",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Implicit output deflator (2007=100)",
                               DisplayLevel = "0",
                               MeasureCode = "T05",
                               SortSequence = "18",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Value of production (million $)",
                               DisplayLevel = "0",
                               MeasureCode = "T30",
                               SortSequence = "20",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Unit labor cost (2007=100)",
                               DisplayLevel = "0",
                               MeasureCode = "U10",
                               SortSequence = "13",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Labor compensation (2007=100)",
                               DisplayLevel = "0",
                               MeasureCode = "U11",
                               SortSequence = "12",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Output per employee (2007=100)",
                               DisplayLevel = "0",
                               MeasureCode = "W00",
                               SortSequence = "11",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Employment (2007=100)",
                               DisplayLevel = "0",
                               MeasureCode = "W01",
                               SortSequence = "10",
                               Selectable = "T",
                           },
                           new IpMeasure
                           {
                               MeasureText = "Number of employees (thousands)",
                               DisplayLevel = "0",
                               MeasureCode = "W20",
                               SortSequence = "29",
                               Selectable = "T",
                           },

                       };
                return _values;
            }
        }
	}//end IpMeasure
}//end NoFuture.Rand.Gov.Bls.Codes