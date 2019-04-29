using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        #region All



       
        [Test]
        public void All_NoEntitiesInDatabase_ReturnsEmptyList()
        {
            var entities = _uut.All();
            Assert.IsEmpty(entities);
        }

        [Test]
        public void All_1EntityInDatabase_ReturnsEntity()
        {
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(new Customer());
                context.SaveChanges();
            }

            var entities = _uut.All();
            Assert.That(entities.Count,Is.EqualTo(1));
        }

        [Test]
        public void All_2EntityInDatabase_Returns2Entity()
        {
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(new Customer());
                context.Customers.Add(new Customer());
                context.SaveChanges();
            }

            var entities = _uut.All();
            Assert.That(entities.Count, Is.EqualTo(2));
        }

        #endregion

        #region Find

        

            
        [Test]
        public void Find_NoEntitiesInDatabase_ReturnsEmptyList()
        {
            var entities = _uut.Find();
            Assert.IsEmpty(entities);
        }

        [Test]
        public void Find_1EntityInDatabase_ReturnsEntity()
        {
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(new Customer());
                context.SaveChanges();
            }

            var entities = _uut.Find();
            Assert.That(entities.Count, Is.EqualTo(1));
        }

        [Test]
        public void Find_2EntityInDatabase_Returns2Entity()
        {
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(new Customer());
                context.Customers.Add(new Customer());
                context.SaveChanges();
            }

            var entities = _uut.Find();
            Assert.That(entities.Count, Is.EqualTo(2));
        }

        [Test]
        public void Find_2EntityInDatabaseFindsOne_ReturnsMadasEntity()
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

            var entities = _uut.Find(entity => entity.Name == "Madas");
            Assert.That(entities.Count, Is.EqualTo(1));
        }
        #endregion

        #region FindOnlyOne

        

            
        [Test]
        public void FindOnlyOne_2EntityInDatabaseFindsOne_ReturnsMadasEntity()
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

            var entities = _uut.FindOnlyOne(entity => entity.Name == "Madas");
            Assert.That(entities.Name, Is.EqualTo("Madas"));
        }

        [Test]
        public void FindOnlyOne_2EntityInDatabaseMatchingQuery_ThrowsException()
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

            Assert.Throws<UserIdInvalidException>(()=>_uut.FindOnlyOne(entity => entity.Name == "Madas"));   
        }

        #endregion

        #region FindById
            
        [Test]
        public void FindByID_FoundEntityFromElementInDb_ReturnsCustomer()
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
            
            Assert.That(_uut.FindByID(customer.Id).Name, Is.EqualTo(customer.Name));
        }

        [Test]
        public void FindByID_CustomerIdNotValid_ThrowsException()
        {
            Assert.Throws<UserIdInvalidException>(() => _uut.FindByID("Invalid Id"));
        }
        #endregion

        #region Add

        [Test]
        public void Add_EntityAdded_EntityAddedToDatabase()
        {
            var customer = new Customer()
            {
                Name = "Madas"
            };
            _uut.Add(customer);
            //To test
            _context.SaveChanges();

            //In a seperate instance validate that changes have been commited
            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.Count(),Is.EqualTo(1));
            }
        }

        [Test]
        public void Add_EntityAdded_EntityWithValidNameAdded()
        {
            var customer = new Customer()
            {
                Name = "Madas"
            };
            _uut.Add(customer);
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
        public void Delete_DeletedElementFromDatabasee_DeletesTheElement()
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
        public void Delete_DeletedElementFromDatabaseeFromId_DeletesTheElement()
        {
            var customer = new Customer();
            using (var context = _factory.CreateContext())
            {
                context.Customers.Add(customer);
                context.SaveChanges();
            }
            _uut.Delete(customer.Id);
            _context.SaveChanges();

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.Count(), Is.EqualTo(0));
            }
        }

        [Test]
        public void Delete_DeleteOnInvalidId_ThrowsException()
        {
            Assert.Throws<UserIdInvalidException>(()=> _uut.Delete("Invalid Id"));
        }

        #endregion

        #region Update

        [Test]
        public void Update_UpdateElement_DatabaseIsUpdated()
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
            _uut.Update(customer);
            _context.SaveChanges();

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.Find(customer.Id).Name,Is.EqualTo(customer.Name));
            }
        }

        [Test]
        public void Update_UpdateAddedElement_DatabaseIsUpdated()
        {
            var customer = new Customer()
            {
                Name = "Madas"
            };

            _uut.Add(customer);
            customer.Name = "MadasUpdated";
            _uut.Update(customer);
            _context.SaveChanges();

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Customers.Find(customer.Id).Name, Is.EqualTo(customer.Name));
            }
        }

        #endregion
    }
}
