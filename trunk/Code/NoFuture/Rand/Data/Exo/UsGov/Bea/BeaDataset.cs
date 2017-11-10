using System.Collections.Generic;
using NoFuture.Rand.Data.Exo.UsGov.Bea.Parameters.GDPbyIndustry;
using NoFuture.Rand.Data.Exo.UsGov.Bea.Parameters.Iip;
using NoFuture.Rand.Data.Exo.UsGov.Bea.Parameters.Ita;

namespace NoFuture.Rand.Data.Exo.UsGov.Bea
{
    public class GDPbyIndustry : BeaDataSet
    {
        #region fields
        private BeaYear _beaYear;
        private Frequency _freq;
        private Industry _industry;
        private TableID _tableId;
        #endregion

        #region ctors

        public GDPbyIndustry()
        {
            _freq = new Frequency
            {
                Options =
                    (BeaParameterOptions.IsRequired | BeaParameterOptions.HasAllValue |
                     BeaParameterOptions.AllowMultiples),
                AllValue = Globals.ALL_STRING
            };
            _industry = new Industry
            {
                Options =
                    (BeaParameterOptions.IsRequired | BeaParameterOptions.HasAllValue |
                     BeaParameterOptions.AllowMultiples),
                AllValue = Globals.ALL_STRING
            };
            _tableId = new TableID
            {
                Options =
                    (BeaParameterOptions.IsRequired | BeaParameterOptions.HasAllValue |
                     BeaParameterOptions.AllowMultiples),
                AllValue = Globals.ALL_STRING
            };
            _beaYear = new BeaYear
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
        public Industry IndustryParam
        {
            get { return _industry; }
            set { _industry = (Industry) BeaParameter.PerserveOptions(_industry, value); }
        }
        public TableID TableIdParam
        {
            get { return _tableId; }
            set { _tableId = (TableID)BeaParameter.PerserveOptions(_tableId, value); }
        }
        public virtual BeaYear BeaYearParam
        {
            get { return _beaYear; }
            set { _beaYear = (BeaYear)BeaParameter.PerserveOptions(_beaYear, value); }
        }

        public virtual Frequency FreqParam
        {
            get { return _freq; }
            set { _freq = (Frequency) BeaParameter.PerserveOptions(_freq, value); }
        }
        #endregion
    }

    public class Iip : BeaDataSet
    {
        #region fields
        private BeaYear _beaYear;
        private Frequency _freq;
        private TypeOfInvestment _typeOfInvestment;
        private Component _component;
        #endregion

        #region ctors

        public Iip()
        {
            _typeOfInvestment = new TypeOfInvestment
            {
                Options =
                    (BeaParameterOptions.HasAllValue | BeaParameterOptions.HasDefaultValue |
                     BeaParameterOptions.AllowMultiples),
                DefaultValue = Globals.ALL_STRING,
                AllValue = Globals.ALL_STRING
            };
            _component = new Component
            {
                Options =
                    (BeaParameterOptions.HasAllValue | BeaParameterOptions.HasDefaultValue |
                     BeaParameterOptions.AllowMultiples),
                DefaultValue = Globals.ALL_STRING,
                AllValue = Globals.ALL_STRING
            };
            _freq = new Frequency
            {
                Options =
                    (BeaParameterOptions.HasAllValue | BeaParameterOptions.HasDefaultValue |
                     BeaParameterOptions.AllowMultiples),
                DefaultValue = Globals.ALL_STRING,
                AllValue = Globals.ALL_STRING
            };
            _beaYear = new BeaYear
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
        public TypeOfInvestment TypeOfInvestmentParam
        {
            get { return _typeOfInvestment; }
            set
            {
                _typeOfInvestment =
                    (TypeOfInvestment) BeaParameter.PerserveOptions(_typeOfInvestment, value);
            }
        }

        public Component ComponentParam
        {
            get { return _component; }
            set { _component = (Component) BeaParameter.PerserveOptions(_component, value); }
        }
        public virtual BeaYear BeaYearParam
        {
            get { return _beaYear; }
            set { _beaYear = (BeaYear)BeaParameter.PerserveOptions(_beaYear, value); }
        }

        public virtual Frequency FreqParam
        {
            get { return _freq; }
            set { _freq = (Frequency)BeaParameter.PerserveOptions(_freq, value); }
        }
        #endregion
    }

    public class Ita : BeaDataSet
    {
        #region fields
        private BeaYear _beaYear;
        private Frequency _freq;
        private Indicator _indicator;
        private AreaOrCountry _areaOrCountry;
        #endregion

        #region ctors

