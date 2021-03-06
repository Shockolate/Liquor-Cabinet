﻿using LiquorCabinet.Repositories;

namespace LiquorCabinet.Models
{
    /// <summary>
    ///     A Component of a Recipe with quantities.
    /// </summary>
    public class RecipeComponent : EntityBase<int>
    {
        public int RecipeId { get; set; }
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public string QuantityPart { get; set; }
        public double? QuantityMetric { get; set; }
        public double? QuantityImperial { get; set; }
    }
}