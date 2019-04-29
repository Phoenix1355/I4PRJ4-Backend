using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.DataAccessLayer.Interfaces;
using Api.DataAccessLayer.Models;
using Api.DataAccessLayer.Repositories;
using Api.DataAccessLayer.UnitOfWork;
using Api.DataAccessLayer.UnitTests.Factories;
using CustomExceptions;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Api.DataAccessLayer.UnitTests.Repositories
{
    [TestFixture]
    public class GenericRepositoryTests
    {
        #region Setup
        //Arbitary entity choosen, customer. 
        private IGenericRepository<Customer> _uut;
        private ApplicationContext _context;
        private InMemorySqlLiteContextFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new InMemorySqlLiteContextFactory();
            //Store context to ensure it's possible to save changes for testing. 
            _context = _factory.CreateContext();
            _uut = new GenericRepository<Customer>(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }
        #endregion
        #region AllAsync



       
        [Test]
        public async Task All_NoEntitiesInDatabase_ReturnsEmptyList()
        {
            var entities = await _uut.AllAsync();
            Assert.IsEmpty(entities);
        }

        [Test]
        public async Task All_1EntityInDatabase_ReturnsEntity()
        {
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(new Customer());
                context.SaveChanges();
            }

            var entities = await _uut.AllAsync();
            Assert.That(entities.Count,Is.EqualTo(1));
        }

        [Test]
        public async Task All_2EntityInDatabase_Returns2Entity()
        {
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(new Customer());
                context.Customers.Add(new Customer());
                context.SaveChanges();
            }

            var entities = await _uut.AllAsync();
            Assert.That(entities.Count, Is.EqualTo(2));
        }

        #endregion

        #region FindAsync

        

            
        [Test]
        public async Task Find_NoEntitiesInDatabase_ReturnsEmptyList()
        {
            var entities = await _uut.FindAsync();
            Assert.IsEmpty(entities);
        }

        [Test]
        public async Task Find_1EntityInDatabase_ReturnsEntity()
        {
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(new Customer());
                context.SaveChanges();
            }

            var entities = await _uut.FindAsync();
            Assert.That(entities.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task Find_2EntityInDatabase_Returns2Entity()
        {
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(new Customer());
                context.Customers.Add(new Customer());
                context.SaveChanges();
            }

            var entities = await _uut.FindAsync();
            Assert.That(entities.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Find_2EntityInDatabaseFindsOne_ReturnsMadasEntity()
        {
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(new Customer()
                {
                    Name = "Madas"
                });
                context.Customers.Add(new Customer());
                context.SaveChanges();
            }

            var entities = await _uut.FindAsync(entity => entity.Name == "Madas");
            Assert.That(entities.Count, Is.EqualTo(1));
        }
        #endregion

        #region FindOnlyOneAsync

        

            
        [Test]
        public async Task FindOnlyOne_2EntityInDatabaseFindsOne_ReturnsMadasEntity()
        {
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(new Customer()
                {
                    Name = "Madas"
                });
                context.Customers.Add(new Customer());
                context.SaveChanges();
            }

            var entities = await _uut.FindOnlyOneAsync(entity => entity.Name == "Madas");
            Assert.That(entities.Name, Is.EqualTo("Madas"));
        }

        [Test]
        public async Task FindOnlyOne_2EntityInDatabaseMatchingQuery_ThrowsException()
        {
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(new Customer()
                {
                    Name = "Madas"
                });
                context.Customers.Add(new Customer()
                {
                    Name = "Madas"
                });
                context.SaveChanges();
            }

            Assert.ThrowsAsync<UserIdInvalidException>(async()=>await _uut.FindOnlyOneAsync(entity => entity.Name == "Madas"));   
        }

        #endregion

        #region FindById
            
        [Test]
        public async Task FindByID_FoundEntityFromElementInDb_ReturnsCustomer()
        {
            var customer = new Customer()
            {
                Name = "Madas"
            };
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customer);
                context.SaveChanges();
            }

            var customerDB = await _uut.FindByIDAsync(customer.Id);


            Assert.That(customerDB.Name, Is.EqualTo(customer.Name));
        }

        [Test]
        public async Task FindByID_CustomerIdNotValid_ThrowsException()
        {
            Assert.ThrowsAsync<UserIdInvalidException>(async() => await _uut.FindByIDAsync("Invalid Id"));
        }
        #endregion

        #region AddAsync

        [Test]
        public async Task Add_EntityAdded_EntityAddedToDatabase()
        {
            var customer = new Customer()
            {
                Name = "Madas"
            };
            await _uut.AddAsync(customer);
            //To test
            _context.SaveChanges();

            //In a seperate instance validate that changes have been commited
            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.Count(),Is.EqualTo(1));
            }
        }

        [Test]
        public async Task Add_EntityAdded_EntityWithValidNameAdded()
        {
            var customer = new Customer()
            {
                Name = "Madas"
            };
            await _uut.AddAsync(customer);
            //To test
            _context.SaveChanges();

            //In a seperate instance validate that changes have been commited
            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.First().Name, Is.EqualTo("Madas"));
            }
        }

        #endregion

        #region Delete

        [Test]
        public async Task Delete_DeletedElementFromDatabasee_DeletesTheElement()
        {
            var customer = new Customer();
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customer);
                context.SaveChanges();
            }
            _uut.Delete(customer);
            _context.SaveChanges();

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.Count(),Is.EqualTo(0));
            }
        }

        [Test]
        public async Task Delete_DeletedElementFromDatabaseeFromId_DeletesTheElement()
        {
            var customer = new Customer();
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customer);
                context.SaveChanges();
            }
            await _uut.DeleteAsync(customer.Id);
            _context.SaveChanges();

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.Count(), Is.EqualTo(0));
            }
        }

        [Test]
        public async Task Delete_DeleteOnInvalidId_ThrowsException()
        {
            Assert.ThrowsAsync<UserIdInvalidException>(async ()=> await _uut.DeleteAsync("Invalid Id"));
        }

        #endregion

        #region Update

        [Test]
        public async Task Update_UpdateElement_DatabaseIsUpdated()
        {
            var customer = new Customer()
            {
                Name = "Madas"
            };
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customer);
                context.SaveChanges();
            }

            customer.Name = "MadasUpdated";
            await _uut.UpdateAsync(customer);
            _context.SaveChanges();

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.Find(customer.Id).Name,Is.EqualTo(customer.Name));
            }
        }

        [Test]
        public async Task Update_UpdateAddedElement_DatabaseIsUpdated()
        {
            var customer = new Customer()
            {
                Name = "Madas"
            };

            await _uut.AddAsync(customer);
            customer.Name = "MadasUpdated";
            await _uut.UpdateAsync(customer);
            _context.SaveChanges();

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.Find(customer.Id).Name, Is.EqualTo(customer.Name));
            }
        }

        #endregion
    }
}
