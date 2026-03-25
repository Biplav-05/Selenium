global using NUnit.Framework;
global using OpenQA.Selenium;
global using OpenQA.Selenium.Remote;
global using Allure.Net.Commons;
global using Allure.NUnit;
global using Allure.NUnit.Attributes;
global using NUnit.Framework.Interfaces;

[assembly: Parallelizable(ParallelScope.All)]
[assembly: LevelOfParallelism(3)]