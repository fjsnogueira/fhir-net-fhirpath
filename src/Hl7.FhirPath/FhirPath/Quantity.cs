﻿/* 
 * Copyright (c) 2015, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.githubusercontent.com/ewoutkramer/fhir-net-api/master/LICENSE
 */

using System;
using Furore.Support;

namespace Hl7.FhirPath
{
    public struct Quantity : IComparable
    {
        public const string UCUM = "http://unitsofmeasure.org";

        public decimal Value { get; }
        public string Unit { get; }
        public string System { get; }

        public Quantity(double value, string unit, string system = UCUM) : this((decimal)value, unit, system)
        {
            // call other constructor
        }

        public Quantity(decimal value, string unit, string system = UCUM)
        {
            Value = value;
            Unit = unit;
            System = system;
        }

        public static bool operator <(Quantity a, Quantity b)
        {
            enforceSameUnits(a,b);

            return a.Value < b.Value;
        }

        public static bool operator <=(Quantity a, Quantity b)
        {
            enforceSameUnits(a, b);

            return a.Value <= b.Value;
        }


        public static bool operator >(Quantity a, Quantity b)
        {
            enforceSameUnits(a, b);

            return a.Value > b.Value;
        }

        public static bool operator >=(Quantity a, Quantity b)
        {
            enforceSameUnits(a, b);

            return a.Value >= b.Value;
        }


        public static bool operator ==(Quantity a, Quantity b)
        {
            enforceSameUnits(a, b);

            return Object.Equals(a, b);
        }

        public static bool operator !=(Quantity a, Quantity b)
        {
            enforceSameUnits(a, b);

            return !Object.Equals(a, b);
        }


        private static void enforceSameUnits(Quantity a, Quantity b)
        {
            if (a.Unit + a.System != b.Unit + b.System)
                throw Error.NotSupported("Comparing quantities with different units is not yet supported");
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is Quantity)
            {
                Quantity q = (Quantity)obj;
                return q.Unit == this.Unit
                        && q.Value == this.Value
                        && q.System == this.System;
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            return Unit.GetHashCode() ^ Value.GetHashCode() ^ System.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj is Quantity)
            {
                var p = (Quantity)obj;

                if (this < p) return -1;
                if (this > p) return 1;
                return 0;
            }
            else
                throw Error.Argument(nameof(obj), "Must be a Quantity");
        }

    }
}
