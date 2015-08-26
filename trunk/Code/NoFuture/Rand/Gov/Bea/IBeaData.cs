using System;
using System.Collections.Generic;
using System.Linq;

namespace NoFuture.Rand.Gov.Bea
{
    public interface IBeaData
    {
        string Description { get; set; }
        BeaDataFormat Format { get; set; }
    }

    public enum BeaDataFormat
    {
        Json,
        Xml
    }

    [Flags]
    public enum BeaParameterOptions
    {
        None = 0,
        IsRequired = 1,
        HasDefaultValue = 1 << 1,
        HasAllValue = 1 << 2,
        AllowMultiples = 1 << 3
    }

    public abstract class BeaDataSet : IBeaData
    {
        #region fields
        private BeaDataFormat _beaDataFormat = BeaDataFormat.Json;
        protected List<BeaParameter> MyParameters;
        #endregion

        public virtual string Description { get; set; }
        public virtual BeaDataFormat Format
        {
            get { return _beaDataFormat; }
            set { _beaDataFormat = value; }
        }

        #region reporting methods
        public virtual string[] WhatIsRequired
        {
            get
            {
                return MyParameters.Where(p => p.Options.HasFlag(BeaParameterOptions.IsRequired)).Select(pp => pp.GetType().Name).ToArray();
            }
        }

        public virtual string[] WhatHasDefaultValue
        {
            get
            {
                return MyParameters.Where(p => p.Options.HasFlag(BeaParameterOptions.HasDefaultValue)).Select(pp => pp.GetType().Name).ToArray();
            }
        }

        public virtual string[] WhatHasAllValue
        {
            get
            {
                return MyParameters.Where(p => p.Options.HasFlag(BeaParameterOptions.HasAllValue)).Select(pp => pp.GetType().Name).ToArray();
            }
        }

        public virtual string[] WhatAllowsMultiAssignment
        {
            get
            {
                return MyParameters.Where(p => p.Options.HasFlag(BeaParameterOptions.AllowMultiples)).Select(pp => pp.GetType().Name).ToArray();
            }
        }
        #endregion

        public override string ToString()
        {
            var bldr = new System.Text.StringBuilder();
            bldr.Append(Links.BeaStdUri);
            bldr.Append("method=GetData&");
            bldr.Append("datasetname=");
            bldr.Append(this.GetType().Name);
            bldr.Append("&");
            foreach (var param in MyParameters)
            {
                if(!param.Options.HasFlag(BeaParameterOptions.IsRequired) && string.IsNullOrWhiteSpace(param.Val))
                    continue;

                bldr.Append(param);
            }

            if (Format == BeaDataFormat.Xml)
                bldr.Append("ResultFormat=XML&");

            return bldr.ToString();
        }
    }

    public abstract class BeaParameter
    {
        public virtual BeaParameterOptions Options { get; set; }
        public abstract string Description { get; set; }
        public abstract string Val { get; set; }

        //assign these at ctor-time for dataset, calling asm doesn't need to know they even exists.
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        internal string DefaultValue { get; set; }
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        internal string AllValue { get; set; }

        public override string ToString()
        {
            var bldr = new System.Text.StringBuilder();
            bldr.Append(GetType().Name);
            bldr.Append("=");
            if (string.IsNullOrWhiteSpace(Val) && Options.HasFlag(BeaParameterOptions.HasDefaultValue))
            {
                bldr.Append(DefaultValue);
            }
            else if (string.IsNullOrWhiteSpace(Val) && Options.HasFlag(BeaParameterOptions.HasAllValue))
            {
                bldr.Append(AllValue);
            }
            else
            {
                bldr.Append(Val);    
            }
            
            bldr.Append("&");
            return bldr.ToString();
        }

        /// <summary>
        /// This is a helper method used in the 'set' portion of a <see cref="BeaDataSet"/> parameter 
        /// properties.  The values its intended to preserve are those set during the ctor of the
        /// dataset object.  Its expected the calling assembly will fetch parameter values from thier
        /// static Values list but these properties are specific to the Dataset not the parameter.
        /// </summary>
        /// <param name="existingParam"></param>
        /// <param name="nextParam"></param>
        /// <returns></returns>
        internal static BeaParameter PerserveOptions(BeaParameter existingParam, BeaParameter nextParam)
        {
            if (existingParam == null)
                return nextParam;
            nextParam.Options = existingParam.Options;
            nextParam.DefaultValue = existingParam.DefaultValue;
            nextParam.AllValue = existingParam.AllValue;
            return nextParam;
        }
    }

}
