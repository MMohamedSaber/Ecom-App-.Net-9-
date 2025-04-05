
using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Data.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Ecom.infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly IMapper mapper;
        private readonly IImageManagementService imageManagementService;
        public ProductRepository(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService) : base(context)
        {
            this.mapper = mapper;
            this.imageManagementService = imageManagementService;
        }

        public async Task<bool> AddAsync(AddProductDto productDto)
        {
            if (productDto is null) return false;

            var product= mapper.Map<Product>(productDto);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var imagePath = await imageManagementService.AddImageAsync
                (productDto.Photos, productDto.Name);

            var photo = imagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = product.Id

            }).ToList();

            await _context.Photos.AddRangeAsync(photo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync(Product product)
        {
            var photos=await _context.Photos.Where(m=>m.ProductId == product.Id).ToListAsync();

            foreach (var item in photos)
            {
                imageManagementService.DeleteImageAsync(item.ImageName);
            }

            _context.Products.Remove(product);
             await _context.SaveChangesAsync    ();
        }

        public async Task<bool> UpdateAsync(UpdateProductDto productDto)
        {
            if (productDto  is null)
            {
                return false;
            }

            var FindProduct = await _context.Products
                .Include(c => c.Category)
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(p => p.Id == productDto.Id);

            if (FindProduct is null)
                return false;

            mapper.Map(productDto, FindProduct);

            await _context.SaveChangesAsync();


            var FindPhoto=await _context.Photos
                 .Where(p=>p.ProductId== productDto.Id)
                 .ToListAsync();

            // Loop through each photo and delete the corresponding image file
            foreach (var item in FindPhoto)
            {
                 imageManagementService.DeleteImageAsync(item.ImageName);
            }

            // Remove the photo records from the database
            _context.Photos.RemoveRange(FindPhoto);

            var imagePath = await imageManagementService.AddImageAsync(productDto.Photos, productDto.Name);

            var photo=imagePath
                .Select(path=> new Photo { ImageName = path, ProductId = productDto.Id })
                .ToList();

            await _context.Photos.AddRangeAsync(photo);
            await _context.SaveChangesAsync();
            return true;


        }
    }
}
