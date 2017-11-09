using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using NoFuture.Shared;
using NoFuture.Shared.Core;

namespace NoFuture.Gsm
{
    public class Settings
    {
        /// <summary>
        /// A default amount of milliseconds used to park thread between Write and Read calls to a Serial Port.
        /// </summary>
        public static int ThreadSleepMs = NfConfig.ThreadSleepTime;

        /// <summary>
        /// A global flag used within various cmdlets to flag if it has sent the +CMGF to the Serial Port.
        /// </summary>
        public static bool HaveSetTextMode { get; set; }

        /// <summary>
        /// A global flag used within various cmdlets to flag if its has sent the +CPBS to the Serial Port.
        /// </summary>
        public static bool HaveSetPhoneBook { get; set; }
    }

    public class Utility
    {
        #region Constants
        /// <summary>
        /// Pattern for matching common MT command \ response headers.  Typically these are the 
        /// actual AT command.
        /// </summary>
        public const string AT_CMD_REGEX_PATTERN = @"^(AT)?\+[A-Z]{4}([\x3A\x20] | [\x3D])?\x3F?";

        /// <summary>
        /// Pattern for simply have text 'OK' as very last in a string.
        /// </summary>
        public const string OK_RESPONSE_REGEX_PATTERN = "OK[\r\n]*$";

        public const string ERROR_RESPONSE_REGEX_PATTERN = @".*?[\r\n]+ERROR[\r\n]+";
        #endregion

        /// <summary>
        /// Attempts to remove the AT command prefix from the front of any string.
        /// </summary>
        /// <param name="anySupposedAtLine">Checks for match against <see cref="AT_CMD_REGEX_PATTERN"/> and removes it from the string.</param>
        /// <returns></returns>
        public static string StripOffAtCommand(string anySupposedAtLine)
        {
            var matchToAtCmdInFront = Regex.Match(anySupposedAtLine, string.Format("({0})", AT_CMD_REGEX_PATTERN));
            if(matchToAtCmdInFront.Success)
            {
                var matchedTo = matchToAtCmdInFront.Groups[0];
                return anySupposedAtLine.Substring(matchedTo.Length, anySupposedAtLine.Length - matchedTo.Length);
            }
            return anySupposedAtLine;
        }

        /// <summary>
        /// Compares <see cref="cmdOutput"/> to <see cref="OK_RESPONSE_REGEX_PATTERN"/> 
        /// when param is non-null\non-empty.
        /// </summary>
        /// <param name="cmdOutput"></param>
        /// <returns>
        /// True for an non-null, non-empty or 
        /// no match to <see cref="OK_RESPONSE_REGEX_PATTERN"/>.
        /// </returns>
        public static bool IsCmdOutputError(string cmdOutput)
        {
            //expect there to be something
            if (string.IsNullOrWhiteSpace(cmdOutput))
                return true;
            return Regex.IsMatch(cmdOutput, ERROR_RESPONSE_REGEX_PATTERN);
        }

        /// <summary>
        /// TS 27.005 version 11.0.0 Release 11 section 4.3 Send Message +CMGS
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string EncodeSmsMessage(string phoneNumber, string message)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(message))
                return null;

            var phCopy = message;
            var phChars = phoneNumber.ToCharArray();

            //scrub the ph number to contain only digits
            var ph = new System.Text.StringBuilder();
            foreach (var phChar in phChars.Where(char.IsDigit))
                ph.Append(phChar);

            phCopy = ph.ToString();

            //if the '1' is missing from the front of standard POTS number then add it
            if (phCopy.Length == 10)
                phCopy = string.Format("1{0}", phCopy);

            //with standard POTS number the '+' is required at the front
            if (phCopy.Length == 11)
                phCopy = string.Format("+{0}", phCopy);

            //this is untested - SMS short numbers
            if (phoneNumber.StartsWith("*") && phoneNumber.Length < 11)
                phCopy = string.Format("*{0}", phCopy);

            var sb = new System.Text.StringBuilder();
            sb.AppendFormat("AT+CMGS=\"{0}\"", phCopy);
            sb.Append(Convert.ToChar((byte) 0xD)); //<CR>
            sb.Append(message);
            sb.Append(Convert.ToChar((byte) 0x1A)); // ctrl-Z

