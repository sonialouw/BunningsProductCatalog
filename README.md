# How to install and run solution

This is an C# Solution created in VS 2019 with .NET Core 3.1. 

There is a web application, but this is serves no purpose other than to make my life a bit easier with EF code first migrations.

This solution uses a SQL Server database so please update BunningsProductCatalogContext connection string to point to your chosen location. 

Please set BunningsProductCatalog.Web as your start up project and run the update-database command in Package Manager Console to apply the database migrations.

The main deliverables are the web jobs. In the real world this would be deployed as part of the web application as scheduled, but for this purpose you can run them manually as you would a console application. 
You do not need to run them in any particular order, however to get the desired output first time around I would suggest running them in the following order:
1) ImportSuppliers
2) ImportCompanyProducts
3) ImportCompanyProductBarcodes
4) GenerateProductCatalog

Please update the InputFilePath and OutputFilePath in the application settings to point to your chosen location on your file storage.

You may also choose to run the unit tests in the BunningsProductCatalog.Services.Tests project.

I have made quite a few assumptions in building this solution. This is not the way I would normally work, so if any assumption I made is incorrect, please let me know and I will be happy to review.


