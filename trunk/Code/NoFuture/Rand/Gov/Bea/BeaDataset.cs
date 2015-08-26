using System.Collections.Generic;

namespace NoFuture.Rand.Gov.Bea
{
    public class GDPbyIndustry : BeaDataSet
    {
        #region fields
        private Parameters.BeaYear _beaYear;
        private Parameters.Frequency _freq;
        private Parameters.GDPbyIndustry.Industry _industry;
        private Parameters.GDPbyIndustry.TableID _tableId;
        #endregion

        #region ctors

        public GDPbyIndustry()
        {
            _freq = new Parameters.Frequency
            {
                Options =
                    (BeaParameterOptions.IsRequired | BeaParameterOptions.HasAllValue |
                     BeaParameterOptions.AllowMultiples),
                AllValue = Globals.ALL_STRING
            };
            _industry = new Parameters.GDPbyIndustry.Industry
            {
                Options =
                    (BeaParameterOptions.IsRequired | BeaParameterOptions.HasAllValue |
                     BeaParameterOptions.AllowMultiples),
                AllValue = Globals.ALL_STRING
            };
            _tableId = new Parameters.GDPbyIndustry.TableID
            {
                Options =
                    (BeaParameterOptions.IsRequired | BeaParameterOptions.HasAllValue |
                     BeaParameterOptions.AllowMultiples),
                AllValue = Globals.ALL_STRING
            };
            _beaYear = new Parameters.BeaYear
            {
                Options =
                    (BeaParameterOptions.IsRequired | BeaParameterOptions.HasAllValue |
                     BeaParameterOptions.AllowMultiples),
                AllValue = Globals.ALL_STRING
            };

            MyParameters = new List<BeaParameter> { _freq, _industry, _tableId, _beaYear };
        }

        #endregion

        #region parameters
        public Parameters.GDPbyIndustry.Industry IndustryParam
        {
            get { return _industry; }
            set { _industry = (Parameters.GDPbyIndustry.Industry) BeaParameter.PerserveOptions(_industry, value); }
        }
        public Parameters.GDPbyIndustry.TableID TableIdParam
        {
            get { return _tableId; }
            set { _tableId = (Parameters.GDPbyIndustry.TableID)BeaParameter.PerserveOptions(_tableId, value); }
        }
        public virtual Parameters.BeaYear BeaYearParam
        {
            get { return _beaYear; }
            set { _beaYear = (Parameters.BeaYear)BeaParameter.PerserveOptions(_beaYear, value); }
        }

        public virtual Parameters.Frequency FreqParam
        {
            get { return _freq; }
            set { _freq = (Parameters.Frequency) BeaParameter.PerserveOptions(_freq, value); }
        }
        #endregion
    }

    public class Iip : BeaDataSet
    {
        #region fields
        private Parameters.BeaYear _beaYear;
        private Parameters.Frequency _freq;
        private Parameters.Iip.TypeOfInvestment _typeOfInvestment;
        private Parameters.Iip.Component _component;
        #endregion

        #region ctors

        public Iip()
        {
            _typeOfInvestment = new Parameters.Iip.TypeOfInvestment
            {
                Options =
                    (BeaParameterOptions.HasAllValue | BeaParameterOptions.HasDefaultValue |
                     BeaParameterOptions.AllowMultiples),
                DefaultValue = Globals.ALL_STRING,
                AllValue = Globals.ALL_STRING
            };
            _component = new Parameters.Iip.Component
            {
                Options =
                    (BeaParameterOptions.HasAllValue | BeaParameterOptions.HasDefaultValue |
                     BeaParameterOptions.AllowMultiples),
                DefaultValue = Globals.ALL_STRING,
                AllValue = Globals.ALL_STRING
            };
            _freq = new Parameters.Frequency
            {
                Options =
                    (BeaParameterOptions.HasAllValue | BeaParameterOptions.HasDefaultValue |
                     BeaParameterOptions.AllowMultiples),
                DefaultValue = Globals.ALL_STRING,
                AllValue = Globals.ALL_STRING
            };
            _beaYear = new Parameters.BeaYear
            {
                Options =
                    (BeaParameterOptions.HasAllValue | BeaParameterOptions.HasDefaultValue |
                     BeaParameterOptions.AllowMultiples),
                DefaultValue = Globals.ALL_STRING,
                AllValue = Globals.ALL_STRING
            };
            MyParameters = new List<BeaParameter> {_typeOfInvestment, _component, _freq, _beaYear};

        }

