namespace OrderDemo.Api.Services;

public static class PaymentApi
{
    public static string ProcessPayment(decimal amount, string merchantApiKey, string paymentIntent)
    {
        // Simulate payment processing
        if (paymentIntent.Contains("fail", StringComparison.OrdinalIgnoreCase))
        {
            return "Error processing payment.";
        }

        return "Success";
    }
}