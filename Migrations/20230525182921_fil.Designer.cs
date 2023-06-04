﻿// <auto-generated />
using System;
using AgroExpressAPI.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AgroExpressAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230525182921_fil")]
    partial class fil
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AgroExpressAPI.Entities.Address", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FullAddress")
                        .HasColumnType("longtext");

                    b.Property<string>("LocalGovernment")
                        .HasColumnType("longtext");

                    b.Property<string>("State")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Addresses");

                    b.HasData(
                        new
                        {
                            Id = "cc7578e3-52a9-49e9-9788-2da54df19f38",
                            FullAddress = "10,Abayomi street,Ipaja,lagos",
                            LocalGovernment = "Alimosho",
                            State = "Lagos"
                        });
                });

            modelBuilder.Entity("AgroExpressAPI.Entities.Admin", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Admins");

                    b.HasData(
                        new
                        {
                            Id = "37846734-732e-4149-8cec-6f43d1eb3f60",
                            UserId = "37846734-732e-4149-8cec-6f43d1eb3f60"
                        });
                });

            modelBuilder.Entity("AgroExpressAPI.Entities.Buyer", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Buyers");
                });

            modelBuilder.Entity("AgroExpressAPI.Entities.Farmer", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Ranking")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Farmers");
                });

            modelBuilder.Entity("AgroExpressAPI.Entities.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("AvailabilityDateFrom")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("AvailabilityDateTo")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FarmerEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("FarmerId")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("FarmerRank")
                        .HasColumnType("int");

                    b.Property<string>("FarmerUserName")
                        .HasColumnType("longtext");

                    b.Property<string>("FirstDimentionPicture")
                        .HasColumnType("longtext");

                    b.Property<string>("ForthDimentionPicture")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Measurement")
                        .HasColumnType("longtext");

                    b.Property<double>("Price")
                        .HasColumnType("double");

                    b.Property<string>("ProductLocalGovernment")
                        .HasColumnType("longtext");

                    b.Property<string>("ProductName")
                        .HasColumnType("longtext");

                    b.Property<string>("ProductState")
                        .HasColumnType("longtext");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("SecondDimentionPicture")
                        .HasColumnType("longtext");

                    b.Property<string>("ThirdDimentionPicture")
                        .HasColumnType("longtext");

                    b.Property<int>("ThumbDown")
                        .HasColumnType("int");

                    b.Property<int>("ThumbUp")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FarmerId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("AgroExpressAPI.Entities.RequestedProduct", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("BuyerEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("BuyerId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("BuyerLocalGovernment")
                        .HasColumnType("longtext");

                    b.Property<string>("BuyerPhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("FarmerEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("FarmerId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FarmerName")
                        .HasColumnType("longtext");

                    b.Property<string>("FarmerNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("Haspaid")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsAccepted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDelivered")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("NotDelivered")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("OrderStatus")
                        .HasColumnType("tinyint(1)");

                    b.Property<double>("Price")
                        .HasColumnType("double");

                    b.Property<string>("ProductName")
                        .HasColumnType("longtext");

                    b.Property<double>("Quantity")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.HasIndex("FarmerId");

                    b.ToTable("RequestedProducts");
                });

            modelBuilder.Entity("AgroExpressAPI.Entities.Transaction", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<double>("Amount")
                        .HasColumnType("double");

                    b.Property<string>("BuyerEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("DateCreated")
                        .HasColumnType("longtext");

                    b.Property<string>("FarmerEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("ReferenceNumber")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("AgroExpressAPI.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("AddressId")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Due")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("Gender")
                        .HasColumnType("longtext");

                    b.Property<bool>("Haspaid")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsRegistered")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("longtext");

                    b.Property<string>("Role")
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "37846734-732e-4149-8cec-6f43d1eb3f60",
                            AddressId = "cc7578e3-52a9-49e9-9788-2da54df19f38",
                            DateCreated = new DateTime(2023, 5, 25, 19, 29, 21, 100, DateTimeKind.Local).AddTicks(2491),
                            Due = true,
                            Email = "tijaniadebayoabdllahi@gmail.com",
                            Gender = "Male",
                            Haspaid = true,
                            IsActive = true,
                            IsRegistered = true,
                            Name = "Adebayo Addullah",
                            Password = "$2b$10$g/pHoM0xbbZrImtT71rioeRfKVCSLaEwdOuZrUnioqXf4tuZ7Ltv2",
                            PhoneNumber = "08087054632",
                            Role = "Admin",
                            UserName = "Modrator"
                        });
                });

            modelBuilder.Entity("AgroExpressAPI.Entities.Admin", b =>
                {
                    b.HasOne("AgroExpressAPI.Entities.User", "User")
                        .WithOne("Admin")
                        .HasForeignKey("AgroExpressAPI.Entities.Admin", "UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AgroExpressAPI.Entities.Buyer", b =>
                {
                    b.HasOne("AgroExpressAPI.Entities.User", "User")
                        .WithOne("Buyer")
                        .HasForeignKey("AgroExpressAPI.Entities.Buyer", "UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AgroExpressAPI.Entities.Farmer", b =>
                {
                    b.HasOne("AgroExpressAPI.Entities.User", "User")
                        .WithOne("Farmer")
                        .HasForeignKey("AgroExpressAPI.Entities.Farmer", "UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AgroExpressAPI.Entities.Product", b =>
                {
                    b.HasOne("AgroExpressAPI.Entities.Farmer", "Farmer")
                        .WithMany("Products")
                        .HasForeignKey("FarmerId");

                    b.Navigation("Farmer");
                });

            modelBuilder.Entity("AgroExpressAPI.Entities.RequestedProduct", b =>
                {
                    b.HasOne("AgroExpressAPI.Entities.Buyer", "Buyer")
                        .WithMany("RequestedProducts")
                        .HasForeignKey("BuyerId");

                    b.HasOne("AgroExpressAPI.Entities.Farmer", "Farmer")
                        .WithMany("RequestedProducts")
                        .HasForeignKey("FarmerId");

                    b.Navigation("Buyer");

                    b.Navigation("Farmer");
                });

            modelBuilder.Entity("AgroExpressAPI.Entities.User", b =>
                {
                    b.HasOne("AgroExpressAPI.Entities.Address", "Address")
                        .WithOne("User")
                        .HasForeignKey("AgroExpressAPI.Entities.User", "AddressId");

                    b.Navigation("Address");
                });

            modelBuilder.Entity("AgroExpressAPI.Entities.Address", b =>
                {
                    b.Navigation("User");
                });

            modelBuilder.Entity("AgroExpressAPI.Entities.Buyer", b =>
                {
                    b.Navigation("RequestedProducts");
                });

            modelBuilder.Entity("AgroExpressAPI.Entities.Farmer", b =>
                {
                    b.Navigation("Products");

                    b.Navigation("RequestedProducts");
                });

            modelBuilder.Entity("AgroExpressAPI.Entities.User", b =>
                {
                    b.Navigation("Admin");

                    b.Navigation("Buyer");

                    b.Navigation("Farmer");
                });
#pragma warning restore 612, 618
        }
    }
}