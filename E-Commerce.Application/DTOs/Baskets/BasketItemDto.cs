using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Application.DTOs.Baskets;

public class BasketItemDto
{
    [Required(ErrorMessage ="Product Id is required")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Product Name is required")]
    public string ProductName { get; set; } = default!;
    public string PictureUrl { get; set; } = default!;
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive number")]
    public decimal Price { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be atleast 1")]
    public int Quantity { get; set; }
}