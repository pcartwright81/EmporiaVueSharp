﻿using System;

namespace EmporiaBlazor.Data
{
    public class EmporiaUsage
    {
        public EmporiaUsage(DateTime counter, double? usage)
        {
            UsageDate = counter;
            UsageValue = usage;
        }

        public DateTime UsageDate { get; set; }

        public double? UsageValue { get; set; }
    }
}
