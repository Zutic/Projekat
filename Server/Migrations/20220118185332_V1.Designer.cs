﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;

namespace Projekat.Migrations
{
    [DbContext(typeof(AgencijaContext))]
    [Migration("20220118185332_V1")]
    partial class V1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Models.Grad", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BrojNekretnina")
                        .HasColumnType("int");

                    b.Property<int>("BrojStanovnika")
                        .HasColumnType("int");

                    b.Property<string>("Naziv")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Gradovi");
                });

            modelBuilder.Entity("Models.Kompanija", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Naziv")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Kompanije");
                });

            modelBuilder.Entity("Models.Nekretnina", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("GraditeljID")
                        .HasColumnType("int");

                    b.Property<int?>("LokacijaID")
                        .HasColumnType("int");

                    b.Property<double>("PocetnaCena")
                        .HasColumnType("float");

                    b.Property<string>("Slika")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Tip")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("GraditeljID");

                    b.HasIndex("LokacijaID");

                    b.ToTable("Nekretnina");
                });

            modelBuilder.Entity("Models.Stanovnik", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("GradStanovanjaID")
                        .HasColumnType("int");

                    b.Property<string>("Ime")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("JMBG")
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<double>("Novac")
                        .HasColumnType("float");

                    b.Property<string>("Prezime")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.HasIndex("GradStanovanjaID");

                    b.ToTable("Stanovnici");
                });

            modelBuilder.Entity("Models.Ugovor", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Cena")
                        .HasColumnType("float");

                    b.Property<int?>("KupacID")
                        .HasColumnType("int");

                    b.Property<int?>("ObjekatID")
                        .HasColumnType("int");

                    b.Property<int>("Procenat")
                        .HasColumnType("int");

                    b.Property<int?>("ProdavacID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("KupacID");

                    b.HasIndex("ObjekatID");

                    b.HasIndex("ProdavacID");

                    b.ToTable("Ugovori");
                });

            modelBuilder.Entity("Models.UgovorKompanije", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Cena")
                        .HasColumnType("float");

                    b.Property<int?>("KupacID")
                        .HasColumnType("int");

                    b.Property<int>("NekretninaFK")
                        .HasColumnType("int");

                    b.Property<int?>("ProdavacID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("KupacID");

                    b.HasIndex("NekretninaFK")
                        .IsUnique();

                    b.HasIndex("ProdavacID");

                    b.ToTable("UgovoriKompanija");
                });

            modelBuilder.Entity("Models.Nekretnina", b =>
                {
                    b.HasOne("Models.Kompanija", "Graditelj")
                        .WithMany()
                        .HasForeignKey("GraditeljID");

                    b.HasOne("Models.Grad", "Lokacija")
                        .WithMany()
                        .HasForeignKey("LokacijaID");

                    b.Navigation("Graditelj");

                    b.Navigation("Lokacija");
                });

            modelBuilder.Entity("Models.Stanovnik", b =>
                {
                    b.HasOne("Models.Grad", "GradStanovanja")
                        .WithMany()
                        .HasForeignKey("GradStanovanjaID");

                    b.Navigation("GradStanovanja");
                });

            modelBuilder.Entity("Models.Ugovor", b =>
                {
                    b.HasOne("Models.Stanovnik", "Kupac")
                        .WithMany()
                        .HasForeignKey("KupacID");

                    b.HasOne("Models.Nekretnina", "Objekat")
                        .WithMany("NekretninaUgovori")
                        .HasForeignKey("ObjekatID");

                    b.HasOne("Models.Stanovnik", "Prodavac")
                        .WithMany()
                        .HasForeignKey("ProdavacID");

                    b.Navigation("Kupac");

                    b.Navigation("Objekat");

                    b.Navigation("Prodavac");
                });

            modelBuilder.Entity("Models.UgovorKompanije", b =>
                {
                    b.HasOne("Models.Stanovnik", "Kupac")
                        .WithMany("StanovnikUgovoriKompanije")
                        .HasForeignKey("KupacID");

                    b.HasOne("Models.Nekretnina", "Objekat")
                        .WithOne("NekretninaUgovorKompanije")
                        .HasForeignKey("Models.UgovorKompanije", "NekretninaFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Kompanija", "Prodavac")
                        .WithMany("KompanijaUgovoriKompanije")
                        .HasForeignKey("ProdavacID");

                    b.Navigation("Kupac");

                    b.Navigation("Objekat");

                    b.Navigation("Prodavac");
                });

            modelBuilder.Entity("Models.Kompanija", b =>
                {
                    b.Navigation("KompanijaUgovoriKompanije");
                });

            modelBuilder.Entity("Models.Nekretnina", b =>
                {
                    b.Navigation("NekretninaUgovori");

                    b.Navigation("NekretninaUgovorKompanije");
                });

            modelBuilder.Entity("Models.Stanovnik", b =>
                {
                    b.Navigation("StanovnikUgovoriKompanije");
                });
#pragma warning restore 612, 618
        }
    }
}
