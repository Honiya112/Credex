using System;
using System.Collections.Generic;
using System.Linq;
using CredexApp.Models;

namespace CredexApp.BusinessLogic { 
    public class StatisticsService {
        // Calculate Standard Deviation
        // High deviation = Unstable/Risky | Low deviation = Predictable
        public decimal CalculateVolatility(List<Transactions> transactions) { 
            if (transactions == null || transactions.Count < 2) return 0;

            // Convert to double for calculation
            double[] values = transactions.Select(t => (double)t.Spending).ToArray();
            double avg = values.Average();

            // Calculate Sum of Squares
            double sumOfSquares = values.Sum(d => Math.Pow(d - avg, 2));

            return (decimal)Math.Sqrt(sumOfSquares / (transactions.Count - 1));
        }

        // Forcasting Earnings (Profit)
        public decimal PredictNextYearEarnings(List<AnnualReport> reports) { 
            if (reports == null || reports.Count < 2) return 0;

            // X Axis = Year (ex. 2023, 2024, 2025)
            // Y Axis = Earnings
            var orderedReports = reports.OrderBy(r => r.Year).ToList();
            int n = orderedReports.Count;

            double[] x = Enumerable.Range(1, n).Select(i => (double)i).ToArray();
            double[] y = orderedReports.Select(r => (double)r.Earnings).ToArray();

            double sumX = x.Sum();
            double sumY = y.Sum();
            double sumXY = x.Zip(y, (a, b) => a * b).Sum();
            double sumXSq = x.Sum(v => Math.Pow(v, 2));

            // Formula for slope and intercept (Linear Regression)
            double slope = (n * sumXY - sumX * sumY) / (n * sumXSq - Math.Pow(sumX, 2));
            double intercept = (sumY - slope * sumX) / n;

            // Predict next month
            double prediction = slope * (n + 1) + intercept;

            return (decimal)prediction;
        }

        // Moving Average (Trend Detection)
        public List<decimal> CalculateMovingAverage(List<decimal> data, int period) { 
            var results = new List<decimal>();  
            if (data.Count < period) return results;

            for (int i = 0; i <= data.Count - period; i++) { 
                decimal sum = 0;
                for (int j = i; j < i + period; j++) { 
                    sum += data[j];
                }
                results.Add(sum / period);
            }
            return results;
        }
    }
}