        #endregion

        #region parameters
        public Parameters.Iip.TypeOfInvestment TypeOfInvestmentParam
        {
            get { return _typeOfInvestment; }
            set
            {
                _typeOfInvestment =
                    (Parameters.Iip.TypeOfInvestment) BeaParameter.PerserveOptions(_typeOfInvestment, value);
            }
        }

        public Parameters.Iip.Component ComponentParam
        {
            get { return _component; }
            set { _component = (Parameters.Iip.Component) BeaParameter.PerserveOptions(_component, value); }
        }
        public virtual Parameters.BeaYear BeaYearParam
        {
            get { return _beaYear; }
            set { _beaYear = (Parameters.BeaYear)BeaParameter.PerserveOptions(_beaYear, value); }
        }

        public virtual Parameters.Frequency FreqParam
        {
            get { return _freq; }
            set { _freq = (Parameters.Frequency)BeaParameter.PerserveOptions(_freq, value); }
        }
        #endregion
    }

    public class Ita : BeaDataSet
    {
        #region fields
        private Parameters.BeaYear _beaYear;
        private Parameters.Frequency _freq;
        private Parameters.Ita.Indicator _indicator;
        private Parameters.Ita.AreaOrCountry _areaOrCountry;
        #endregion

        #region ctors

        public Ita()
        {
            _indicator = new Parameters.Ita.Indicator
            {
                Options =
                    (BeaParameterOptions.HasAllValue | BeaParameterOptions.HasDefaultValue |
                     BeaParameterOptions.AllowMultiples),
                DefaultValue = Globals.ALL_STRING,
                AllValue = Globals.ALL_STRING
            };
            _areaOrCountry = new Parameters.Ita.AreaOrCountry
            {
                Options =
                    (BeaParameterOptions.HasAllValue | BeaParameterOptions.HasDefaultValue |
                     BeaParameterOptions.AllowMultiples),
                DefaultValue = "AllCountries",
                AllValue = Globals.ALL_STRING
            };
            _freq = new Parameters.Frequency
            {
                Options =
                    (BeaParameterOptions.HasAllValue | BeaParameterOptions.HasDefaultValue |
                     BeaParameterOptions.AllowMultiples),
                DefaultValue = Globals.ALL_STRING,
                AllValue = Globals.ALL_STRING
            };
            _beaYear = new Parameters.BeaYear
            {
                Options =
                    (BeaParameterOptions.HasAllValue | BeaParameterOptions.HasDefaultValue |
                     BeaParameterOptions.AllowMultiples),
                DefaultValue = Globals.ALL_STRING,
                AllValue = Globals.ALL_STRING
            };
            MyParameters = new List<BeaParameter> {_indicator, _areaOrCountry, _freq, _beaYear};
        }

        #endregion

        #region parameters
        public Parameters.Ita.Indicator IndicatorParam
        {
            get { return _indicator; }
            set { _indicator = (Parameters.Ita.Indicator) BeaParameter.PerserveOptions(_indicator, value); }
        }

        public Parameters.Ita.AreaOrCountry AreaOrCountryParam
        {
            get { return _areaOrCountry; }
            set { _areaOrCountry = (Parameters.Ita.AreaOrCountry) BeaParameter.PerserveOptions(_areaOrCountry, value); }
        }
        public virtual Parameters.BeaYear BeaYearParam
        {
            get { return _beaYear; }
            set { _beaYear = (Parameters.BeaYear)BeaParameter.PerserveOptions(_beaYear, value); }
        }

        public virtual Parameters.Frequency FreqParam
        {
            get { return _freq; }
            set { _freq = (Parameters.Frequency)BeaParameter.PerserveOptions(_freq, value); }
        }
        #endregion
    }
    
    public class Nipa : BeaDataSet
    {
        #region fields
        private Parameters.BeaYear _beaYear;
        private Parameters.Frequency _freq;
        private Parameters.Nipa.TableID _tableId;
        #endregion

        #region ctors

        public Nipa()
        {
            _tableId = new Parameters.Nipa.TableID
            {
                Options = BeaParameterOptions.IsRequired
            };
            _freq = new Parameters.Frequency
            {
                Options = (BeaParameterOptions.IsRequired | BeaParameterOptions.AllowMultiples)
            };
            _beaYear = new Parameters.BeaYear
            {
                Options = BeaParameterOptions.IsRequired | BeaParameterOptions.HasAllValue | BeaParameterOptions.AllowMultiples,
                AllValue = "X"
            };
            MyParameters = new List<BeaParameter> { _tableId, _freq, _beaYear };
        }

