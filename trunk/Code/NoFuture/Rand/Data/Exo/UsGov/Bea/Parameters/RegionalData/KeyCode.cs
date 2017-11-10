using System;
using System.Collections.Generic;
namespace NoFuture.Rand.Data.Exo.UsGov.Bea.Parameters.RegionalData
{
    public class KeyCode : BeaParameter
    {
        public override string Description { get; set; }
        public override string Val { get; set; }
        private static List<KeyCode> _values;
        public static List<KeyCode> Values
        {
            get
            {
                if (_values != null)
                    return _values;
                _values = new List<KeyCode>
                           {
                           
                           new KeyCode
                           {
                               Val = "GDP_SP",
                               Description = "GDP in current dollars (state annual product)",
                           },
                           new KeyCode
                           {
                               Val = "RGDP_SP",
                               Description = "Real GDP in chained dollars (state annual product)",
                           },
                           new KeyCode
                           {
                               Val = "PCRGDP_SP",
                               Description = "Per capita real GDP (state annual product)",
                           },
                           new KeyCode
                           {
                               Val = "COMP_SP",
                               Description = "Compensation of employees (state annual product)",
                           },
                           new KeyCode
                           {
                               Val = "TOPILS_SP",
                               Description = "Taxes on production and imports less subsidies (state annual product)",
                           },
                           new KeyCode
                           {
                               Val = "GOS_SP",
                               Description = "Gross operating surplus (state annual product)",
                           },
                           new KeyCode
                           {
                               Val = "SUBS_SP",
                               Description = "Subsidies (state annual product)",
                           },
                           new KeyCode
                           {
                               Val = "TOPI_SP",
                               Description = "Taxes on production and imports (state annual product)",
                           },
                           new KeyCode
                           {
                               Val = "GDP_MP",
                               Description = "GDP in current dollars (MSA annual product)",
                           },
                           new KeyCode
                           {
                               Val = "RGDP_MP",
                               Description = "Real GDP in chained dollars (MSA annual product)",
                           },
                           new KeyCode
                           {
                               Val = "PCRGDP_MP",
                               Description = "Per capita real GDP (MSA annual product)",
                           },
                           new KeyCode
                           {
                               Val = "TPI_SI",
                               Description = "Total personal income (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "POP_SI",
                               Description = "Population (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCPI_SI",
                               Description = "Per capita personal income (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "NFPI_SI",
                               Description = "Nonfarm personal income (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "FPI_SI",
                               Description = "Farm income (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "EARN_SI",
                               Description = "Earnings by place of work (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "CGSI_SI",
                               Description = "Contributions for government social insurance (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "AR_SI",
                               Description = "Adjustment for residence (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "NE_SI",
                               Description = "Net earnings by place of residence (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "DIR_SI",
                               Description = "Dividends, interest, and rent (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCTR_SI",
                               Description = "Personal current transfer receipts (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "WS_SI",
                               Description = "Wages and salaries (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "SUPP_SI",
                               Description = "Supplements to wages and salaries (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PROP_SI",
                               Description = "Proprietors Income (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "EMP000_SI",
                               Description = "Total employment (full and part time) (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "EMP100_SI",
                               Description = "Wage and salary employment (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "EMP200_SI",
                               Description = "Proprietors employment (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PJEARN_SI",
                               Description = "Average earnings per job (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PJWS_SI",
                               Description = "Average wage per job (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PJCOMP_SI",
                               Description = "Average compensation per job (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "DPI_SI",
                               Description = "Disposable personal income (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCDPI_SI",
                               Description = "Per capita disposable personal income (dollars) (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCNE_SI",
                               Description = "Per capita net earnings (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCTRAN_SI",
                               Description = "Per capita personal current transfer receipts (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCINCMAIN_SI",
                               Description = "Per capita income maintenance (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCUNIC_SI",
                               Description = "Per capita unemployment insurance compensation (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCRET_SI",
                               Description = "Per capita retirement and other (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCDIR_SI",
                               Description = "Per capita dividends, interest, and rent (state annual income)",
                           },
                           new KeyCode
                           {
                               Val = "TPI_CI",
                               Description = "Total personal income (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "POP_CI",
                               Description = "Population (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCPI_CI",
                               Description = "Per capita personal income (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "NFPI_CI",
                               Description = "Nonfarm personal income (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "FPI_CI",
                               Description = "Farm income (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "EARN_CI",
                               Description = "Earnings by place of work (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "CGSI_CI",
                               Description = "Contributions for government social insurance (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "AR_CI",
                               Description = "Adjustment for residence (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "NE_CI",
                               Description = "Net earnings by place of residence (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "DIR_CI",
                               Description = "Dividends, interest, and rent (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCTR_CI",
                               Description = "Personal current transfer receipts (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "WS_CI",
                               Description = "Wages and salaries (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "SUPP_CI",
                               Description = "Supplements to wages and salaries (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PROP_CI",
                               Description = "Proprietors' Income (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "EMP000_CI",
                               Description = "Total employment (full and part time) (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "EMP100_CI",
                               Description = "Wage and salary employment (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "EMP200_CI",
                               Description = "Proprietors employment (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PJEARN_CI",
                               Description = "Average earnings per job (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PJWS_CI",
                               Description = "Average wage per job (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PJCOMP_CI",
                               Description = "Average compensation per job (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCNE_CI",
                               Description = "Per capita net earnings (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCTRAN_CI",
                               Description = "Per capita personal current transfer receipts (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCINCMAIN_CI",
                               Description = "Per capita income maintenance (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCUNIC_CI",
                               Description = "Per capita unemployment insurance compensation (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCRET_CI",
                               Description = "Per capita retirement and other (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCDIR_CI",
                               Description = "Per capita dividends, interest, and rent (county annual income)",
                           },
                           new KeyCode
                           {
                               Val = "TPI_MI",
                               Description = "Total personal income (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "POP_MI",
                               Description = "Population (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCPI_MI",
                               Description = "Per capita personal income (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "NFPI_MI",
                               Description = "Nonfarm personal income (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "FPI_MI",
                               Description = "Farm income (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "EARN_MI",
                               Description = "Earnings by place of work (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "CGSI_MI",
                               Description = "Contributions for government social insurance (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "AR_MI",
                               Description = "Adjustment for residence (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "NE_MI",
                               Description = "Net earnings by place of residence (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "DIR_MI",
                               Description = "Dividends, interest, and rent (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCTR_MI",
                               Description = "Personal current transfer receipts (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "WS_MI",
                               Description = "Wages and salaries (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "SUPP_MI",
                               Description = "Supplements to wages and salaries (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PROP_MI",
                               Description = "Proprietors income (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "EMP000_MI",
                               Description = "Total employment (full and part time) (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "EMP100_MI",
                               Description = "Wage and salary employment (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "EMP200_MI",
                               Description = "Proprietors employment (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PJEARN_MI",
                               Description = "Average earnings per job (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PJWS_MI",
                               Description = "Average wage per job (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PJCOMP_MI",
                               Description = "Average compensation per job (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCNE_MI",
                               Description = "Per capita net earnings (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCTRAN_MI",
                               Description = "Per capita personal current transfer receipts (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCINCMAIN_MI",
                               Description = "Per capita income maintenance (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCUNIC_MI",
                               Description = "Per capita unemployment insurance compensation (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCRET_MI",
                               Description = "Per capita retirement and other (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "PCDIR_MI",
                               Description = "Per capita dividends, interest, and rent (MSA annual income)",
                           },
                           new KeyCode
                           {
                               Val = "TPI_QI",
                               Description = "Total personal income (state quarterly income)",
                           },
                           new KeyCode
                           {
                               Val = "NFPI_QI",
                               Description = "Nonfarm personal income (state quarterly income)",
                           },
                           new KeyCode
                           {
                               Val = "FPI_QI",
                               Description = "Farm income (state quarterly income)",
                           },
                           new KeyCode
                           {
                               Val = "EARN_QI",
                               Description = "Earnings by place of work (state quarterly income)",
                           },
                           new KeyCode
                           {
                               Val = "CGSI_QI",
                               Description = "Contributions for government social insurance (state quarterly income)",
                           },
                           new KeyCode
                           {
                               Val = "AR_QI",
                               Description = "Adjustment for residence (state quarterly income)",
                           },
                           new KeyCode
                           {
                               Val = "NE_QI",
                               Description = "Net earnings by place of residence (state quarterly income)",
                           },
                           new KeyCode
                           {
                               Val = "DIR_QI",
                               Description = "Dividends, interest, and rent (state quarterly income)",
                           },
                           new KeyCode
                           {
                               Val = "PCTR_QI",
                               Description = "Personal current transfer receipts (state quarterly income)",
                           },
                           new KeyCode
                           {
                               Val = "WS_QI",
                               Description = "Wages and salaries (state quarterly income)",
                           },
                           new KeyCode
                           {
                               Val = "SUPP_QI",
                               Description = "Supplements to wages and salaries (state quarterly income)",
                           },
                           new KeyCode
                           {
                               Val = "PROP_QI",
                               Description = "Proprietors income (state quarterly income)",
                           },
                           new KeyCode
                           {
                               Val = "RPI_SI",
                               Description = "Real personal income (state regional price parities)",
                           },
                           new KeyCode
                           {
                               Val = "RPCPI_SI",
                               Description = "Per capita real personal income (state regional price parities)",
                           },
                           new KeyCode
                           {
                               Val = "RPPIPD_SI",
                               Description = "Implicit regional price deflator (state regional price parities)",
                           },
                           new KeyCode
                           {
                               Val = "RPPALL_SI",
                               Description = "RPPs: All items (state regional price parities)",
                           },
                           new KeyCode
                           {
                               Val = "RPPGOODS_SI",
                               Description = "RPPs: Goods (state regional price parities)",
                           },
                           new KeyCode
                           {
                               Val = "RPPRENTS_SI",
                               Description = "RPPs: Services: Rents (state regional price parities)",
                           },
                           new KeyCode
                           {
                               Val = "RPPOTHER_SI",
                               Description = "RPPs: Services: Other (state regional price parities)",
                           },
                           new KeyCode
                           {
                               Val = "RPI_MI",
                               Description = "Real personal income (MSA regional price parities)",
                           },
                           new KeyCode
                           {
                               Val = "RPCPI_MI",
                               Description = "Per capita real personal income (MSA regional price parities)",
                           },
                           new KeyCode
                           {
                               Val = "RPPIPD_MI",
                               Description = "Implicit regional price deflator (MSA regional price parities)",
                           },
                           new KeyCode
                           {
                               Val = "RPPALL_MI",
                               Description = "RPPs: All items (MSA regional price parities)",
                           },
                           new KeyCode
                           {
                               Val = "RPPGOODS_MI",
                               Description = "RPPs: Goods (MSA regional price parities)",
                           },
                           new KeyCode
                           {
                               Val = "RPPRENTS_MI",
                               Description = "RPPs: Services: Rents (MSA regional price parities)",
                           },
                           new KeyCode
                           {
                               Val = "RPPOTHER_MI",
                               Description = "RPPs: Services: Other (MSA regional price parities)",
                           },

                       };
                return _values;
            }
        }
	}//end KeyCode
}//end NoFuture.Rand.Gov.Bea.Parameters.RegionalData