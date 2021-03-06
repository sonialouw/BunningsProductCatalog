﻿using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Domain.Repository;
using System;


namespace BunningsProductCatalog.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		public UnitOfWork(BunningsProductCatalogContext context, IRepositoryProvider repositoryProvider)
		{
			Context = context;
			repositoryProvider.DbContext = Context;
			RepositoryProvider = repositoryProvider;
		}

		protected IRepositoryProvider RepositoryProvider { get; set; }

		private BunningsProductCatalogContext Context { get; }

		public IRepository<Company> Companies => GetStandardRepo<Company>();
		public IRepository<Supplier> Suppliers => GetStandardRepo<Supplier>();
		public IRepository<CompanyProductBarcode> CompanyProductBarcodes => GetStandardRepo<CompanyProductBarcode>();
		public IRepository<CompanyProduct> CompanyProducts => GetStandardRepo<CompanyProduct>();

		public void Save()
		{
			try
			{
				Context.SaveChanges();
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		private IRepository<T> GetStandardRepo<T>() where T : class
		{
			return RepositoryProvider.GetRepositoryForEntityType<T>();
		}

		private T GetRepo<T>() where T : class
		{
			return RepositoryProvider.GetRepository<T>();
		}

		#region IDisposable

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (Context != null)
				{
					Context.Dispose();
				}
			}
		}

		#endregion
	}
}