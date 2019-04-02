﻿// <auto-generated />
using System;
using Api.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Api.DataAccessLayer.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Api.DataAccessLayer.Models.CustomerRides", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CustomerId");

                    b.Property<string>("CustomerId1");

                    b.Property<int>("RideId");

                    b.Property<int>("TaxiCompanyId");

                    b.Property<string>("TaxiCompanyId1");

                    b.Property<string>("status");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId1");

                    b.HasIndex("RideId");

                    b.HasIndex("TaxiCompanyId1");

                    b.ToTable("CustomerRides");
                });

            modelBuilder.Entity("Api.DataAccessLayer.Models.MatchedRides", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RideStatus");

                    b.HasKey("Id");

                    b.ToTable("MatchedRides");
                });

            modelBuilder.Entity("Api.DataAccessLayer.Models.Ride", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CountPassengers");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("DepartureTime");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int>("EndDestinationId");

                    b.Property<DateTime>("LatestConfirmed");

                    b.Property<int>("Price");

                    b.Property<int>("RideStatus");

                    b.Property<int>("StartDestinationId");

                    b.HasKey("Id");

                    b.ToTable("Rides");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Ride");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Api.DataAccessLayer.Models.SharedOpenRide", b =>
                {
                    b.HasBaseType("Api.DataAccessLayer.Models.Ride");

                    b.Property<int>("MatchedRidesId");

                    b.HasIndex("MatchedRidesId");

                    b.HasDiscriminator().HasValue("SharedOpenRide");
                });

            modelBuilder.Entity("Api.DataAccessLayer.Models.SoloRide", b =>
                {
                    b.HasBaseType("Api.DataAccessLayer.Models.Ride");

                    b.HasDiscriminator().HasValue("SoloRide");
                });

            modelBuilder.Entity("Api.DataAccessLayer.Models.Customer", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasDiscriminator().HasValue("Customer");
                });

            modelBuilder.Entity("Api.DataAccessLayer.Models.TaxiCompany", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("TaxiCompany_Name");

                    b.HasDiscriminator().HasValue("TaxiCompany");
                });

            modelBuilder.Entity("Api.DataAccessLayer.Models.CustomerRides", b =>
                {
                    b.HasOne("Api.DataAccessLayer.Models.Customer", "Customer")
                        .WithMany("CustomerRides")
                        .HasForeignKey("CustomerId1");

                    b.HasOne("Api.DataAccessLayer.Models.Ride", "Ride")
                        .WithMany()
                        .HasForeignKey("RideId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Api.DataAccessLayer.Models.TaxiCompany", "TaxiCompany")
                        .WithMany("CustomerRides")
                        .HasForeignKey("TaxiCompanyId1");
                });

            modelBuilder.Entity("Api.DataAccessLayer.Models.Ride", b =>
                {
                    b.OwnsOne("Api.DataAccessLayer.Models.Address", "EndDestination", b1 =>
                        {
                            b1.Property<int>("RideId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("CityName");

                            b1.Property<int>("PostalCode");

                            b1.Property<string>("StreetName");

                            b1.Property<int>("StreetNumber");

                            b1.HasKey("RideId");

                            b1.ToTable("Rides");

                            b1.HasOne("Api.DataAccessLayer.Models.Ride")
                                .WithOne("EndDestination")
                                .HasForeignKey("Api.DataAccessLayer.Models.Address", "RideId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("Api.DataAccessLayer.Models.Address", "StartDestination", b1 =>
                        {
                            b1.Property<int>("RideId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("CityName");

                            b1.Property<int>("PostalCode");

                            b1.Property<string>("StreetName");

                            b1.Property<int>("StreetNumber");

                            b1.HasKey("RideId");

                            b1.ToTable("Rides");

                            b1.HasOne("Api.DataAccessLayer.Models.Ride")
                                .WithOne("StartDestination")
                                .HasForeignKey("Api.DataAccessLayer.Models.Address", "RideId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Api.DataAccessLayer.Models.SharedOpenRide", b =>
                {
                    b.HasOne("Api.DataAccessLayer.Models.MatchedRides", "MatchedRides")
                        .WithMany("SharedOpenRides")
                        .HasForeignKey("MatchedRidesId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
