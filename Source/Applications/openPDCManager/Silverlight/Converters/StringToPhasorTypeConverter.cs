﻿//******************************************************************************************************
//  StringToPhasorTypeConverter.cs - Gbtc
//
//  Copyright © 2010, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  11/09/2009 - Mehulbhai P Thakkar
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace openPDCManager.Converters
{
	public class StringToPhasorTypeConverter : IValueConverter
	{

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
            //if (value.ToString() == "I")
            //    return new KeyValuePair<string, string>("I", "Current");
            //else if (value.ToString() == "V")
            //    return new KeyValuePair<string, string>("V", "Voltage");
            //else
            //    throw new ArgumentException("Value not supported as a Phasor Type");
            if (value.ToString() == "I")
                return "Current";
            else if (value.ToString() == "V")
                return "Voltage";
            else
                return "";
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
            return null;
            //if (value is KeyValuePair<string, string>)
            //{
            //    return ((KeyValuePair<string, string>)value).Key;
            //}
            //else
            //    throw new ArgumentException("Value not supported as a Phasor Type");
		}

		#endregion
	}
}