        #endregion

        #region parameters
        public Parameters.Nipa.TableID TableIdParam
        {
            get { return _tableId; }
            set { _tableId = (Parameters.Nipa.TableID) BeaParameter.PerserveOptions(_tableId, value); }
        }
        public virtual Parameters.BeaYear BeaYearParam
        {
            get { return _beaYear; }
            set { _beaYear = (Parameters.BeaYear)BeaParameter.PerserveOptions(_beaYear, value); }
        }

        public virtual Parameters.Frequency FreqParam
        {
            get { return _freq; }
            set { _freq = (Parameters.Frequency)BeaParameter.PerserveOptions(_freq, value); }
        }
        #endregion
    }

    public class NIUnderlyingDetail : BeaDataSet
    {
        #region fields
        private Parameters.BeaYear _beaYear;
        private Parameters.Frequency _freq;
        private Parameters.NIUnderlyingDetail.TableID _tableId;
        #endregion
        
        #region ctors

        public NIUnderlyingDetail()
        {
            _tableId = new Parameters.NIUnderlyingDetail.TableID
            {
                Options = BeaParameterOptions.IsRequired
            };
            _freq = new Parameters.Frequency
            {
                Options = BeaParameterOptions.IsRequired | BeaParameterOptions.AllowMultiples
            };
            _beaYear = new Parameters.BeaYear
            {
                Options = BeaParameterOptions.IsRequired | BeaParameterOptions.HasAllValue | BeaParameterOptions.AllowMultiples,
                AllValue = "X"
            };
            MyParameters = new List<BeaParameter> { _tableId, _freq, _beaYear };
        }

        #endregion
        
        #region parameters
        public Parameters.NIUnderlyingDetail.TableID TableIdParam
        {
            get { return _tableId; }
            set { _tableId = (Parameters.NIUnderlyingDetail.TableID) BeaParameter.PerserveOptions(_tableId, value); }
        }

        public virtual Parameters.BeaYear BeaYearParam
        {
            get { return _beaYear; }
            set { _beaYear = (Parameters.BeaYear)BeaParameter.PerserveOptions(_beaYear, value); }
        }

        public virtual Parameters.Frequency FreqParam
        {
            get { return _freq; }
            set { _freq = (Parameters.Frequency)BeaParameter.PerserveOptions(_freq, value); }
        }
        #endregion
    }

    public class RegionalData : BeaDataSet
    {
        #region fields
        private Parameters.RegionalData.KeyCode _keyCode;
        private Parameters.RegionalData.GeoFips _geoFips;
        private Parameters.BeaYear _beaYear;
        #endregion

        #region ctors

        public RegionalData()
        {
            _keyCode = new Parameters.RegionalData.KeyCode
            {
                Options = BeaParameterOptions.IsRequired
            };
            _geoFips = new Parameters.RegionalData.GeoFips
            {
                Options = BeaParameterOptions.AllowMultiples
            };
            _beaYear = new Parameters.BeaYear
            {
                Options =
                    (BeaParameterOptions.HasAllValue | BeaParameterOptions.HasDefaultValue |
                     BeaParameterOptions.AllowMultiples),
                DefaultValue = Globals.ALL_STRING,
                AllValue = Globals.ALL_STRING
            };

            MyParameters = new List<BeaParameter> {_keyCode, _geoFips, _beaYear};
        }

        #endregion

        #region parameters
        public Parameters.RegionalData.KeyCode KeyCodeParam 
        { 
            get { return _keyCode; }
            set { _keyCode = (Parameters.RegionalData.KeyCode) BeaParameter.PerserveOptions(_keyCode, value); }
        }

        public Parameters.RegionalData.GeoFips GeoFipsParam
        {
            get { return _geoFips; }
            set { _geoFips = (Parameters.RegionalData.GeoFips) BeaParameter.PerserveOptions(_geoFips, value); }
        }

        public Parameters.BeaYear BeaYearParam
        {
            get { return _beaYear; }
            set { _beaYear = (Parameters.BeaYear) BeaParameter.PerserveOptions(_beaYear, value); }
        }
        #endregion
    }
}
