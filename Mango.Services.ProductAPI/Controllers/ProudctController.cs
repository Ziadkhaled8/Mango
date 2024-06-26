﻿using AutoMapper;
using Azure;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Remoting;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("/api/product")]
    [ApiController]
    [Authorize]
    public class ProudctController:ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;

        public ProudctController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _response = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto Get() 
        {
            try
            {
                IEnumerable<Product> obj = _db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(obj);
            }
            catch (Exception ex) 
            {
                _response.IsSuccess = false;
                _response.Messege=ex.Message;
            }
            return _response;
           
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Product obj = _db.Products.FirstOrDefault(p => p.ProductId == id);
                _response.Result=_mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess=false;
                _response.Messege = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles ="ADMIN")]
        public ResponseDto Create([FromBody] ProductDto productDto)
        {
            try
            {
                Product obj=_mapper.Map<Product>(productDto);
                _db.Products.Add(obj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messege = ex.Message;
            }
            return _response;
        }
        
        [HttpPut]
        [Authorize(Roles ="ADMIN")]
        public ResponseDto Update([FromBody] ProductDto productDto)
        {
            try
            {
                Product obj = _mapper.Map<Product>(productDto);
                _db.Products.Update(obj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messege = ex.Message;
            }
            return _response;
        }
        [HttpDelete]
        [Authorize(Roles ="ADMIN")]
        [Route("{id:int}")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Product obj = _db.Products.First(c => c.ProductId == id);
                _db.Products.Remove(obj);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messege = ex.Message;
            }
            return _response;
        }



    }
}
