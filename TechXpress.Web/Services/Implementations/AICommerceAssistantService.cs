using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Data.Models;
using TechXpress.Web.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Stripe;
using Review = TechXpress.Data.Models.Review;

namespace TechXpress.Web.Services.Implementations
{
    public class AICommerceAssistantService : IAIAssistantService, IAICommerceService
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly string _geminiApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent";
        private static DateTime _lastRequestTime;
        private static readonly TimeSpan MinRequestInterval = TimeSpan.FromSeconds(1);

        public AICommerceAssistantService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _client = httpClientFactory.CreateClient();
            _apiKey = configuration.GetValue<string>("AI:ApiKey");
        }

        #region Core AI Assistant Functions
        public async Task<string> GetProductRecommendations(List<ProductViewModel> viewedProducts)
        {
            var prompt = new StringBuilder("Based on these viewed products, recommend similar or complementary items:\n");

            foreach (var product in viewedProducts)
            {
                prompt.AppendLine($"- {product.Name} ({product.Category}, ${product.Price}, Rating: {product.Reviews.Average(r => r.Rating)})");
            }

            prompt.AppendLine("\nProvide recommendations in this format:");
            prompt.AppendLine("1. [Product Name] - [Brief Reason] (Category: X, Price: $Y)");

            return await GetAIResponse(prompt.ToString());
        }

        public async Task<string> GetOrderPrioritization(List<Order> orders)
        {
            var prompt = new StringBuilder("Analyze these orders and prioritize them for fulfillment:\n\n");

            foreach (var order in orders)
            {
                prompt.AppendLine($"Order #{order.Id}:");
                prompt.AppendLine($"- Items: {order.ItemCount}");
                prompt.AppendLine($"- Total: ${order.TotalAmount}");
                prompt.AppendLine($"- Order Date: {order.OrderDate}");
                prompt.AppendLine();
            }

            prompt.AppendLine("Consider these factors:");
            prompt.AppendLine("- VIP customers get priority");
            prompt.AppendLine("- Express shipping orders should be processed first");
            prompt.AppendLine("- Older orders should be prioritized");
            prompt.AppendLine("- Larger orders may need more processing time");

            prompt.AppendLine("\nProvide the prioritized order numbers in sequence with brief explanations.");

            return await GetAIResponse(prompt.ToString());
        }

        public async Task<string> GetCustomerQueryResponse(string customerQuery)
        {
            var prompt = new StringBuilder("You are an e-commerce customer service assistant. ");
            prompt.AppendLine("Respond to this customer query professionally and helpfully:\n\n");
            prompt.AppendLine(customerQuery);
            prompt.AppendLine("\nIf the query is about:");
            prompt.AppendLine("- Order status: Ask for order number first");
            prompt.AppendLine("- Returns: Provide return policy summary");
            prompt.AppendLine("- Products: Offer specific details or alternatives");
            prompt.AppendLine("- Complaints: Apologize and escalate if needed");

            return await GetAIResponse(prompt.ToString());
        }
        #endregion

        #region Expanded AI Commerce Services
        public async Task<string> GenerateProductDescriptions(ProductViewModel product)
        {
            var prompt = $"Write a compelling 150-word product description for:\n\n";
            prompt += $"Name: {product.Name}\n";
            prompt += $"Category: {product.Category}\n";
            prompt += "Key Features:\n";
            

                foreach (var specification in product.Specifications ?? Enumerable.Empty<ProductSpecification>()) { 
                prompt += $"{specification.Key}: {specification.Value}\n";
            }
            prompt += "Include SEO-friendly keywords and highlight unique selling points.";

            return await GetAIResponse(prompt);
        }

        public async Task<string> AnalyzeCustomerSentiment(List<Review> reviews)
        {
            var prompt = new StringBuilder("Analyze these product reviews for overall sentiment and key themes:\n\n");

            foreach (var review in reviews.Take(10)) // Limit to 10 for token constraints
            {
                prompt.AppendLine($"Rating: {review.Rating}/5");
                prompt.AppendLine($"Review: {review.Comment}");
                prompt.AppendLine();
            }

            prompt.AppendLine("Provide:");
            prompt.AppendLine("- Overall sentiment score (1-10)");
            prompt.AppendLine("- 3 key positive themes");
            prompt.AppendLine("- 3 key improvement areas");
            prompt.AppendLine("- Suggested actions for the product team");

            return await GetAIResponse(prompt.ToString());
        }

        public async Task<string> GeneratePersonalizedPromotions(ApplicationUser customer, List<Order> orderHistory)
        {
            var prompt = new StringBuilder($"Create personalized promotion ideas for customer:\n\n");
            prompt.AppendLine($"Name: {customer.FirstName} {customer.LastName}");
            prompt.AppendLine($"Total Spend: ${orderHistory.Sum(o => o.TotalAmount)}");
            prompt.AppendLine($"Favorite Categories: {GetTopCategories(orderHistory)}");
            prompt.AppendLine("\nSuggest:");
            prompt.AppendLine("- 3 discount offers tailored to their preferences");
            prompt.AppendLine("- 1 loyalty reward idea");
            prompt.AppendLine("- 1 cross-sell opportunity");
            prompt.AppendLine("- Email subject line and brief body text");

            return await GetAIResponse(prompt.ToString());
        }

        public async Task<string> OptimizePricingStrategy(List<ProductViewModel> products, MarketData marketData)
        {
            var prompt = new StringBuilder("Suggest pricing optimizations based on this market data:\n\n");
            prompt.AppendLine($"Competitor Average Prices: {marketData.CompetitorPrices}");
            prompt.AppendLine($"Demand Trends: {marketData.DemandTrends}");
            prompt.AppendLine($"Inventory Levels: {marketData.InventoryLevels}");
            prompt.AppendLine("\nFor these products:\n");

            foreach (var product in products.Take(5)) // Limit examples
            {
                prompt.AppendLine($"{product.Name} (Current: ${product.Price})");
            }

            prompt.AppendLine("\nRecommend:");
            prompt.AppendLine("- Which products to increase/decrease price");
            prompt.AppendLine("- Suggested new prices with rationale");
            prompt.AppendLine("- Any bundle or promotion opportunities");

            return await GetAIResponse(prompt.ToString());
        }

        public async Task<string> PredictInventoryNeeds(List<ProductViewModel> products, SalesForecast forecast)
        {
            var prompt = new StringBuilder("Predict inventory needs based on:\n\n");
            prompt.AppendLine($"Season: {forecast.Season}");
            prompt.AppendLine($"Upcoming Holidays: {string.Join(", ", forecast.UpcomingHolidays)}");
            prompt.AppendLine($"Historical Sales: {forecast.HistoricalSalesData}");
            prompt.AppendLine("\nFor these products:\n");

            foreach (var product in products.Take(5))
            {
                prompt.AppendLine($"{product.Name} (Current Stock: {product.StockQuantity}");
            }

            prompt.AppendLine("\nRecommend:");
            prompt.AppendLine("- Quantity to order for each product");
            prompt.AppendLine("- Timing for reorders");
            prompt.AppendLine("- Any products to discontinue");

            return await GetAIResponse(prompt.ToString());
        }
        #endregion

        #region Helper Methods
        private async Task<string> GetAIResponse(string prompt)
        {
            // Rate limiting
            var timeSinceLastRequest = DateTime.UtcNow - _lastRequestTime;
            if (timeSinceLastRequest < MinRequestInterval)
            {
                await Task.Delay(MinRequestInterval - timeSinceLastRequest);
            }

            try
            {
                var requestBody = new
                {
                    contents = new[] {
                        new {
                            parts = new[] {
                                new {
                                    text = prompt
                                }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        temperature = 0.7,
                        topP = 0.9,
                        maxOutputTokens = 2048
                    }
                };

                var jsonRequest = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync($"{_geminiApiUrl}?key={_apiKey}", content);
                _lastRequestTime = DateTime.UtcNow;

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    return $"AI service error: {errorBody}";
                }

                var responseStr = await response.Content.ReadAsStringAsync();
                dynamic responseObj = JsonConvert.DeserializeObject(responseStr);
                return responseObj?.candidates?[0]?.content?.parts?[0]?.text ?? "No response generated";
            }
            catch (Exception ex)
            {
                return $"Error accessing AI service: {ex.Message}";
            }
        }

        private string GetTopCategories(List<Order> orders)
        {
            var categories = orders
                .SelectMany(o => o.OrderDetails)
                .GroupBy(i => i.Product.Category)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => g.Key);

            return string.Join(", ", categories);
        }
        #endregion


        public async Task<SalesPerformanceReport> GenerateSalesPerformanceReport(List<Order> orders, DateTime startDate, DateTime endDate)
        {
            var prompt = new StringBuilder("Generate a comprehensive sales performance report with these sections:\n\n");
            prompt.AppendLine($"Time Period: {startDate:d} to {endDate:d}");
            prompt.AppendLine($"Total Orders: {orders.Count}");
            prompt.AppendLine($"Total Revenue: ${orders.Sum(o => o.TotalAmount):N2}");

            prompt.AppendLine("\n1. Overview:");
            prompt.AppendLine("- Overall sales trend (daily/weekly)");
            prompt.AppendLine("- Comparison to previous period");
            prompt.AppendLine("- Key highlights and anomalies");

            prompt.AppendLine("\n2. Product Performance:");
            prompt.AppendLine("- Top 5 selling products by revenue");
            prompt.AppendLine("- Top 5 selling products by quantity");
            prompt.AppendLine("- Underperforming products");

            prompt.AppendLine("\n3. Customer Insights:");
            prompt.AppendLine("- Customer acquisition trends");
            prompt.AppendLine("- Repeat customer rate");
            prompt.AppendLine("- Average order value by customer segment");

            prompt.AppendLine("\n4. Recommendations:");
            prompt.AppendLine("- Inventory adjustments needed");
            prompt.AppendLine("- Marketing opportunities");
            prompt.AppendLine("- Process improvements");

            var reportContent = await GetAIResponse(prompt.ToString());

            return new SalesPerformanceReport
            {
                ReportDate = DateTime.Now,
                PeriodStart = startDate,
                PeriodEnd = endDate,
                Content = reportContent,
                Metrics = CalculateSalesMetrics(orders)
            };
        }


        public async Task<InventoryAnalysisReport> GenerateInventoryAnalysisReport(List<ProductViewModel> products)
        {
            var prompt = new StringBuilder("Generate an inventory analysis report with these sections:\n\n");
            prompt.AppendLine($"Total Products: {products.Count}");
            prompt.AppendLine($"Total Inventory Value: ${products.Sum(p => p.Price * p.StockQuantity):N2}");

            prompt.AppendLine("\n1. Stock Status:");
            prompt.AppendLine("- Products running low (below reorder threshold)");
            prompt.AppendLine("- Overstocked products");
            prompt.AppendLine("- Products out of stock");

            prompt.AppendLine("\n2. Turnover Analysis:");
            prompt.AppendLine("- Fast moving products");
            prompt.AppendLine("- Slow moving products");
            prompt.AppendLine("- Dead stock");

            prompt.AppendLine("\n3. Recommendations:");
            prompt.AppendLine("- Immediate reorder suggestions");
            prompt.AppendLine("- Discount strategies for slow movers");
            prompt.AppendLine("- Potential product discontinuations");

            var reportContent = await GetAIResponse(prompt.ToString());

            return new InventoryAnalysisReport
            {
                ReportDate = DateTime.Now,
                Content = reportContent,
                InventoryMetrics = CalculateInventoryMetrics(products)
            };
        }



        public async Task<CustomerBehaviorReport> GenerateCustomerBehaviorReport(List<ApplicationUser> customers, List<Order> orders, List<Review> reviews)
        {
            var prompt = new StringBuilder("Generate a customer behavior analysis report with these sections:\n\n");
            prompt.AppendLine($"Total Customers: {customers.Count}");
            prompt.AppendLine($"Total Orders Analyzed: {orders.Count}");
            prompt.AppendLine($"Total Reviews Analyzed: {reviews.Count}");

            prompt.AppendLine("\n1. Customer Segmentation:");
            prompt.AppendLine("- High value customers");
            prompt.AppendLine("- Frequent shoppers");
            prompt.AppendLine("- One-time buyers");

            prompt.AppendLine("\n2. Purchase Patterns:");
            prompt.AppendLine("- Peak shopping times");
            prompt.AppendLine("- Preferred product categories");
            prompt.AppendLine("- Average order value trends");

            prompt.AppendLine("\n3. Sentiment Analysis:");
            prompt.AppendLine("- Overall customer satisfaction");
            prompt.AppendLine("- Common praise themes");
            prompt.AppendLine("- Frequent complaints");

            prompt.AppendLine("\n4. Recommendations:");
            prompt.AppendLine("- Loyalty program improvements");
            prompt.AppendLine("- Targeted marketing strategies");
            prompt.AppendLine("- Customer service enhancements");

            var reportContent = await GetAIResponse(prompt.ToString());

            return new CustomerBehaviorReport
            {
                ReportDate = DateTime.Now,
                Content = reportContent,
                BehaviorMetrics = CalculateBehaviorMetrics(customers, orders, reviews)
            };
        }

     





        private SalesMetrics CalculateSalesMetrics(List<Order> orders)
        {
            return new SalesMetrics
            {
                TotalRevenue = orders.Sum(o => o.TotalAmount),
                AverageOrderValue = orders.Average(o => o.TotalAmount),
                OrderCount = orders.Count,
                ProductsSold = orders.Sum(o => o.OrderDetails.Sum(od => od.Quantity))
            };
        }


        private InventoryMetrics CalculateInventoryMetrics(List<ProductViewModel> products)
        {
            return new InventoryMetrics
            {
                TotalProducts = products.Count,
                TotalInventoryValue = products.Sum(p => p.Price * p.StockQuantity),
                OutOfStockItems = products.Count(p => p.StockQuantity <= 0),
                LowStockItems = products.Count(p => p.StockQuantity > 0)
            };
        }

        private BehaviorMetrics CalculateBehaviorMetrics(List<ApplicationUser> customers, List<Order> orders, List<Review> reviews)
        {
            var repeatCustomers = customers.Count(c => orders.Count(o => o.UserId == c.Id) > 1);

            return new BehaviorMetrics
            {
                TotalCustomers = customers.Count,
                RepeatCustomerRate = (decimal)repeatCustomers / customers.Count,
                AverageRating = reviews.Any() ? (decimal)reviews.Average(r => r.Rating) : 0,
                AverageOrdersPerCustomer = orders.Any() ? (decimal)orders.Count / customers.Count : 0
            };
        }


       

    }



    public class MarketData
    {
        public string CompetitorPrices { get; set; }
        public string DemandTrends { get; set; }
        public string InventoryLevels { get; set; }
    }

    public class SalesForecast
    {
        public string Season { get; set; }
        public List<string> UpcomingHolidays { get; set; }
        public string HistoricalSalesData { get; set; }
    }

    public class SalesPerformanceReport
    {
        public DateTime ReportDate { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public string Content { get; set; }
        public SalesMetrics Metrics { get; set; }
    }

    public class CustomerBehaviorReport
    {
        public DateTime ReportDate { get; set; }
        public string Content { get; set; }
        public BehaviorMetrics BehaviorMetrics { get; set; }
    }

    






    public class SalesMetrics
    {
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int OrderCount { get; set; }
        public int ProductsSold { get; set; }
    }


    public class InventoryAnalysisReport
    {
        public DateTime ReportDate { get; set; }
        public string Content { get; set; }
        public InventoryMetrics InventoryMetrics { get; set; }
    }



    public class InventoryMetrics
    {
        public int TotalProducts { get; set; }
        public decimal TotalInventoryValue { get; set; }
        public int OutOfStockItems { get; set; }
        public int LowStockItems { get; set; }
    }

    public class BehaviorMetrics
    {
        public int TotalCustomers { get; set; }
        public decimal RepeatCustomerRate { get; set; }
        public decimal AverageRating { get; set; }
        public decimal AverageOrdersPerCustomer { get; set; }
    }


  
}
