﻿/* 
 * Copyright (c) 2015, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.githubusercontent.com/ewoutkramer/fhir-net-api/master/LICENSE
 */

using Hl7.FhirPath.Functions;
using System;
using Hl7.ElementModel;
using Furore.Support;

namespace Hl7.FhirPath
{
    public class ConstantValue : IElementNavigator
    {
        public static object ToFhirPathValue(object value)
        {
            object Value;

            if (value is Boolean)
                Value = value;
            else if (value is String)
                Value = value;
            else if (value is Uri)
                Value = ((Uri)value).OriginalString;
            else if (value is char)
                Value = new String((char)value,1);
            else if (value is Int32 || value is Int16 || value is UInt16 || value is UInt32 || value is Int64 || value is UInt64)
                Value = Convert.ToInt64(value);
            else if (value is float || value is double || value is Decimal)
                Value = Convert.ToDecimal(value);
            else if (value is DateTimeOffset)
                Value = PartialDateTime.FromDateTime((DateTimeOffset)value);
            else if (value is DateTime)
                Value = PartialDateTime.FromDateTime((DateTime)value);
            else if (value is PartialDateTime)
                Value = value;
            else if (value is Time)
                Value = value;
            else
                throw Error.NotSupported("Don't know how to convert an instance of .NET type {0} (with value '{1}') to a FhirPath constant"
                    .FormatWith(value.GetType().Name, value.ToString()));

            return Value;
        }

        
        private object _original;

        public ConstantValue(object value)
        {
            _original = value;
            Value = ToFhirPathValue(value);
        }

        public string Name
        {
            // todo: it's not used. Should it throw NotImplemented instead?
            get
            {
                return null;
            }
        }

        public object Value
        {
            get;
            private set;
        }

        public string TypeName
        {
            get
            {
                if (Value is Boolean)
                    return TypeInfo.Boolean.Name;
                else if (Value is String)
                    return TypeInfo.String.Name;
                else if (Value is long)
                    return TypeInfo.Integer.Name;
                else if (Value is decimal)
                    return TypeInfo.Decimal.Name;
                else if (Value is PartialDateTime)
                    return TypeInfo.DateTime.Name;
                else if (Value is Time)
                    return TypeInfo.Time.Name;
                else
                    throw Error.NotSupported("Don't know how to derive the FhirPath typename for an instance of .NET type {0} (with value '{1}')"
                            .FormatWith(Value.GetType().Name, Value.ToString()));
            }
        }

        public string Path
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            return this.ToStringRepresentation();
        }

        public override bool Equals(object obj)
        {
            if (obj is IElementNavigator)
                return Object.Equals((obj as IElementNavigator).Value,Value);
            else
                return false;
        }

        public override int GetHashCode()
        {
            if (Value != null)
                return Value.GetHashCode();
            else
                return 0;
        }

        public bool MoveToNext()
        {
            return false;
        }

        public bool MoveToFirstChild()
        {
            return false;
        }

        public IElementNavigator Clone()
        {
            return new ConstantValue(Value);
        }
    }
}
