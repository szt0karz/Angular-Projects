﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PortfolioManagerApi.Data;

#nullable disable

namespace PortfolioManagerApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        // Budowanie modelu bazy danych
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            // Definicja tabeli "Projects"
            modelBuilder.Entity("PortfolioManagerApi.Models.PortfolioProject", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
                b.Property<string>("DemoUrl").IsRequired().HasColumnType("nvarchar(max)");
                b.Property<string>("Description").IsRequired().HasColumnType("nvarchar(max)");
                b.Property<DateTime?>("EndDate").HasColumnType("datetime2");
                b.Property<string>("RepositoryUrl").IsRequired().HasColumnType("nvarchar(max)");
                b.Property<DateTime>("StartDate").HasColumnType("datetime2");
                b.Property<string>("Status").IsRequired().HasColumnType("nvarchar(max)");
                b.Property<string>("Title").IsRequired().HasMaxLength(200).HasColumnType("nvarchar(200)");

                b.HasKey("Id");
                b.ToTable("Projects");
            });

            // Definicja tabeli pośredniej "ProjectTechnologies"
            modelBuilder.Entity("PortfolioManagerApi.Models.ProjectTechnology", b =>
            {
                b.Property<Guid>("ProjectId").HasColumnType("uniqueidentifier");
                b.Property<Guid>("TechnologyId").HasColumnType("uniqueidentifier");

                b.HasKey("ProjectId", "TechnologyId");
                b.HasIndex("TechnologyId");
                b.ToTable("ProjectTechnologies");
            });

            // Definicja tabeli "Technologies"
            modelBuilder.Entity("PortfolioManagerApi.Models.Technology", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");
                b.Property<string>("Name").IsRequired().HasColumnType("nvarchar(max)");

                b.HasKey("Id");
                b.ToTable("Technologies");
            });

            // Relacje między tabelami
            modelBuilder.Entity("PortfolioManagerApi.Models.ProjectTechnology", b =>
            {
                b.HasOne("PortfolioManagerApi.Models.PortfolioProject", "Project")
                    .WithMany("Technologies")
                    .HasForeignKey("ProjectId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("PortfolioManagerApi.Models.Technology", "Technology")
                    .WithMany("Projects")
                    .HasForeignKey("TechnologyId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("PortfolioManagerApi.Models.PortfolioProject", b =>
            {
                b.Navigation("Technologies");
            });

            modelBuilder.Entity("PortfolioManagerApi.Models.Technology", b =>
            {
                b.Navigation("Projects");
            });
        }
    }
}
