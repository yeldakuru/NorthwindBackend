using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using IResult = Core.Utilities.Results.IResult;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;
        private ICategoryService _categoryService;
    

        public ProductManager(IProductDal productDal,ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
         
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        [PerformanceAspect(interval:5)]
        public IDataResult<List<Product>> GetList()
        {
            Thread.Sleep(5000);
            return new SuccessDataResult<List<Product>>(_productDal.GetList().ToList());
        }

        //[SecuredOperation(roles:"Product.List,Admin")]
        [LogAspect(typeof(FileLogger))]
        [CacheAspect(duration:10)]
     
        public IDataResult<List<Product>> GetListByCategory(int categoryId)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetList(p => p.CategoryId == categoryId).ToList());
        }



        [ValidationAspect(typeof(ProductValidator),Priority =1)]
        [CacheRemoveAspect(pattern:"IProductService.Get")]
      
        public IResult Add(Product product)
        {
         
         IResult result=BusinessRules.Run(CheckIfProductNameExists(product.ProductName),CheckIfCtegoryIsEnabled());
            if (result != null)
            {
                return result;
            }

            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        private IResult CheckIfProductNameExists(string? productName)
        {  
            if(_productDal.Get(filter:p=>p.ProductName==productName)!=null)
            {
                return new ErrorResult(Messages.ProductNameAlredyExists);
            }
            return new SuccessResult();
        }
        private IResult CheckIfCtegoryIsEnabled()
        {
            var result = _categoryService.GetList();
            if (result.Data.Count<10)
            {
                return new ErrorResult(Messages.ProductNameAlredyExists);
            }
            return new SuccessResult();

        }

            public IResult Delete(Product product)
        {
            _productDal.Delete(product);
            return new SuccessResult(Messages.ProductDeleted);
        }

        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult(Messages.ProductUpdated);
        }
        [TransactionScopeAspect]
        public IResult TransactionalOperation(Product product)
        {
            _productDal.Update(product);
           // _productDal.Add(product);
            return new SuccessResult(Messages.ProductUpdated);
        }
    }
}