        public Ita()
        {
            _indicator = new Indicator
            {
                Options =
                    (BeaParameterOptions.HasAllValue | BeaParameterOptions.HasDefaultValue |
                     BeaParameterOptions.AllowMultiples),
                DefaultValue = Globals.ALL_STRING,
                AllValue = Globals.ALL_STRING
            };
            _areaOrCountry = new AreaOrCountry
            {
                Options =
                    (BeaParameterOptions.HasAllValue | BeaParameterOptions.HasDefaultValue |
                     BeaParameterOptions.AllowMultiples),
                DefaultValue = "AllCountries",
                AllValue = Globals.ALL_STRING
            };
            _freq = new Frequency
            {
                Options =
                    (BeaParameterOptions.HasAllValue | BeaParameterOptions.HasDefaultValue |
                     BeaParameterOptions.AllowMultiples),
                DefaultValue = Globals.ALL_STRING,
                AllValue = Globals.ALL_STRING
            };
            _beaYear = new BeaYear
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
        public Indicator IndicatorParam
        {
            get { return _indicator; }
            set { _indicator = (Indicator) BeaParameter.PerserveOptions(_indicator, value); }
        }

        public AreaOrCountry AreaOrCountryParam
        {
            get { return _areaOrCountry; }
            set { _areaOrCountry = (AreaOrCountry) BeaParameter.PerserveOptions(_areaOrCountry, value); }
        }
        public virtual BeaYear BeaYearParam
        {
            get { return _beaYear; }
            set { _beaYear = (BeaYear)BeaParameter.PerserveOptions(_beaYear, value); }
        }

        public virtual Frequency FreqParam
        {
            get { return _freq; }
            set { _freq = (Frequency)BeaParameter.PerserveOptions(_freq, value); }
        }
        #endregion
    }
    
    public class Nipa : BeaDataSet
    {
        #region fields
        private BeaYear _beaYear;
        private Frequency _freq;
        private Parameters.Nipa.TableID _tableId;
        #endregion

        #region ctors

        public Nipa()
        {
            _tableId = new Data.Exo.UsGov.Bea.Parameters.Nipa.TableID
            {
                Options = BeaParameterOptions.IsRequired
            };
            _freq = new Frequency
            {
                Options = (BeaParameterOptions.IsRequired | BeaParameterOptions.AllowMultiples)
            };
            _beaYear = new BeaYear
            {
                Options = BeaParameterOptions.IsRequired | BeaParameterOptions.HasAllValue | BeaParameterOptions.AllowMultiples,
                AllValue = "X"
            };
            MyParameters = new List<BeaParameter> { _tableId, _freq, _beaYear };
        }

        #endregion

        #region parameters
        public Data.Exo.UsGov.Bea.Parameters.Nipa.TableID TableIdParam
        {
            get { return _tableId; }
            set { _tableId = (Data.Exo.UsGov.Bea.Parameters.Nipa.TableID) BeaParameter.PerserveOptions(_tableId, value); }
        }
        public virtual BeaYear BeaYearParam
        {
            get { return _beaYear; }
            set { _beaYear = (BeaYear)BeaParameter.PerserveOptions(_beaYear, value); }
        }

        public virtual Frequency FreqParam
        {
            get { return _freq; }
            set { _freq = (Frequency)BeaParameter.PerserveOptions(_freq, value); }
        }
        #endregion
    }

    public class NIUnderlyingDetail : BeaDataSet
    {
        #region fields
        private BeaYear _beaYear;
        private Frequency _freq;
        private Parameters.NIUnderlyingDetail.TableID _tableId;
        #endregion
        
        #region ctors

        public NIUnderlyingDetail()
        {
            _tableId = new Data.Exo.UsGov.Bea.Parameters.NIUnderlyingDetail.TableID
            {
                Options = BeaParameterOptions.IsRequired
            };
            _freq = new Frequency
            {
                Options = BeaParameterOptions.IsRequired | BeaParameterOptions.AllowMultiples
            };
            _beaYear = new BeaYear
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

        public virtual BeaYear BeaYearParam
        {
            get { return _beaYear; }
            set { _beaYear = (BeaYear)BeaParameter.PerserveOptions(_beaYear, value); }
        }

        public virtual Frequency FreqParam
        {
            get { return _freq; }
            set { _freq = (Frequency)BeaParameter.PerserveOptions(_freq, value); }
        }
        #endregion
    }

    public class RegionalData : BeaDataSet
    {
        #region fields
        private Parameters.RegionalData.KeyCode _keyCode;
        private Parameters.RegionalData.GeoFips _geoFips;
        private BeaYear _beaYear;
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
            _beaYear = new BeaYear
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

        public BeaYear BeaYearParam
        {
            get { return _beaYear; }
            set { _beaYear = (BeaYear) BeaParameter.PerserveOptions(_beaYear, value); }
        }
        #endregion
    }
}