            return sb.ToString();
        }
    }

    /// <summary>
    /// TS 24.008 [version 8.6.0]; Table 10.5.118/3GPP TS 24.008: Called party BCD number
    /// </summary>
    public class TypeOfNumber
    {
        public const byte EXT = 8;
        public const byte UNKNOWN = EXT | 0;
        public const byte INTERNATIONAL_NUMBER = EXT | 1;
        public const byte NATIONAL_NUMBER = EXT | 2;
        public const byte NETWORK_SPECIFIC_NUMBER = EXT | 3;
        public const byte DEDICATED_ACCESS_SHORT_CODE = EXT | 4;
        public const byte RESERVED_FIVE = EXT | 5;
        public const byte RESERVED_AT_SIX = EXT | 6;
        public const byte RESERVED_FOR_EXTENSION = EXT | 7;
    }

    /// <summary>
    /// TS 24.008 [version 8.6.0]; Table 10.5.118/3GPP TS 24.008: Called party BCD number (continued)
    /// </summary>
    public class NumberingPlanId
    {
        public const byte UNKNOWN = 0;
        public const byte ISDN_TELEPHONY_NUMBERING_PLAN = 1;
        public const byte DATA_NUMBERING_PLAN = 2;
        public const byte TELEX_NUMBERING_PLAN = 3;
        public const byte NATIONAL_NUMBERING_PLAN = 4;
        public const byte PRIVATE_NUMBERING_PLAN = 5;
        public const byte RESERVED_FOR_CTS = 6;
        public const byte RESERVED_FOR_EXTENSION = 7;
    }

    /// <summary>
    /// TS 27.007 [version 8.3] 8.11
    /// </summary>
    public class PhoneBook
    {
        #region Constants
        public const string DIALED_CALLS = "DC";
        public const string EMERGENCY_NUMBER = "EN";
        public const string FIXED_DIALING = "FD";
        public const string SIM_LAST_DIALING = "LD";
        public const string MISSED_CALLS = "MC";
        public const string MOBILE_EQUIPMENT = "ME";
        public const string COMBINED = "MT";
        public const string OWN_NUMBERS = "ON";
        public const string RECEIVED_CALLS = "RC";
        public const string SIM_CARD = "SM";
        public const string TA = "TA";
        public const string SELECTED_APPLICATION = "AP";
        /// <summary>
        /// this one was listed in my RAZRV3, don't know what it stands for
        /// </summary>
        public const string PROVIDER_SPECIFIC = "AD";

        public const string CPBS_RSPN_REGEX_PATTERN = @"\x22[A-Z]{2}\x22\x2C[0-9]{1,4}\x2C[0-9]{1,4}";
        #endregion

        #region Properties
        public string Storage { get; set; }
        public int BlocksUsed { get; set; }
        public int TotalBlocks { get; set; }
        #endregion

        /// <summary>
        /// Attempts to parse the results of +CPBS? from text into a 
        /// PhoneBook type
        /// </summary>
        /// <param name="atData">The text returned from the MT upon sending the +CPBS? command</param>
        /// <param name="phoneBook">The values parsed from the given text.</param>
        /// <returns>Truth-value of <see cref="atData"/> being parsed.</returns>
        public static bool TryParse(string atData, out PhoneBook phoneBook)
        {
            var parseResult = false;
            try
            {
                //distill string in unix style newline
                var atDataChars = new System.Text.StringBuilder();
                foreach (var c in atData.ToCharArray().Where(c => c != (char)0x0D))
                {
                    atDataChars.Append(c);
                }

                //split on unix newline
                var cpbsLines = atDataChars.ToString().Split((char)0x0A);

                //if caller passed in multi-line string then scope in on the target
                if(cpbsLines.Length > 1)
                {
                    //find the target line in the list of lines
                    foreach (var ln in cpbsLines)
                    {
                        var cpbsLine = Utility.StripOffAtCommand(ln);
                        if (!Regex.IsMatch(cpbsLine, CPBS_RSPN_REGEX_PATTERN))
                            continue;

                        //reassign param to target line
                        atData = cpbsLine;
                        break;
                    }
                }

                if (string.IsNullOrWhiteSpace(atData))
                {
                    phoneBook = null;
                    return false;
                }
                    
                if (!atData.Contains(","))
                {
                    phoneBook = null;
                    return false;
                }
                var data = atData.Split(',');
                if (data.Length < 3)
                {
                    phoneBook = null;
                    return false;
                }

                var storage = data[0];
                if(string.IsNullOrWhiteSpace(storage))
                {
                    phoneBook = null;
                    return false;
                }
                int usedBlocks;
                int totalBlocks;
                
                
                if(!int.TryParse(data[1],out usedBlocks))
                {
                    phoneBook = null;
                    return false;
                }

                if(!int.TryParse(data[2], out totalBlocks))
                {
                    phoneBook = null;
                    return false;
                }

                phoneBook = new PhoneBook
                                {
                                    BlocksUsed = usedBlocks,
                                    TotalBlocks = totalBlocks,
                                    Storage = storage.Replace("\"", "")
                                };
                parseResult = true;
            }
            catch (Exception)
            {
                phoneBook = null;
            }
            return parseResult;
        }

        /// <summary>
        /// Asserts that <see cref="phoneBookCode"/> in not null nor empty and that
        /// it matches, by case, one of the phone book contants.
        /// </summary>
        /// <param name="phoneBookCode"></param>
        /// <returns></returns>
        public static bool IsValidPhoneBookCode(string phoneBookCode)
        {
            if (string.IsNullOrWhiteSpace(phoneBookCode))
                return false;
            return phoneBookCode == DIALED_CALLS ||
                   phoneBookCode == EMERGENCY_NUMBER ||
                   phoneBookCode == FIXED_DIALING ||
                   phoneBookCode == SIM_LAST_DIALING ||
                   phoneBookCode == MISSED_CALLS ||
                   phoneBookCode == MOBILE_EQUIPMENT ||
                   phoneBookCode == COMBINED ||
                   phoneBookCode == OWN_NUMBERS ||
                   phoneBookCode == RECEIVED_CALLS ||
                   phoneBookCode == SIM_CARD ||
                   phoneBookCode == TA ||
                   phoneBookCode == SELECTED_APPLICATION ||
                   phoneBookCode == PROVIDER_SPECIFIC;
        }

    }

    /// <summary>
    /// TS 27.007 [version 8.3.0] Release 8; section 8.12
    /// http://en.wikipedia.org/wiki/URI_scheme
    /// </summary>
    public class PhoneBookEntry
    {
        #region Properties
        public int Index { get; set; }
        public string Number { get; set; }
        public PhoneBookEntryType NumberType { get; set; }
        public string Text { get; set; }
        public bool? Hidden { get; set; }
        public string Group { get; set; }
        public string AdditionalNumber { get; set; }
        public PhoneBookEntryType AdditionalNumberType { get; set; }
        public string SecondText { get; set; }
        public string Email { get; set; }
        public string SipUri { get; set; }
        public string TelUri { get; set; }
        #endregion

        /// <summary>
        /// Attempts to parse a single +CPBR: line into a <see cref="PhoneBookEntry"/>.
        /// </summary>
        /// <param name="cpbrLine">The line candidate.</param>
        /// <param name="phoneBookEntry">The outbound <see cref="PhoneBookEntry"/> which will be assigned the parsed values if successful.</param>
        /// <returns>True if string was parsed, False otherwise.</returns>
        public static bool TryParse(string cpbrLine, out PhoneBookEntry phoneBookEntry)
        {
            
            const string INDEX = @"([0-9]{1,4})";
            const string TEXT = @"\x2C\x22(.*?)\x22";
            const string TYPE = @"\x2C([0-9]{1,3})";
            const string EMAIL = @"\x22?\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\x22?";
            const string SESSION_INIT_PROTOCOL = @"\x22?sip\:.*";
            const string TEL_PROTOCOL = @"\x22?tel\:.*";


            const string CPBR_REGEX_PATTERN_REQUIRED = INDEX + TEXT + TYPE + TEXT;

            if(String.IsNullOrWhiteSpace(cpbrLine))
            {
                phoneBookEntry = null;
                return false;
            }

            //strip off any headers
            cpbrLine = Utility.StripOffAtCommand(cpbrLine);

            try
            {
                var match = Regex.Match(cpbrLine, CPBR_REGEX_PATTERN_REQUIRED);

                if (!match.Success)
                {
                    phoneBookEntry = null;
                    return false;
                }
                phoneBookEntry = new PhoneBookEntry();

                //index
                int index = 0;
                Int32.TryParse(match.Groups[1].Value, out index);
                phoneBookEntry.Index = index;

                //number
                phoneBookEntry.Number = match.Groups[2].Value;

                //phone type
                byte phoneType = 0;
                byte.TryParse(match.Groups[3].Value, out phoneType);
                phoneBookEntry.NumberType = new PhoneBookEntryType(phoneType);

                //text
                phoneBookEntry.Text = match.Groups[4].Value;

                //optional values are present
                if (cpbrLine.Split(',').Length > 4)
                {
                    var startAt = match.Groups[4].Index + match.Groups[4].Length + 1;
                    var subStringLength = cpbrLine.Length - startAt;
                    var ov = cpbrLine.Substring(startAt, subStringLength).Split(',');
                    var ovf = (from optionalValue in ov
                               where !String.IsNullOrWhiteSpace(optionalValue)
                               select optionalValue).ToArray();

                    //see 8.14 versus 8.12, when returned from CPBR Hidden is fifth, when entered in CPBW hidden is last
                    foreach(var op in ovf)
                    {
                        //hidden 
                        if(op.Length == 1 && (op == "1" || op == "0"))
                        {
                            phoneBookEntry.Hidden = op == "1";
                            continue;
                        }

                        //additional phone type
                        if(!op.StartsWith("\"") && Regex.IsMatch(op,"[0-9]{3}"))
                        {
                            byte adNumType = 0;
                            if (byte.TryParse(op, out adNumType))
                            {
                                phoneBookEntry.AdditionalNumberType = new PhoneBookEntryType(adNumType);
                                continue;
                            }                           
                        }

                        //sip uri
                        if(Regex.IsMatch(op, SESSION_INIT_PROTOCOL))
                        {
                            phoneBookEntry.SipUri = op.Replace("\"", "");
                            continue;
                        }

                        //tel uri
                        if(Regex.IsMatch(op, TEL_PROTOCOL))
                        {
                            phoneBookEntry.TelUri = op.Replace("\"", "");
                            continue;
                        }

                        //email
                        if(Regex.IsMatch(op,EMAIL))
                        {
                            phoneBookEntry.Email = op.Replace("\"", "");
                            continue;
                        }

                        //additional number
                        if(op.ToCharArray().All(v => char.IsDigit(v) || v == '"' || v == '+'))
                        {
                            phoneBookEntry.AdditionalNumber = op.Replace("\"", "");
                            continue;
                        }

                        //take group first then second text
                        if(string.IsNullOrWhiteSpace(phoneBookEntry.Group))
                            phoneBookEntry.Group = op.Replace("\"", "");
                        else
                            phoneBookEntry.SecondText = op.Replace("\"", "");
                    }

                }

                return true;
            }
            catch
            {
                phoneBookEntry = null;
            }
            return false;
        }

        /// <summary>
        /// Returns true when obj is <see cref="PhoneBookEntry"/> type and 
        /// both its <see cref="Number"/> and this <see cref="Number"/> equal.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj as PhoneBookEntry == null)
                return false;
            var pbe = obj as PhoneBookEntry;

            return pbe.Number == Number;
        }

        public override int GetHashCode()
        {
            if (string.IsNullOrWhiteSpace(Number))
                return 1;

            return 1 + Number.GetHashCode();
        }

        /// <summary>
        /// Returns the entry encoded for +CPBW command, the string does
        /// not include the actual 'AT+CPBW=' portion, only the data.
        /// The first four fields are considered required and are always 
        /// present even as an empty string.  The optional parameters will 
        /// be added up to the first one which is null.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var cpbwCmd = new System.Text.StringBuilder();
            cpbwCmd.Append(Index);
            cpbwCmd.Append(",");
            cpbwCmd.AppendFormat("\"{0}\"", Number);
            cpbwCmd.Append(",");
            cpbwCmd.AppendFormat("{0}", NumberType);
            cpbwCmd.Append(",");
            cpbwCmd.AppendFormat("\"{0}\"", Text);

            if (!string.IsNullOrWhiteSpace(Group))
                cpbwCmd.AppendFormat(",\"{0}\"", Group);
            else
                return cpbwCmd.ToString();

            if (!string.IsNullOrWhiteSpace(AdditionalNumber))
                cpbwCmd.AppendFormat(",\"{0}\"", AdditionalNumber);
            else
                return cpbwCmd.ToString();

            if (AdditionalNumberType != null)
                cpbwCmd.AppendFormat(",{0}",AdditionalNumberType);
            else
                return cpbwCmd.ToString();

            if (!string.IsNullOrWhiteSpace(SecondText))
                cpbwCmd.AppendFormat(",\"{0}\"", SecondText);
            else
                return cpbwCmd.ToString();

            if (!string.IsNullOrWhiteSpace(Email))
                cpbwCmd.AppendFormat(",\"{0}\"", Email);
            else
                return cpbwCmd.ToString();

            if (!string.IsNullOrWhiteSpace(SipUri))
                cpbwCmd.AppendFormat(",\"{0}\"", SipUri);
            else
                return cpbwCmd.ToString();

            if (!string.IsNullOrWhiteSpace(TelUri))
                cpbwCmd.AppendFormat(",\"{0}\"", TelUri);
            else
                return cpbwCmd.ToString();

            if (Hidden != null)
                cpbwCmd.Append(Hidden.Value ? ",1" : ",0");

            return cpbwCmd.ToString();
        }
    }

    /// <summary>
    /// TS 24.008 [8] subclause 10.5.4.7
    /// </summary>
    public class PhoneBookEntryType
    {
        #region Properties
        public byte TypeOfNumber { get; set; }
        public byte NumberingPlanId { get; set; }
        #endregion

        #region Ctor
        public PhoneBookEntryType(byte typeValue)
        {
            NumberingPlanId = (byte) (typeValue & 0xF);
            TypeOfNumber = (byte)((typeValue & 0xF0) >> 4);
        }
        #endregion

        /// <summary>
        /// Returns properties as <see cref="TypeOfNumber"/> -rsh 4 -or <see cref="NumberingPlanId"/>
        /// </summary>
        /// <returns>A string with numerical value between 128 and 247</returns>
        public override string ToString()
        {
            return string.Format("{0}", TypeOfNumber << 4 | NumberingPlanId);
        }

        /// <summary>
        /// Checks if <see cref="obj"/> is a <see cref="PhoneBookEntryType"/> and its
        /// <see cref="TypeOfNumber"/> and <see cref="NumberingPlanId"/> are both equal.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj as PhoneBookEntryType == null)
                return false;
            var pbet = obj as PhoneBookEntryType;
            return pbet.NumberingPlanId == NumberingPlanId && pbet.TypeOfNumber == TypeOfNumber;
        }

        public override int GetHashCode()
        {
            return TypeOfNumber.GetHashCode() + NumberingPlanId.GetHashCode();
        }
    }

    /// <summary>
    /// TS 27.005 [version 11.0] 3.1 Parameter Definitions
    /// </summary>
    public class SmsStatusTextMode
    {
        #region Constants
        public const string RECEIVED_UNREAD = "REC UNREAD";
        public const string RECEIVED_READ = "REC READ";
        public const string STORED_UNSENT = "STO UNSENT";
        public const string STORED_SENT = "STO SENT";
        public const string ALL = "ALL";
        #endregion

        /// <summary>
        /// Asserts that <see cref="supposedMode"/> is not null nor empty and is a valid SMS Mode.
        /// </summary>
        /// <param name="supposedMode"></param>
        /// <returns></returns>
        public static bool IsValid(string supposedMode)
        {
            if (String.IsNullOrWhiteSpace(supposedMode))
                return false;

            return supposedMode == RECEIVED_UNREAD ||
                   supposedMode == RECEIVED_READ ||
                   supposedMode == STORED_UNSENT ||
                   supposedMode == STORED_SENT ||
                   supposedMode == ALL;
        }
    }

    /// <summary>
    /// TS 27.005 [version 11.0] 3.4.2 List Messages
    /// </summary>
    public class SmsSummary
    {
        public const string SMS_SUMMARY_PATTERN_LN1 = @"([0-9]{1,4})\x2C\x22(.*?)\x22\x2C\x22(.*?)\x22";

        #region Properties
        public int Index { get; set; }
        public string Status { get; set; }
        public string Number { get; set; }
        public string Message { get; set; }
        #endregion

        /// <summary>
        /// Parse a single line having the message concatenated to the end of the summary string.
        /// Its intention is mostly to serve <see cref="TryParseList"/> since the actual contents
        /// from the MT is not in the form.
        /// </summary>
        /// <param name="smsSummaryText"></param>
        /// <param name="smsOut"></param>
        /// <returns>
        /// True if the string matched to <see cref="SMS_SUMMARY_PATTERN_LN1"/> after 
        /// after having the AT command striped <see cref="Utility.StripOffAtCommand"/>.
        /// </returns>
        public static bool TryParse(string smsSummaryText, out SmsSummary smsOut)
        {
            smsOut = null;
            if (string.IsNullOrWhiteSpace(smsSummaryText))
                return false;

            try
            {
                smsSummaryText = Utility.StripOffAtCommand(smsSummaryText);

                var smsMatch = Regex.Match(smsSummaryText, SMS_SUMMARY_PATTERN_LN1);

                if(smsMatch.Success)
                {
                    smsOut = new SmsSummary();
                    var index = 0;
                    int.TryParse(smsMatch.Groups[1].Value, out index);
                    smsOut.Index = index;
                    smsOut.Status = smsMatch.Groups[2].Value;
                    smsOut.Number = smsMatch.Groups[3].Value;

                    var text = smsSummaryText.Replace(smsMatch.Groups[0].Value, "");
                    smsOut.Message = text;
                    return true;
                }
            }
            catch
            {
                smsOut = null;
                return false;
            }

            return false;
        }

        /// <summary>
        /// Will parse the entire string returned from the +CMGL command into an array of
        /// summaries.
        /// </summary>
        /// <param name="smsSummaryDump">The raw string from the Serial Port</param>
        /// <param name="smsOut">The array to received all parsed results</param>
        /// <returns></returns>
        public static bool TryParseList(string smsSummaryDump, out SmsSummary[] smsOut)
        {
            smsOut = null;
            var workingSmsOut = new List<SmsSummary>();

            if (string.IsNullOrWhiteSpace(smsSummaryDump))
                return false;

            try
            {
                //distill string in unix style newline
                var smsChars = new System.Text.StringBuilder();
                foreach (var c in smsSummaryDump.ToCharArray().Where(c => c != (char)0x0D))
                {
                    smsChars.Append(c);
                }
                
                //split on unix newline
                var smsLines = smsChars.ToString().Split((char)0x0A);

                //find index of each summary line
                var indexSummaryLines = new List<int>();
                for (var i = 0; i < smsLines.Length; i++)
                {
                    var smsLine = smsLines[i];

                    if (Regex.IsMatch(smsLine, SMS_SUMMARY_PATTERN_LN1))
                        indexSummaryLines.Add(i);
                }

                //go through each index only for lines that matched summary regex
                foreach (var j in indexSummaryLines)
                {
                    //expect message body to be next line past msg summary
                    if (smsLines.Length <= j + 1)
                        continue;

                    //put the summary and message body together and try to parse it
                    SmsSummary smsSummary = null;
                    if (TryParse(string.Format("{0} {1}", smsLines[j], smsLines[j + 1]), out smsSummary))
                        workingSmsOut.Add(smsSummary);
                }
                if (workingSmsOut.Count > 0)
                {
                    smsOut = workingSmsOut.ToArray();
                    return true;
                }

                return false;
            }
            catch
            {
                smsOut = null;
                return false;
            }
        }
    }
}

namespace NoFuture.Gsm.RazrV3
{
    public class CalendarEntry
    {
        #region Properties
        public int EntryIndex { get; set; }
        public string Description { get; set; }
        public DateTime EntryDateTime { get; set; }
        public int Duration { get; set; }
        #endregion

        /// <summary>
        /// Asserts that a <see cref="CalendarEntry"/> is equal by <see cref="Description"/>, <see cref="EntryDateTime"/> and <see cref="Duration"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(CalendarEntry obj)
        {
            var compare = obj;
            if (compare == null)
                return false;
            return (this.Description == compare.Description && this.EntryDateTime == compare.EntryDateTime && this.Duration == compare.Duration);
        }
    }
}

namespace NoFuture.Gsm.RazrV3.Calendar
{
    public class EntryRepeat
    {
        public const int Daily = 1;
        public const int Weekly = 2;
        public const int MonthlyOnDate = 3;
        public const int MonthlyOnDay = 4;
        public const int Yearly = 5;
    }
}