
using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Data.Migrations;
using Microsoft.EntityFrameworkCore;
using System.Net.Quic;

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

        public async Task<IEnumerable<ProductDTO>> GetAllAsync(ProductParams productParams)
        {
            var query = _context.Products
                .Include(p=>p.Category)
                .Include(p=>p.Photos)
                .AsNoTracking();


            //filtering by word
            if (!string.IsNullOrEmpty(productParams.Searching))
            {
                var searchWords = productParams.Searching.Split(' ');
                query = query.Where(m => searchWords.All(word =>
                    m.Name.ToLower().Contains(word.ToLower()) ||
                    m.Description.ToLower().Contains(word.ToLower())
                ));
            }

            //filtering by categoryId
            if (productParams.CategoryId.HasValue)
                query = query.Where(m => m.CategoryId== productParams.CategoryId);



            if (productParams.Sort is not  null)
            {

                query = productParams.Sort switch
                {
                    "PriceAsn" => query.OrderBy(p => p.NewPrice),
                    "PriceDes" => query.OrderByDescending(p => p.NewPrice),
                    _ => query.OrderBy(p => p.Name),
                };
            }

            query = query.Skip((productParams.PageSize) * (productParams.PageNumber -1) ).Take(productParams.PageSize);
            var result = mapper.Map<List<ProductDTO>>(query);
            return result;

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
